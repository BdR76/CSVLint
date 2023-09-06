// -------------------------------------
// CsvValidate
// Validate csv data based on <CsvDefinition>
// and report any data errors
// -------------------------------------

using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVLint
{
    class LogLine
    {
        public string Message;
        public int LineNumber;
        public int Severity; // -1 is info message, 0=warning, 1=error
        public LogLine(string msg)
        {
            this.Message = msg;
            this.LineNumber = -1;
            this.Severity = -1;
        }
        public LogLine(string msg, int linenr, int sev)
        {
            this.Message = msg;
            this.LineNumber = linenr;
            this.Severity = sev;
        }
    }

    class CsvValidate
    {
        private readonly List<LogLine> log;

        public CsvValidate()
        {
            this.log = new List<LogLine>();
        }

        /// <summary>
        /// validate csv data against the definition of a CsvDefinition 
        /// </summary>
        /// <param name="strdata"> csv data </param>
        public void ValidateData(StreamReader strdata, CsvDefinition csvdef)
        {
            // Exception: nothing to validate
            if ((csvdef.Fields.Count == 1) && (csvdef.Fields[0].DataType == ColumnType.String) && (csvdef.Fields[0].MaxWidth >= 9999))
            {
                // warning message
                string msg = string.Format("Nothing to inspect, not tabular data ({0}).", csvdef.Fields[0].Name);
                this.log.Add(new LogLine(msg, -1, -1));
                return;
            }

            // ChatGPT generated this LinQ code :D
            var duplicateNames = csvdef.Fields.GroupBy(c => c.Name) // Group the columns by their Name property
                                        .Where(g => g.Count() > 1) // Only keep groups with more than one element (i.e. duplicates)
                                        .Select(g => g.Key) // Select the Name property of each group
                                        .ToList(); // Convert the result to a List<string>

            // list any duplicate column names
            if (duplicateNames.Count > 0)
            {
                // list duplicate column names
                var msgdup = "";
                foreach (var dup in duplicateNames)
                {
                    //var dupcount = dup.HowMany.ToString();
                    msgdup += string.Format("{0}{1}", (msgdup == "" ? "" : ", "), dup);
                }

                string msg = string.Format("duplicate column names ({0})", msgdup);
                this.log.Add(new LogLine(msg, -1, 0));
            }

            // start line reader
            int counterr = 0;
            var dtStart = DateTime.Now;
            var lineCount = 0;

            // if fixed length what is max line length
            int fixedlength = 0;
            if (csvdef.Separator == '\0')
            {
                foreach (CsvColumn fld in csvdef.Fields)
                {
                    fixedlength += fld.MaxWidth;
                }
            }

            // skip any comment lines
            csvdef.SkipCommentLinesAtStart(strdata);

            // read all lines
            while (!strdata.EndOfStream)
            {
                // get values from line
                List<string> values = csvdef.ParseNextLine(strdata, out bool iscomm);
                lineCount++;

                // skip comment lines
                if (!iscomm) {

                    string err = "";

                    // too many or too few characters in line
                    //if (fixedlength != line.Length)
                    //{
                    //    int dif = line.Length - fixedlength;
                    //    err = string.Format("Line {0} character(s) too {1}, ", (dif > 0 ? dif : -1 * dif), (dif > 0 ? "long" : "short"));
                    //    counterr++;
                    //}

                    // too many or too few columns
                    if (values.Count != csvdef.Fields.Count)
                    {
                        err += string.Format("Too {0} columns, ", values.Count > csvdef.Fields.Count ? "many" : "few");
                        counterr++;
                    }

                    // check all values
                    for (var i = 0; i < values.Count; i++)
                    {
                        // next value and column number
                        string val = values[i];

                        // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
                        var valtrim = val.Trim();
                        if ((valtrim.Length > 0) && (valtrim[0] == Main.Settings.DefaultQuoteChar))
                        {
                            val = val.Trim();
                            val = val.Trim(Main.Settings.DefaultQuoteChar);
                        }

                        if (Main.Settings.TrimValues) val = val.Trim();

                        if (val != "")
                        {
                            // within bounds of column definition and non-empty value
                            if (i < csvdef.Fields.Count)
                            {
                                // column header or actual data value
                                if ( (lineCount == 1) && csvdef.ColNameHeader )
                                {
                                    // column header
                                    if (val != csvdef.Fields[i].Name)
                                    {
                                        err += string.Format("unexpected column name \"{0}\", ", val);
                                        counterr++;
                                    }
                                }
                                else
                                {
                                    // data values
                                    string evalerr = this.EvaluateDataValue(val, csvdef.Fields[i], i);
                                    if (evalerr != "")
                                    {
                                        err += evalerr;
                                        counterr++;
                                    }
                                }
                            }
                        }
                    }
               
                    // log any errors
                    if (err != "")
                    {
                        err = err.Remove(err.Length - 2); // remove last comma ", "
                        this.log.Add(new LogLine(err, csvdef.ParseCurrentLine, 1));
                    }
                }
            }

            var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

            // final ready message
            var line = string.Format("Inspected {0} lines, {1} data errors found, time elapsed {2}", lineCount, this.log.Count == 0 ? "no" : counterr.ToString(), dtElapsed);
            this.log.Add(new LogLine(line, -1, -1));
        }

        /// <summary>
        /// validate csv data against the definition of a CsvDefinition 
        /// </summary>
        /// <param name="data"> csv data </param>
        public string EvaluateDataValue(string val, CsvColumn coldef, int idx)
        {

            // error result
            string err = "";
            int colnr = idx + 1;

            // ignore null values
            if (val == Main.Settings.NullKeyword) val = "";

            // check if value is too long
            if (val.Length > coldef.MaxWidth)
            {
                err += string.Format("Column {0} value \"{1}\" is too long, ", colnr, val);
            }
            else {
                // validation based on datetype
                bool valid = true;
                string typ = "";
                string msg = "";

                // check coded values
                if (coldef.isCodedValue)
                {
                    if (coldef.CodedList.IndexOf(val) == -1)
                    {
                        msg = "is not a valid enumeration member";
                        valid = false;
                    }
                }

                // check valid data types
                if (valid)
                {
                    switch (coldef.DataType)
                    {
                        case ColumnType.Integer:
                            typ = "integer";
                            valid = EvaluateInteger(val);
                            break;
                        case ColumnType.Decimal:
                            typ = "decimal";
                            valid = EvaluateDecimal(val, coldef, out msg);
                            break;
                        case ColumnType.DateTime:
                            typ = "datetime";
                            valid = EvaluateDateTime(val, coldef, out msg);
                            break;
                    };
                }


                // report if value is invalid
                if (!valid)
                {
                    if (msg == "")
                    {
                        err += string.Format("Column {0} value \"{1}\" not a valid {2} value, ", colnr, val, typ);
                    }
                    else
                    {
                        err += string.Format("Column {0} value \"{1}\" {2}, ", colnr, val, msg);
                    };
                }
            }

            // result
            return err;
        }

        /// <summary>
        /// validate integer value
        /// Use custom function instead of using the standard `int.TryParse(val, out _);`
        /// which allows for max integer of 2147483647 (32bit) or 9223372036854775807 (64bit)
        /// 
        /// This custom function gives the same result regardless of 32bit/64bit bytecode
        /// and only depends on the `IntegerDigitsMax` setting
        /// so that it will also correctly detect bigint/large int and even googolplex values
        /// (For typical datasets this function also performs faster, though only slightly; 180ns vs 160ns)
        /// </summary>
        /// <param name="val"> integer value, examples "1", "23", "-456" etc.</param>
        public bool EvaluateInteger(string val)
        {
            val = val.Trim();

            bool isNumeric = true;
            int sign = 0;
            for (int i = 0; i < val.Length; i++)
            {
                char ch = val[i];
                if ((ch < 48) || (ch > 57)) // not '0'..'9'
                {
                    // integer with plus/minus allowed but only on very first position
                    if ((i == 0) && ((ch == 43) || (ch == 45))) // '+' = chr(43) '-' = chr(45)
                    {
                        sign = 1;
                    }
                    else
                    {
                        isNumeric = false;
                        break;
                    }
                }
            }

            // check max digits for integer value
            if (val.Length - sign > Main.Settings.IntegerDigitsMax) isNumeric = false;

            return isNumeric;
        }

        /// <summary>
        /// validate decimal value
        /// Use custom function instead of using the standard `float.TryParse(val, out _);`
        /// 
        /// This custom function gives the same result regardless of 32bit/64bit bytecode
        /// and only depends on the `DecimalDigitsMax` setting
        /// so that it will also correctly detect values with lots of decimals
        /// and also detect incorrect thousand separators for example "123,45,678.00"
        /// (For typical datasets this function also performs faster; 320ns vs 180ns)
        /// </summary>
        /// <param name="val"> decimal value, example "1.23", "-4,56", ".5" etc.</param>
        //private bool EvaluateDecimal(string val, CsvColumn coldef, out string err)
        public bool EvaluateDecimal(string val, CsvColumn coldef, out string err)
        {
            err = "";

            // cannot be converted to decimal
            //bool isFloat = Double.TryParse(val, out _);
            val = val.Trim();

            bool isDecimal = true;
            int digits = 0;
            int sign = -1; // -1 is no sign character
            int decsep = -1;
            int thosep = -1;

            for (int i = val.Length - 1; i >= 0; i--)
            {
                char ch = val[i];
                // digits '0' = chr(48) '9' = chr(57)
                if ((ch >= 48) && (ch <= 57))
                {
                    digits++;
                }
                else
                {
                    // decimal with plus/minus allowed but only on first position, '+' = chr(43) '-' = chr(45)
                    if ((ch == 43) || (ch == 45))
                    {
                        if (i > 0)
                        {
                            isDecimal = false;
                            break;
                        }
                        sign = i;
                    }
                    // decimal character, cannot appear more than once
                    else if ((decsep == -1) && (ch == coldef.sTag[1]))
                    {
                        // thousand seaparto before decimal
                        if (thosep != -1)
                        {
                            err += "incorrect position of thousand separator";
                            isDecimal = false;
                            break;
                        }
                        // check max decimal digits
                        if (digits > coldef.iTag)
                        {
                            isDecimal = false;
                            if (err != "") err += " and ";
                            err += "has too many decimals";
                        }
                        // position in string of decimal separator
                        decsep = i;
                        thosep = i; // decimal is also start point reference for thousand separators
                    }
                    // thousand separator, must be 3+1 characters apart example "12,345,678.00" but not "12,34,56.00"
                    else if (ch == coldef.sTag[0])
                    {
                        // allow values with thousand separators but no decimal separator, example "12,345"
                        if (thosep == -1) thosep = val.Length;

                        // spaces
                        if (thosep - i != (3 + 1))
                        {
                            err += (decsep == -1 ? "incorrect decimal separator" : "incorrect position of thousand separator");
                            isDecimal = false;
                            break;
                        }
                        // remember last thousand separator
                        thosep = i;
                    }
                    else
                    {
                        isDecimal = false;
                        break;
                    }
                }
            }

            // example ".25" or "-.5"
            if ((decsep - sign == 1) && Main.Settings.DecimalLeadingZero)
            {
                err += "missing leading zero not allowed";
                isDecimal = false;
            }

            return isDecimal;
        }

        /// <summary>
        /// validate datetime value
        /// </summary>
        /// <param name="val"> datetime value, example "31-12-2019", "12/31/2019", "2019-12-31 23:59" etc.</param>
        private bool EvaluateDateTime(string val, CsvColumn coldef, out string err)
        {
            bool isDate = false;

            err = "";

            // check if valid date using DateTime
            if (DateTime.TryParseExact(val, coldef.Mask,
                                       Main.dummyCulture,
                                       DateTimeStyles.None,
                                       out DateTime dateValue))
            {
                // valid date
                isDate = true;

                // check year range
                int year = dateValue.Year;
                if (year < Main.Settings.YearMinimum || year > Main.Settings.YearMaximum)
                {
                    isDate = false;
                    err = "is out of range";
                };
            };

            return isDate;
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();

            // output as one string
            foreach (var line in this.log)
            {
                // add line number and error/warning
                if (line.Severity == 0) sb.Append("** warning");
                if (line.Severity > 0)  sb.Append("** error");

                if (line.LineNumber > 0) sb.Append(" line " + line.LineNumber);
                if (line.Severity >= 0) sb.Append(": ");

                // add the message
                sb.Append(line.Message + "\r\n");
            }

            return sb.ToString();
        }
    }
}
