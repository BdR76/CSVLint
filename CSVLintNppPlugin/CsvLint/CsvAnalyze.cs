// -------------------------------------
// CsvAnalyze
// Analyze csv data return a CsvDefinition,
// infer settings, dateformat, columns, widths etc. from input data,
// -------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVLint.Tools;

namespace CSVLint
{
    class CsvAnalyze
    {
        private class CsvColumStats
        {
            public string Name = "";
            public int MinWidth = 9999;
            public int MaxWidth = 0;
            public int CountString = 0;
            public int CountInteger = 0;
            public int CountDecimal = 0;
            public int CountDecimalComma = 0;
            public int CountDecimalPoint = 0;
            public int DecimalDigMax = 0; // maximum digits, example "1234.5" = 4 digits
            public int DecimalDecMax = 0; // maximum decimals, example "123.45" = 2 decimals
            public int CountDateTime = 0;
            public char DateSep = '\0';
            public int DateMax1 = 0;
            public int DateMax2 = 0;
            public int DateMax3 = 0;
        }

        /// <summary>
        /// Infer CSV definition from data; determine separators, column names, datatypes etc
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CsvDefinition InferFromData(string data)
        {
            // First do a letter frequency analysis on each row
            var s = new StringReader(data);
            string line;
            int lineCount = 0, linesQuoted = 0;

            // -----------------------------------------------------------------------------
            // determine separator character or fixed length
            // -----------------------------------------------------------------------------

            // statistics about this data
            var frequencies = new List<Dictionary<char, int>>();        // frequencies of letters per line
            var occurrences = new Dictionary<char, int>();              // occurences of letters in entire dataset
            var frequenciesQuoted = new List<Dictionary<char, int>>();  // frequencies of quoted letters per line
            var occurrencesQuoted = new Dictionary<char, int>();        // occurences of quoted letters in entire dataset
            var bigSpaces = new Dictionary<int, int>();                 // fixed width data; more than 1 spaces
            var wordStarts = new Dictionary<int, int>();                // fixed width data; places after space where characters starts again, or switch from numeric to alphacharacter
            var inQuotes = false;
            var letterFrequencyQuoted = new Dictionary<char, int>();
            var lineLengths = new Dictionary<int, int>();

            // analyse individual character frequencies
            while ((line = s.ReadLine()) != null)
            {
                // letter freq per line
                var letterFrequency = new Dictionary<char, int>();

                // line length
                lineLengths.Increase(line.Length);

                // process characters in this line
                int spaces = 0, i = 0, num = -1;
                foreach (var c in line)
                {
                    letterFrequency.Increase(c);
                    occurrences.Increase(c);

                    if (c == '"') inQuotes = !inQuotes;
                    else if (!inQuotes)
                    {
                        letterFrequencyQuoted.Increase(c);
                        occurrencesQuoted.Increase(c);
                    }

                    // check fixed columns
                    int newcol = 0;
                    if (c == ' ')
                    {
                        // more than 2 spaces could indicate new column
                        if (++spaces > 1) bigSpaces.Increase((i + 1));

                        // one single space after a digit might be a new column
                        if (num == 1)
                        {
                            wordStarts.Increase(i);
                            num = 0;
                        }
                    }
                    else
                    {
                        // more than 2 spaces could indicate new column
                        if (spaces > 1) newcol = 1;
                        spaces = 0;

                        // switch between alpha and numeric characters could indicate new column
                        int checknum = ("0123456789".IndexOf(c));
                        // ignore characters that can be both numeric or alpha values example "A.B." or "Smith-Johnson"
                        int ignore = (".-+".IndexOf(c));
                        if (ignore < 0)
                        {
                            if (checknum < 0)
                            {
                                if (num == 1) newcol = 1;
                                num = 0;
                            }
                            else
                            {
                                if (num == 0) newcol = 1;
                                num = 1;
                            };
                        };
                        // new column found
                        if (newcol == 1) wordStarts.Increase(i);
                    }

                    // next character
                    i++;
                }

                frequencies.Add(letterFrequency);
                if (!inQuotes)
                {
                    frequenciesQuoted.Add(letterFrequencyQuoted);
                    letterFrequencyQuoted = new Dictionary<char, int>();
                    linesQuoted++;
                }

                // stop after 20 lines
                if (lineCount++ > 20) break;
            }

            // check the variance on the frequency of each char
            var variances = new Dictionary<char, float>();
            foreach (var c in occurrences.Keys)
            {
                var mean = (float)occurrences[c] / lineCount;
                float variance = 0;
                foreach (var frequency in frequencies)
                {
                    var f = 0;
                    if (frequency.ContainsKey(c)) f = frequency[c];
                    variance += (f - mean) * (f - mean);
                }
                variance /= lineCount;
                variances.Add(c, variance);
            }

            // check variance on frequency of quoted chars(?)
            var variancesQuoted = new Dictionary<char, float>();
            foreach (var c in occurrencesQuoted.Keys)
            {
                var mean = (float)occurrencesQuoted[c] / linesQuoted;
                float variance = 0;
                foreach (var frequency in frequenciesQuoted)
                {
                    var f = 0;
                    if (frequency.ContainsKey(c)) f = frequency[c];
                    variance += (f - mean) * (f - mean);
                }
                variance /= lineCount;
                variancesQuoted.Add(c, variance);
            }

            // get separator
            char Separator = GetSeparatorFromVariance(variances, occurrences, lineCount, out var uncertancy);

            // The char with lowest variance is most likely the separator
            CsvDefinition result = new CsvDefinition(Separator);

            //var Separator = GetSeparatorFromVariance(variances, occurrences, lineCount, out var uncertancy);
            var separatorQuoted = GetSeparatorFromVariance(variancesQuoted, occurrencesQuoted, linesQuoted, out var uncertancyQuoted);
            if (uncertancyQuoted < uncertancy)
                result.Separator = separatorQuoted;
            else if (uncertancy < uncertancyQuoted || (uncertancy == uncertancyQuoted && lineCount > linesQuoted)) // It was better ignoring quotes!
                result.TextQualifier = '\0';

            // head column name
            result.ColNameHeader = (result.Separator != '\0');

            // Exception, probably not tabular data file
            if ( (result.Separator == '\0') && ( (lineLengths.Count > 1) || (lineCount <= 1) ) )
            {
                // check for typical XML characters
                var xml1 = (occurrences.ContainsKey('>') ? occurrences['>'] : 0);
                var xml2 = (occurrences.ContainsKey('<') ? occurrences['<'] : 0);

                // check for binary characters, chr(31) or lower and not TAB
                var bin = occurrences.Where(x => (int)x.Key < 32 && (int)x.Key != 9).Sum(x => x.Value);

                // set filetype as first column name, as a hint to user
                var guess = "Textfile";
                if (bin > 0) guess = "Binary";
                if ((xml1 > 0) && (xml1 == xml2)) guess = "XML";

                // add single column and bail!
                result.AddColumn(guess, 9999, ColumnType.String, "");
                return result;
            }

            // Failed to detect separator, could it be a fixed-width file?
            if (result.Separator == '\0')
            {
                // big spaces
                var commonSpace = bigSpaces.Where(x => x.Value == lineCount).Select(x => x.Key).OrderByDescending(x => x);
                var lastvalue = 0;
                int lastStart = 0;
                var foundfieldWidths = new List<int>();
                foreach (var space in commonSpace)
                {
                    if (space != lastvalue - 1)
                    {
                        foundfieldWidths.Add(space);
                        lastStart = space;
                    }
                    lastvalue = space;
                }

                // new columns or numeric/alpha 
                var commonBreaks = wordStarts.Where(x => x.Value == lineCount).Select(x => x.Key).OrderBy(x => x);

                //foundfieldWidths.AddRange(commonBreaks); // AddRange simply adds duplicates

                // only add unique breaks
                foreach (var br in commonBreaks)
                    if (!foundfieldWidths.Contains(br))
                        foundfieldWidths.Add(br);

                foundfieldWidths.Sort();

                if (foundfieldWidths.Count < 3) return result; // unlikely fixed width
                foundfieldWidths.Add(-1); // Last column gets "the rest"
                result.FieldWidths = foundfieldWidths;
            }

            // -----------------------------------------------------------------------------
            // determine data types for columns
            // -----------------------------------------------------------------------------

            // reset string reader to first line is not possible, create a new one
            s = new StringReader(data);

            // examine data and keep statistics for each column
            List<CsvAnalyzeColumn> colstats = new List<CsvAnalyzeColumn>();
            //List<CsvColumStats> colstats = new List<CsvColumStats>();
            lineCount = 0;

            // examine data
            while ((line = s.ReadLine()) != null)
            {
                // keep track of how many lines
                lineCount++;

                // get values from line
                List<string> values = new List<string>();
                if (result.Separator == '\0')
                {
                    // fixed width columns
                    int pos1 = 0;
                    for (int i = 0; i < result.FieldWidths.Count(); i++)
                    {
                        // if line is too short, columns missing?
                        if (pos1 > line.Length) break;

                        // next column end pos, last column gets the rest
                        int pos2 = result.FieldWidths[i];
                        if (pos2 < 0) pos2 = line.Length;

                        // get column value
                        string val = line.Substring(pos1, pos2 - pos1);
                        values.Add(val);
                        pos1 = pos2;
                    }
                }
                else
                {
                    // delimited columns
                    values = line.Split(result.Separator).ToList();
                }

                // inspect all values
                for (int i = 0; i < values.Count(); i++)
                {
                    // add columnstats if needed
                    if (i > colstats.Count() - 1) colstats.Add(new CsvAnalyzeColumn(i));

                    // next value to evaluate
                    colstats[i].InputData(values[i]);
                }
            }

            // add columns as actual fields
            int idx = 0;
            foreach (CsvAnalyzeColumn stats in colstats)
            {
                // get data type up
                CSVLint.CsvColumn col = stats.InferDatatype();

                // add column
                result.AddColumn(idx, col.Name, col.MaxWidth, col.DataType, col.Mask);

                idx++;
            }

            // result
            return result;
        }

        private static char GetSeparatorFromVariance(Dictionary<char, float> variances, Dictionary<char, int> occurrences, int lineCount, out int uncertancy)
        {
            //var preferredSeparators = Main.Settings.Separators.Replace("\\t", "\t");
            var preferredSeparators = "\t,;|";
            uncertancy = 0;

            // The char with lowest variance is most likely the separator
            // Optimistic: check prefered with 0 variance 
            var separator = variances
                .Where(x => x.Value == 0f && preferredSeparators.IndexOf(x.Key) != -1)
                .OrderByDescending(x => occurrences[x.Key])
                .Select(x => (char?)x.Key)
                .FirstOrDefault();

            if (separator != null)
                return separator.Value;

            uncertancy++;
            var defaultKV = default(KeyValuePair<char, float>);

            // Ok, no perfect separator. Check if the best char that exists on all lines is a prefered separator
            var sortedVariances = variances.OrderBy(x => x.Value).ToList();
            var best = sortedVariances.FirstOrDefault(x => occurrences[x.Key] >= lineCount);
            if (!best.Equals(defaultKV) && preferredSeparators.IndexOf(best.Key) != -1)
                return best.Key;
            uncertancy++;

            // No? Second best?
            best = sortedVariances.Where(x => occurrences[x.Key] >= lineCount).Skip(1).FirstOrDefault();
            if (!best.Equals(defaultKV) && preferredSeparators.IndexOf(best.Key) != -1)
                return best.Key;
            uncertancy++;

            // Ok, screw the preferred separators, is any other char a perfect separator? (and common, i.e. at least 3 per line)
            separator = variances
                .Where(x => x.Value == 0f && occurrences[x.Key] >= lineCount * 2)
                .OrderByDescending(x => occurrences[x.Key])
                .Select(x => (char?)x.Key)
                .FirstOrDefault();
            if (separator != null)
                return separator.Value;

            uncertancy++;

            // Ok, I have no idea
            return '\0';
        }
    }
}
