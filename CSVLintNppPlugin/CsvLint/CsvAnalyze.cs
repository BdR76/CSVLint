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
using CSVLint.Tools;
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace CSVLint
{
    class CsvAnalyze
    {
        /// <summary>
        /// Infer CSV definition from data; determine separators, column names, datatypes etc
        /// </summary>
        /// <param name="autodetect">automatically detect separator and header names</param>
        /// <param name="mansep">Override automatic detection, manually provided column separator</param>
        /// <param name="manhead">Override automatic detection, manually set first header row contains columns names</param>
        /// <param name="userRequested">if the inference was explicitly requested by the user (rather than auto-triggered on opening a file)</param>
        /// <returns></returns>
        public static CsvDefinition InferFromData(bool autodetect, char mansep, string manwid, bool manhead, int manskip, char commchar, bool userRequested)
        {
            // First do a letter frequency analysis on each row
            if (!ScintillaStreams.TryStreamAllText(out StreamReader strfreq, userRequested))
            {
                var csvdef = new CsvDefinition { FileIsTooBig = true };
                return csvdef;
            }
            string line;
            int lineCount = 0, linesQuoted = 0, lineContent = 0;

            // manual skip lines parameter
            int skipdetect = 0;
            char commentdetect = '\0';

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
                // count all lines, empty or not
                lineCount++;

                var firstchar = (!string.IsNullOrEmpty(line) ? line[0] : '\0');

                // check for comment character
                if (firstchar == commchar)
                {
                    line = "";
                    if (commentdetect == '\0') commentdetect = firstchar;
                }

                // automatically detect comment lines
                if (autodetect)
                {
                    // Skip lines must be consecutive lines at start of file, not some empty line in the middle of the data.
                    // check for empty lines or lines that start with the skip character
                    if ((skipdetect == lineCount - 1) && (line.Trim() == "")) skipdetect++;
                }
                else
                {
                    // manually skip first X lines
                    if (lineCount <= manskip) line = "";
                }

                // ignore empty lines
                if (line.Trim() != "") { 
                    // letter freq per line
                    var letterFrequency = new Dictionary<char, int>();

                    // line length
                    lineLengths.Increase(line.Length);

                    // end-of-line also technically counts as a column start/end, else the last columns width determined incorrectly
                    wordStarts.Increase(line.Length);

                    // process characters in this line
                    int spaces = 0, pos = 0, num = -1; // num  0 = text value  1 = numeric value  -1 = currently unknown
                    foreach (var chr in line)
                    {
                        // check for separators
                        letterFrequency.Increase(chr);
                        occurrences.Increase(chr);

                        // string value in quotes, treat as one continuous value, i.e. do not check content for column breaks
                        if (chr == Main.Settings.DefaultQuoteChar) inQuotes = !inQuotes;
                        else if (!inQuotes)
                        {
                            letterFrequencyQuoted.Increase(chr);
                            occurrencesQuoted.Increase(chr);

                            // check fixed columns
                            int newcol = 0;
                            if (chr == ' ')
                            {
                                // a space after a numeric value
                                if (num == 1)
                                {
                                    num = -1; // reset text/numeric indicator because probably a new column
                                    spaces++; // count single space as "big space" i.e. 2 spaces or more
                                }

                                // 2 spaces or more could indicate new column
                                if (++spaces > 1) bigSpaces.Increase(pos + 1);
                            }
                            else
                            {
                                // 2 spaces or more could indicate new column
                                if (spaces > 1) newcol = 1;
                                spaces = 0;

                                // switch between alpha and numeric characters could indicate new column
                                int isdigit = "0123456789".IndexOf(chr);
                                // ignore characters that can be both numeric or alpha values example "A.B." or "Smith-Johnson"
                                // Digits connected by certain characters can be one single column, not multiple,
                                // For example date, time, telephonenr "12:34","555-890-4327" etc.
                                int ignore = ",.-+:/\\".IndexOf(chr);
                                if (ignore < 0)
                                {
                                    if (isdigit < 0)
                                    {
                                        if (num == 1) newcol = 1;
                                        num = 0; // currently in text value
                                    }
                                    else
                                    {
                                        if (num == 0) newcol = 1;
                                        num = 1; // currently in numeric value
                                    };
                                };
                                // new column found
                                if (newcol == 1) wordStarts.Increase(pos);
                            }
                        }

                        // next character position
                        pos++;
                    }

                    frequencies.Add(letterFrequency);
                    if (!inQuotes)
                    {
                        frequenciesQuoted.Add(letterFrequencyQuoted);
                        letterFrequencyQuoted = new Dictionary<char, int>();
                        linesQuoted++;
                    }

                    // stop after 20 lines. Exception; quoted string values can also contain newline char,
                    // do not stop inspecting halfway through a quoted string because that would mis-count frequency of separator characters
                    if ( (++lineContent > 20 - 1) && (!inQuotes) ) break;
                }
            }

            strfreq.Dispose();

            // check the variance on the frequency of each char
            var variances = new Dictionary<char, float>();
            foreach (var key in occurrences.Keys)
            {
                var mean = (float)occurrences[key] / lineContent;
                float variance = 0;
                foreach (var frequency in frequencies)
                {
                    var f = 0;
                    if (frequency.ContainsKey(key)) f = frequency[key];
                    variance += (f - mean) * (f - mean);
                }
                variance /= lineContent;
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
                variance /= lineContent;
                variancesQuoted.Add(key, variance);
            }

            // get separator
            char Separator = GetSeparatorFromVariance(variances, occurrences, lineContent, out var uncertancy);

            // The char with lowest variance is most likely the separator
            CsvDefinition result = new CsvDefinition(Separator);

            //var Separator = GetSeparatorFromVariance(variances, occurrences, lineCount, out var uncertancy);
            var separatorQuoted = GetSeparatorFromVariance(variancesQuoted, occurrencesQuoted, linesQuoted, out var uncertancyQuoted);
            if (uncertancyQuoted < uncertancy)
                result.Separator = separatorQuoted;
            else if (uncertancy < uncertancyQuoted || (uncertancy == uncertancyQuoted && lineContent > linesQuoted)) // It was better ignoring quotes!
                result.TextQualifier = '\0';

            // Override auto-detection, user sets column separator manually
            if (!autodetect) result.Separator = mansep;

            // head column name
            result.ColNameHeader = result.Separator != '\0';

            // Auto-detect from data or manual override, user sets column header names manually
            if (autodetect)
            {
                // auto-detect
                result.SkipLines = skipdetect;
                result.CommentChar = commentdetect; // only if detected in data
            }
            else
            {
                // manual override
                result.ColNameHeader = manhead;
                result.SkipLines = manskip;
                result.CommentChar = commchar; // manual, no matter if it was detected or not
            }

            // Exception, probably not tabular data file
            if ( (result.Separator == '\0') && ( (lineLengths.Count > 1) || (lineContent <= 1) ) )
            {
                // check for typical XML characters
                var xml1 = occurrences.ContainsKey('>') ? occurrences['>'] : 0;
                var xml2 = occurrences.ContainsKey('<') ? occurrences['<'] : 0;

                // check for binary characters, chr(31) or lower and not TAB
                var bin = occurrences.Where(x => (int)x.Key < 32 && (int)x.Key != 9).Sum(x => x.Value);

                // set filetype as first column name, as a hint to user
                var guess = "Textfile";
                if (bin > 0) guess = "Binary";
                if ((xml1 > 0) && (xml1 == xml2)) guess = "XML";

                // add single column and bail!
                result.AddColumn(guess, 9999, ColumnType.String, "");
                result.FieldWidths = new List<int> { 9999 };

                return result;
            }

            // Failed to detect separator, could it be a fixed-width file?
            if (result.Separator == '\0')
            {
                // Sort big spaces descending, i.e. go from end of the line to the beginning
                var commonSpace = bigSpaces.Where(x => x.Value >= lineContent-1).Select(x => x.Key).OrderByDescending(x => x);
                var lastvalue = 0;
                int lastStart = 0;
                var foundfieldWidths = new List<int>();

                // set widths manually
                var manwidint = manwid.Split(',');
                foreach (var pos in manwidint)
                {
                    if (int.TryParse(pos, out int wid))
                    {
                        // set widths manually
                        foundfieldWidths.Add(wid);
                    }
                }

                // detect widths automatically
                if (autodetect || (foundfieldWidths.Count == 0))
                {
                    // Go backwards and find the end positions of "big spaces", i.e. the right-most part of consecutive spaces
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
                    var commonBreaks = wordStarts.Where(x => x.Value == lineContent).Select(x => x.Key).OrderBy(x => x);

                    //foundfieldWidths.AddRange(commonBreaks); // AddRange simply adds duplicates

                    // only add unique breaks
                    foreach (var br in commonBreaks)
                        if (!foundfieldWidths.Contains(br))
                            foundfieldWidths.Add(br);
                }
                foundfieldWidths.Sort();
                if (foundfieldWidths.Count < 3) return result; // unlikely fixed width

                // widths now contain column end positions, convert to individual column widths, example pos [8, 14, 15, 22, 25] -> widths [8, 6, 1, 7, 3]
                var pos1 = 0;
                for (var i = 0; i < foundfieldWidths.Count; i++)
                {
                    // next column end pos, last column gets the rest
                    int pos2 = foundfieldWidths[i];

                    // positions to column widths
                    foundfieldWidths[i] = (pos2 - pos1);

                    pos1 = pos2;
                }

                result.FieldWidths = foundfieldWidths;
            }

            // -----------------------------------------------------------------------------
            // determine data types for columns
            // -----------------------------------------------------------------------------

            // reset string reader to first line is not possible, create a new one
            bool fixedwidth = result.Separator == '\0';

            ScintillaStreams.TryStreamAllText(out var strdata);

            // examine data and keep statistics for each column
            List<CsvAnalyzeColumn> colstats = new List<CsvAnalyzeColumn>();
            lineContent = 0;

            // skip any comment lines
            result.SkipCommentLinesAtStart(strdata);

            while (!strdata.EndOfStream)
            {
                // keep track of how many lines
                lineContent++;

                List<string> values = result.ParseNextLine(strdata, out bool iscomm);

                // skip comment lines
                if (!iscomm)
                {
                    // inspect all values
                    for (int i = 0; i < values.Count; i++)
                    {
                        // add columnstats if needed, assume first row contains the correct number of columns, i.e. do not add columns due to errors in data
                        if (lineContent == 1 && (i > colstats.Count - 1))
                        {
                            colstats.Add(new CsvAnalyzeColumn(i));
                        }

                        // value index within column indexes, i.e. don't evaluate "extra columns data" due to errors in data, for example unclosed " quotes, superfluous separator characters etc.
                        if (i <= colstats.Count - 1)
                        {
                            // next value to evaluate
                            colstats[i].InputData(values[i], false);
                        }
                    }
                }
            }

            strdata.Dispose();

            // add columns as actual fields
            int idx = 0;
            foreach (CsvAnalyzeColumn stats in colstats)
            {
                // get data type up
                CSVLint.CsvColumn col = stats.InferDatatype();

                // Fixed width, values can be shorter than column width or even empty, so re-apply all columns widths
                if ( (fixedwidth) && (idx < result.FieldWidths.Count) ) {
                    col.MaxWidth = result.FieldWidths[idx];
                }

                // add column
                result.AddColumn(idx, col.Name, col.MaxWidth, col.DataType, col.Mask);

                // Check for coded values, only when datatype is string or integer
                // i.e. do not treat datetime/float values as coded values
                if ((col.DataType == ColumnType.String) || (col.DataType == ColumnType.Integer))
                {
                    result.Fields[idx].AddCodedValues(stats.stat_uniquecount);
                }

                idx++;
            }

            // determine if the first row was actually header names
            int count = 0;
            bool allstrings = true;
            bool emptyname = false;
            var csvvalid = new CsvValidate();
            foreach (var namcol in result.Fields)
            {
                // don't check for invalid string, because only "invalid string" is too long which doesn't say much about wether it's a header or not
                if (namcol.DataType != ColumnType.String)
                {
                    // not all columns are datatype String
                    allstrings = false;

                    // if value in first row (=Names) is not of valid datatype, then first row probably contains column names
                    var str = csvvalid.EvaluateDataValue(namcol.Name, namcol, namcol.Index);
                    if (str != "") count++;
                }

                // if value in first row (=Names) is empty then probably not header names
                // Note: always Trim() and ignore settings.TrimValues for name detection
                if (namcol.Name.Trim() == "") emptyname = true;

                // TODO: carriage returns in header name not supported
                // replace with space so that schema.ini can at least be validated
                namcol.Name = namcol.Name.Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ');
            }

            // if all header Names (=frst row) comply with the column datatype, then there is no column names
            result.ColNameHeader = ( (allstrings || count > 0) && !emptyname);

            // Override auto-detection, user sets column header names manually
            if (!autodetect) result.ColNameHeader = manhead;

            // if no header column names rename all columns to "FIELD1", "FIELD2", "FIELD3" etc.
            if (!result.ColNameHeader)
            {
                foreach (CsvColumn col in result.Fields) col.Name = string.Format("FIELD{0}", col.Index + 1);
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
            // Optimistic: check prefered with 0 variance (0f = floating point 0)
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
            // Bugfix; Looking outside the preferred separators will in practise lead to choosing a random letter, like the letter E, as the separator which makes no sense
            //separator = variances
            //    .Where(x => x.Value == 0f && occurrences[x.Key] >= lineCount * 2)
            //    .OrderByDescending(x => occurrences[x.Key])
            //    .Select(x => (char?)x.Key)
            //    .FirstOrDefault();
            //if (separator != null)
            //    return separator.Value;
            //
            //uncertancy++;

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

            int lineCount = 0;
            bool fixedwidth = csvdef.Separator == '\0';

            if (!ScintillaStreams.TryStreamAllText(out var strdata))
                return;

            // skip any comment lines
            int commentCount = csvdef.SkipCommentLinesAtStart(strdata);

            List<string> values;

            while (!strdata.EndOfStream)
            {
                values = csvdef.ParseNextLine(strdata, out bool iscomm);

                // skip comment lines
                if (iscomm)
                    commentCount++;
                else
                {
                    // keep track of how many lines
                    lineCount++;

                    // inspect all values
                    for (int i = 0; i < values.Count; i++)
                    {
                        // add columnstats if needed
                        if (i > colstats.Count - 1)
                        {
                            colstats.Add(new CsvAnalyzeColumn(i));
                            // if datetime field, the datetime format is already known
                            if ( (i < csvdef.Fields.Count) && (csvdef.Fields[i].DataType == ColumnType.DateTime) )
                            {
                                colstats[i].DateTimeFormatKnown(csvdef.Fields[i].Mask);
                            }
                        }

                        // next value to evaluate
                        colstats[i].InputData(values[i], true);
                    }
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
            string strhead = csvdef.ColNameHeader ? " (+1 header line)" : "";

            sb.Append("Analyze dataset\r\n");
            sb.Append(string.Format("File: {0}\r\n", FILE_NAME));
            sb.Append(string.Format("Date: {0}\r\n", DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
            sb.Append(string.Format("CSV Lint: v{0}\r\n\r\n", Main.GetVersion()));
            sb.Append(string.Format("Data records: {0}{1}\r\n", lineCount, strhead));
            if (commentCount > 0) sb.Append(string.Format("Comment lines total: {0}\r\n", commentCount));
            sb.Append(string.Format("Max.unique values: {0}\r\n", Main.Settings.UniqueValuesMax));
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

            // ChatGPT generated this LinQ code :D
            var duplicateNames = colstats.GroupBy(c => c.Name) // Group the columns by their Name property
                                 .Where(g => g.Count() > 1) // Only keep groups with more than one element (i.e. duplicates)
                                 .Select(g => new { Name = g.Key, HowMany = g.Count() }) // Select the Name and Count of each group as an anonymous type
                                 .ToList(); // Convert the result to a List of anonymous types

            // list any duplicate column names
            if (duplicateNames.Count > 0)
            {
                // list duplicate column names
                sb.Append(string.Format("**Warning: duplicate column names ({0}) **\r\n", duplicateNames.Count));

                foreach (var dup in duplicateNames)
                {
                    var dupcount = dup.HowMany.ToString();
                    sb.Append(string.Format("n={0}: {1}\r\n", dupcount.PadRight(13, ' '), dup.Name));
                }
                sb.Append("\r\n");
            }

            // add columns as actual fields
            int idx = 0;
            foreach (CsvAnalyzeColumn stats in colstats)
            {
                // next column
                idx++;
                sb.Append("----------------------------------------\r\n");
                sb.Append(string.Format("{0}: {1}\r\n", idx, stats.Name));

                // count date types that were found
                sb.Append("DataTypes      : ");
                if (stats.CountDecimal  > 0) sb.Append(string.Format( "decimal ({0} = {1}%), ", stats.CountDecimal,  ReportPercentage(stats.CountDecimal,  lineCount)));
                if (stats.CountInteger  > 0) sb.Append(string.Format( "integer ({0} = {1}%), ", stats.CountInteger,  ReportPercentage(stats.CountInteger,  lineCount)));
                if (stats.CountString   > 0) sb.Append(string.Format(  "string ({0} = {1}%), ", stats.CountString,   ReportPercentage(stats.CountString,   lineCount)));
                if (stats.CountDateTime > 0) sb.Append(string.Format("datetime ({0} = {1}%), ", stats.CountDateTime, ReportPercentage(stats.CountDateTime, lineCount)));
                if (stats.CountEmpty    > 0) sb.Append(string.Format(   "empty ({0} = {1}%), ", stats.CountEmpty,    ReportPercentage(stats.CountEmpty,    lineCount)));
                sb.Length -= 2; // remove last ", "
                sb.Append("\r\n");

                // width range
                if (stats.MaxWidth > 0)
                {
                    var strwid = stats.MinWidth == stats.MaxWidth ? stats.MaxWidth.ToString() : string.Format("{0} ~ {1}", stats.MinWidth, stats.MaxWidth);
                    sb.Append(string.Format("Width range    : {0} characters\r\n", strwid));
                }

                // minimum maximum values
                if (stats.CountInteger  > 0) sb.Append(string.Format("Integer range  : {0} ~ {1}\r\n", stats.stat_minint_org,  stats.stat_maxint_org));
                if (stats.CountDecimal  > 0) sb.Append(string.Format("Decimal range  : {0} ~ {1}\r\n", stats.stat_mindbl_org,  stats.stat_maxdbl_org));
                if (stats.CountDateTime > 0) sb.Append(string.Format("DateTime range : {0} ~ {1}\r\n", stats.stat_mindat_org,  stats.stat_maxdat_org));

                // if coded variable, unique values 
                if ((stats.stat_uniquecount.Count > 0) && (stats.stat_uniquecount.Count <= Main.Settings.UniqueValuesMax))
                {
                    // apply sorting, note that obj.Key actually contains the column value(s) and obj.Value contains the unique counter
                    stats.stat_uniquecount = stats.stat_uniquecount.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                    //stats.stat_uniquecount = stats.stat_uniquecount.OrderByDescending(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                    //stats.stat_uniquecount = stats.stat_uniquecount.OrderBy(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
                    //stats.stat_uniquecount = stats.stat_uniquecount.OrderByDescending(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);

                    // list all unique values
                    sb.Append(string.Format("-- Unique values ({0}) --\r\n", stats.stat_uniquecount.Count));
                    foreach (var uqval in stats.stat_uniquecount)
                    {
                        string strcount = uqval.Value.ToString();
                        sb.Append(string.Format("n={0}: {1}\r\n", strcount.PadRight(13, ' '), uqval.Key));
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
        private static string ReportPercentage(int iPart, int iTotal)
        {
            double dPerc = iPart * 100.0 / iTotal;

            return dPerc.ToString("0.0");
        }

        /// <summary>
        /// Data statistical analysis report
        /// <param name="data"></param>
        /// <returns></returns>
        public static void CountUniqueValues(CsvDefinition csvdef, List<int> colidx, bool sortBy, bool sortValue, bool sortDesc)
        {
            // examine data and keep list of counters per unique values
            Dictionary<string, int> uniquecount = new Dictionary<string, int>();
            if (!ScintillaStreams.TryStreamAllText(out var strdata))
                return;
            List<string> values;

            bool iscomm = false;
            // skip any comment lines
            csvdef.SkipCommentLinesAtStart(strdata);

            // if first line is header column names, then consume line and ignore
            if (csvdef.ColNameHeader) csvdef.ParseNextLine(strdata, out iscomm);

            // New separator same as old
            // Exception for fixed width, because code for fixed width output would be too complicated
            var newsep = (csvdef.Separator == '\0' ? ',' : csvdef.Separator);

            // read all data lines
            while (!strdata.EndOfStream)
            {
                // get next line of values
                values = csvdef.ParseNextLine(strdata, out iscomm);

                // skip comment lines
                if (!iscomm)
                {
                    string uniq = "";

                    // get unique value(s) from column indexes
                    for (int i = 0; i < colidx.Count; i++)
                    {
                        // get index of selected column 
                        int col = colidx[i];

                        // if value contains separator character then put value in quotes
                        var val = col < values.Count ? values[col] : "";
                        if (val.IndexOf(newsep) >= 0)
                        {
                            val = val.Replace("\"", "\"\"");
                            val = string.Format("\"{0}\"", val);
                        }

                        // concatenate selected column values as csv string, use same separator as source file
                        uniq += (i > 0 ? newsep.ToString() : "") + val;
                    }

                    // count unique value(s)
                    if (!uniquecount.ContainsKey(uniq))
                        uniquecount.Add(uniq, 1);
                    else
                        uniquecount[uniq]++;
                }
            }
            strdata.Dispose();

            // output unique values and count to new file
            StringBuilder sb = new StringBuilder();

            // new output unique values and count to new file
            CsvDefinition csvnew = new CsvDefinition(newsep);

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // get column names
            for (int i = 0; i < colidx.Count; i++)
            {
                // if column name contains separator character then put column name in quotes
                var colname = colidx[i] < csvdef.Fields.Count ? csvdef.Fields[colidx[i]].Name : "";
                if (colname.IndexOf(newsep) >= 0) colname = string.Format("\"{0}\"", colname);

                // new header
                sb.Append(string.Format("{0}{1}", colname, newsep));

                // new csv defintion
                csvnew.AddColumn(i, colname, csvdef.Fields[colidx[i]].MaxWidth, csvdef.Fields[colidx[i]].DataType, csvdef.Fields[colidx[i]].Mask);
            }
            sb.Append("count_unique\r\n");

            // if sorting
            if (sortBy)
            {
                // apply sorting, note that obj.Key actually contains the column value(s) and obj.Value contains the unique counter
                if ((sortValue == true ) && (sortDesc == false)) uniquecount = uniquecount.OrderBy          (obj => obj.Key  ).ToDictionary(obj => obj.Key, obj => obj.Value);
                if ((sortValue == true ) && (sortDesc == true )) uniquecount = uniquecount.OrderByDescending(obj => obj.Key  ).ToDictionary(obj => obj.Key, obj => obj.Value);
                if ((sortValue == false) && (sortDesc == false)) uniquecount = uniquecount.OrderBy          (obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
                if ((sortValue == false) && (sortDesc == true )) uniquecount = uniquecount.OrderByDescending(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
            }

            // add all unique values, sort by count
            var maxwidth = 0;
            foreach (KeyValuePair<string, int> unqcnt in uniquecount)
            {
                sb.Append(string.Format("{0}{1}{2}\r\n", unqcnt.Key, newsep, unqcnt.Value));
                if (maxwidth < unqcnt.Value) maxwidth = unqcnt.Value;
            }
            csvnew.AddColumn("count_unique", maxwidth.ToString().Length, ColumnType.Integer);

            // create new file
            notepad.FileNew();

            // add csv def for this new list
            //Main.CSVChangeFileTab();
            Main.UpdateCSVChanges(csvnew, false);

            // file content
            editor.SetText(sb.ToString());
        }
    }
}
