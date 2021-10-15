// -------------------------------------
// CsvAnalyze
// Analyze csv data return a CsvDefinition,
// infer settings, dateformat, columns, widths etc. from input data,
// -------------------------------------
using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVLint.Tools;
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace CSVLint
{
    class CsvAnalyze
    {
        private const int MAX_UNIQUE_VALUES = 15;

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
        public static CsvDefinition InferFromData()
        {
            // First do a letter frequency analysis on each row
            var strfreq = ScintillaStreams.StreamAllText();
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
            while ((line = strfreq.ReadLine()) != null)
            {
                // letter freq per line
                var letterFrequency = new Dictionary<char, int>();

                // line length
                lineLengths.Increase(line.Length);

                // end-of-line also technically counts as a column start/end, else the last columns width determined incorrectly
                wordStarts.Increase(line.Length);

                // process characters in this line
                int spaces = 0, c = 0, num = -1;
                foreach (var chr in line)
                {
                    letterFrequency.Increase(chr);
                    occurrences.Increase(chr);

                    if (chr == '"') inQuotes = !inQuotes;
                    else if (!inQuotes)
                    {
                        letterFrequencyQuoted.Increase(chr);
                        occurrencesQuoted.Increase(chr);
                    }

                    // check fixed columns
                    int newcol = 0;
                    if (chr == ' ')
                    {
                        // more than 2 spaces could indicate new column
                        if (++spaces > 1) bigSpaces.Increase((c + 1));

                        // one single space after a digit might be a new column
                        if (num == 1)
                        {
                            wordStarts.Increase(c);
                            num = 0;
                        }
                    }
                    else
                    {
                        // more than 2 spaces could indicate new column
                        if (spaces > 1) newcol = 1;
                        spaces = 0;

                        // switch between alpha and numeric characters could indicate new column
                        int checknum = ("0123456789".IndexOf(chr));
                        // ignore characters that can be both numeric or alpha values example "A.B." or "Smith-Johnson"
                        int ignore = (".-+".IndexOf(chr));
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
                        if (newcol == 1) wordStarts.Increase(c);
                    }

                    // next character
                    c++;
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

            strfreq.Dispose();

            // check the variance on the frequency of each char
            var variances = new Dictionary<char, float>();
            foreach (var key in occurrences.Keys)
            {
                var mean = (float)occurrences[key] / lineCount;
                float variance = 0;
                foreach (var frequency in frequencies)
                {
                    var f = 0;
                    if (frequency.ContainsKey(key)) f = frequency[key];
                    variance += (f - mean) * (f - mean);
                }
                variance /= lineCount;
                variances.Add(key, variance);
            }

            // check variance on frequency of quoted chars(?)
            var variancesQuoted = new Dictionary<char, float>();
            foreach (var key in occurrencesQuoted.Keys)
            {
                var mean = (float)occurrencesQuoted[key] / linesQuoted;
                float variance = 0;
                foreach (var frequency in frequenciesQuoted)
                {
                    var f = 0;
                    if (frequency.ContainsKey(key)) f = frequency[key];
                    variance += (f - mean) * (f - mean);
                }
                variance /= lineCount;
                variancesQuoted.Add(key, variance);
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

                // widths contain line positions, convert to individual column widths, example pos [8, 14, 15, 22, 25] -> widths [8, 6, 1, 7, 3]
                var pos1 = 0;
                for (var i = 0; i < foundfieldWidths.Count; i++)
                {
                    // next column end pos, last column gets the rest
                    int pos2 = foundfieldWidths[i];

                    // positions to column widths
                    foundfieldWidths[i] = pos2 - pos1;
                    pos1 = pos2;
                }

                result.FieldWidths = foundfieldWidths;
            }

            // -----------------------------------------------------------------------------
            // determine data types for columns
            // -----------------------------------------------------------------------------

            // reset string reader to first line is not possible, create a new one
            bool fixedwidth = (result.Separator == '\0');

            var strdata = ScintillaStreams.StreamAllText();

            // examine data and keep statistics for each column
            List<CsvAnalyzeColumn> colstats = new List<CsvAnalyzeColumn>();
            //List<CsvColumStats> colstats = new List<CsvColumStats>();
            lineCount = 0;

            while (!strdata.EndOfStream)
            {
                // keep track of how many lines
                lineCount++;

                List<string> values = result.ParseNextLine(strdata);

                // inspect all values
                for (int i = 0; i < values.Count(); i++)
                {
                    // add columnstats if needed
                    if (i > colstats.Count() - 1)
                    {
                        colstats.Add(new CsvAnalyzeColumn(i));
                    }

                    int fixedLength = -1;
                    if (fixedwidth) fixedLength = (i < result.FieldWidths.Count ? result.FieldWidths[i] : values[i].Length);

                    // next value to evaluate
                    colstats[i].InputData(values[i], fixedLength, false);
                }
            }

            strdata.Dispose();

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

            // determine if the first row was actually header names
            int count = 0;
            var csvvalid = new CsvValidate();
            foreach (CSVLint.CsvColumn col in result.Fields)
            {
                // if fist row values (=Names) are all valid datatypes, then probably not actually column names
                var str = csvvalid.EvaluateDataValue(col.Name, col, col.Index);
                if (str != "") count++;
            }

            // if all header Names (=frst row) comply with the column datatype, then there is no column names
            result.ColNameHeader = (count > 0);

            // if no header column names rename all columns to "FIELD1", "FIELD2", "FIELD3" etc.
            if (!result.ColNameHeader)
            {
                foreach (CSVLint.CsvColumn col in result.Fields) col.Name = string.Format("FIELD{0}", (col.Index + 1));
            }

            // result
            return result;
        }

        private static char GetSeparatorFromVariance(Dictionary<char, float> variances, Dictionary<char, int> occurrences, int lineCount, out int uncertancy)
        {
            //var preferredSeparators = "\t,;|";
            var preferredSeparators = Main.Settings._charSeparators;
            uncertancy = 0;

            // The char with lowest variance is most likely the separator
            // Optimistic: check prefered with 0 variance 
            var separator = variances
                .Where(x => x.Value == 0f && preferredSeparators.IndexOf(x.Key) != -1)
                .OrderByDescending(x => occurrences[x.Key])
                .Select(x => (char?)x.Key)
                .FirstOrDefault();

            // The char with lowest variance is most likely the separator
            // Optimistic: check prefered with 0 variance 
            //var separator = variances
            //    .Where(x => x.Value == 0f && preferredSeparators.IndexOf(x.Key) != -1)
            //    .OrderByDescending(x => occurrences[x.Key])
            //    .Select(x => (char?)x.Key)
            //    .FirstOrDefault();

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

        /// <summary>
        /// Data statistical analysis report
        /// <param name="data"></param>
        /// <returns></returns>
        public static void StatisticalReportData(CsvDefinition csvdef)
        {
            // examine data and keep statistics for each column
            List<CsvAnalyzeColumn> colstats = new List<CsvAnalyzeColumn>();

            //List<CsvColumStats> colstats = new List<CsvColumStats>();
            int lineCount = 0;
            bool fixedwidth = (csvdef.Separator == '\0');

            var strdata = ScintillaStreams.StreamAllText();

            List<string> values;

            while (!strdata.EndOfStream)
            {
                // keep track of how many lines
                lineCount++;

                values = csvdef.ParseNextLine(strdata);

                // inspect all values
                for (int i = 0; i < values.Count(); i++)
                {
                    // add columnstats if needed
                    if (i > colstats.Count() - 1)
                    {
                        colstats.Add(new CsvAnalyzeColumn(i));
                    }

                    int fixedLength = -1;
                    //if (fixedwidth) fixedLength = (i < result.FieldWidths.Count ? result.FieldWidths[i] : values[i].Length);

                    // next value to evaluate
                    colstats[i].InputData(values[i], fixedLength, true);
                }
            }

            strdata.Dispose();

            // if first row is header column names, count one less line for totals
            if (csvdef.ColNameHeader) lineCount--;

            StringBuilder sb = new StringBuilder();

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            string strhead = (csvdef.ColNameHeader ? " (+1 header line)" : "");

            sb.Append(string.Format("CSV Lint plug-in v{0}\r\n", Main.GetVersion()));
            sb.Append("Analyze dataset\r\n");
            sb.Append(string.Format("File: {0}\r\n", FILE_NAME));
            sb.Append(string.Format("Date: {0}\r\n", DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
            sb.Append(String.Format("Data records: {0}{1}\r\n", lineCount, strhead));
            sb.Append(String.Format("Max.unique values: {0}\r\n", MAX_UNIQUE_VALUES));
            sb.Append("\r\n");

			// goal output, depending on data found:
			// -------------------------------------
			// 2: Fieldname
			// DataTypes     : string (174 = 87,0%), empty (26 = 13,0%)
			// Width range   : 2 ~ 6 characters
			// Integer range : 123 ~ 999
			// Decimal range : 1,2 ~ 34,56
			// Date range    : 01/01/2021 ~ 12/31/2021
			// -- Unique values --
			// n=123         : YES
			// n=45          : NO
			// n=6           : UNKOWN

            // add columns as actual fields
            int idx = 0;
            foreach (CsvAnalyzeColumn stats in colstats)
            {
                // next column
                idx++;
                sb.Append("----------------------------------------\r\n");
                sb.Append(String.Format("{0}: {1}\r\n", idx, stats.Name));

                // count date types that were found
                sb.Append("DataTypes      : ");
                if (stats.CountDecimal  > 0) sb.Append(String.Format( "decimal ({0} = {1}%), ", stats.CountDecimal,  ReportPercentage(stats.CountDecimal,  lineCount)));
                if (stats.CountInteger  > 0) sb.Append(String.Format( "integer ({0} = {1}%), ", stats.CountInteger,  ReportPercentage(stats.CountInteger,  lineCount)));
                if (stats.CountString   > 0) sb.Append(String.Format(  "string ({0} = {1}%), ", stats.CountString,   ReportPercentage(stats.CountString,   lineCount)));
                if (stats.CountDateTime > 0) sb.Append(String.Format("datetime ({0} = {1}%), ", stats.CountDateTime, ReportPercentage(stats.CountDateTime, lineCount)));
                if (stats.CountEmpty    > 0) sb.Append(String.Format(   "empty ({0} = {1}%), ", stats.CountEmpty,    ReportPercentage(stats.CountEmpty,    lineCount)));
                sb.Length -= 2; // remove last ", "
                sb.Append("\r\n");

                // width range
                if (stats.MaxWidth > 0)
                {
                    var strwid = (stats.MinWidth == stats.MaxWidth ? stats.MaxWidth.ToString() : String.Format("{0} ~ {1}", stats.MinWidth, stats.MaxWidth));
                    sb.Append(String.Format("Width range    : {0} characters\r\n", strwid));
                }

                // minimum maximum values
                if (stats.CountInteger  > 0) sb.Append(String.Format("Integer range  : {0} ~ {1}\r\n", stats.stat_minint_org,  stats.stat_maxint_org));
                if (stats.CountDecimal  > 0) sb.Append(String.Format("Decimal range  : {0} ~ {1}\r\n", stats.stat_mindbl_org,  stats.stat_maxdbl_org));
                if (stats.CountDateTime > 0) sb.Append(String.Format("DateTime range : {0} ~ {1}\r\n", stats.stat_mindat_org,  stats.stat_maxdat_org));

                // if coded variable, unique values 
                if ((stats.stat_uniquecount.Count > 0) && (stats.stat_uniquecount.Count <= MAX_UNIQUE_VALUES))
                {
                    // apply sorting, note that obj.Key actually contains the column value(s) and obj.Value contains the unique counter
                    stats.stat_uniquecount = stats.stat_uniquecount.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                    //stats.stat_uniquecount = stats.stat_uniquecount.OrderByDescending(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                    //stats.stat_uniquecount = stats.stat_uniquecount.OrderBy(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
                    //stats.stat_uniquecount = stats.stat_uniquecount.OrderByDescending(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);

                    // list all unique values
                    sb.Append(String.Format("-- Unique values ({0}) --\r\n", stats.stat_uniquecount.Count));
                    foreach (var uqval in stats.stat_uniquecount)
                    {
                        String strcount = uqval.Value.ToString();
                        sb.Append(String.Format("n={0}: {1}\r\n", strcount.PadRight(13, ' '), uqval.Key));
                    }
                }

                sb.Append("\r\n");
            }
            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
        }
        /// <summary>
        /// Data statistical analysis report
        /// <param name="data"></param>
        /// <returns></returns>
        private static String ReportPercentage(int iPart, int iTotal)
        {
            Double dPerc = (iPart * 100.0 / iTotal);

            return dPerc.ToString("0.0");
        }

        /// <summary>
        /// Data statistical analysis report
        /// <param name="data"></param>
        /// <returns></returns>
        public static void CountUniqueValues(CsvDefinition csvdef, List<int> colidx, bool sortValue, bool sortDesc)
        {
            // examine data and keep list of counters per unique values
            Dictionary<String, int> uniquecount = new Dictionary<String, int>();
            var strdata = ScintillaStreams.StreamAllText();
            List<string> values;

            // if first line is header column names, then consume line and ignore
            if (csvdef.ColNameHeader) csvdef.ParseNextLine(strdata);

            // read all data lines
            while (!strdata.EndOfStream)
            {
                // get next line of values
                values = csvdef.ParseNextLine(strdata);
                String uniq = "";

                // get unique value(s) from column indexes
                for (int i = 0; i < colidx.Count(); i++)
                {
                    // add columnstats if needed
                    int col = colidx[i];
                    uniq += (i > 0 ? csvdef.Separator.ToString() : "") + (col < values.Count ? values[col] : "");
                }

                // count unique value(s)
                if (!uniquecount.ContainsKey(uniq))
                    uniquecount.Add(uniq, 1);
                else
                    uniquecount[uniq]++;
            }
            strdata.Dispose();

            // output unique values and count to new file
            StringBuilder sb = new StringBuilder();

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // get column names
            for (int i = 0; i < colidx.Count(); i++)
            {
                var colname = (colidx[i] < csvdef.Fields.Count ? csvdef.Fields[colidx[i]].Name : "");
                sb.Append(String.Format("{0}{1}", colname, csvdef.Separator));
            }
            sb.Append("count_unique\r\n");

            // apply sorting, note that obj.Key actually contains the column value(s) and obj.Value contains the unique counter
            if ((sortValue == true ) && (sortDesc == false)) uniquecount = uniquecount.OrderBy          (obj => obj.Key  ).ToDictionary(obj => obj.Key, obj => obj.Value);
            if ((sortValue == true ) && (sortDesc == true )) uniquecount = uniquecount.OrderByDescending(obj => obj.Key  ).ToDictionary(obj => obj.Key, obj => obj.Value);
            if ((sortValue == false) && (sortDesc == false)) uniquecount = uniquecount.OrderBy          (obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
            if ((sortValue == false) && (sortDesc == true )) uniquecount = uniquecount.OrderByDescending(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);

            // add all unique values, sort by count
            foreach (KeyValuePair<String, int> unqcnt in uniquecount)
            {
                sb.Append(String.Format("{0}{1}{2}\r\n", unqcnt.Key, csvdef.Separator, unqcnt.Value));
            }

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
        }
    }
}
