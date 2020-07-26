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
using System.Threading.Tasks;

namespace CSVLint
{
    class logline
    {
        public string Message;
        public int LineNumber;
        public int Severity; // -1 is info message, 0=warning, 1=error
        public logline(string msg)
        {
            this.Message = msg;
            this.LineNumber = -1;
            this.Severity = -1;
        }
        public logline(string msg, int linenr, int sev)
        {
            this.Message = msg;
            this.LineNumber = linenr;
            this.Severity = sev;
        }
    }

    class CsvValidate
    {
        private List<logline> log;

        public CsvValidate()
        {
            this.log = new List<logline>();
        }

        /// <summary>
        ///     validate csv data against the definition of a CsvDefinition 
        /// </summary>
        /// <param name="data"> csv data </param>
        public void ValidateData(StreamReader data, CsvDefinition csvdef)
        {
            // Exception: nothing to validate
            if ((csvdef.Fields.Count == 1) && (csvdef.Fields[0].DataType == ColumnType.String) && (csvdef.Fields[0].MaxWidth >= 9999))
            {
                // warning message
                string msg = string.Format("Nothing to inspect, not tabular data ({0}).", csvdef.Fields[0].Name);
                this.log.Add(new logline(msg, -1, -1));
                return;
            }

            // start line reader
            string line = "";
            int lineCount = 0;
            int counterr = 0;

            // if fixed length what is max line length
            int fixedlength = 0;
            if (csvdef.Separator == '\0')
            {
                foreach (CsvColumn fld in csvdef.Fields)
                {
                    fixedlength += fld.MaxWidth;
                }
            }

            // read all lines
            while (line != null)
            {
                line = data.ReadLine();
                if (line != null)
                {
                    // next line
                    lineCount++;

                    string err = "";

                    // get values from line
                    List<String> values = new List<String>();
                    if (csvdef.Separator == '\0')
                    {
                        // fixed width columns
                        int pos1 = 0;
                        for (int i = 0; i < csvdef.Fields.Count(); i++)
                        {
                            // next column width, last column gets the rest
                            int pos2 = csvdef.Fields[i].MaxWidth;

                            // if line is too short, columns missing?
                            if (pos1 >= line.Length) break;

                            // unexcepted line end or last column, then 'eat up' anything at the end of the line
                            if ((pos1 + pos2 > line.Length) || (i == csvdef.Fields.Count() - 1))
                            {
                                pos2 = line.Length - pos1;
                            }

                            // get column value
                            string val = line.Substring(pos1, pos2);
                            values.Add(val);
                            pos1 = pos1 + pos2;
                        }

                        // too many or too few characters in line
                        if (fixedlength != line.Length)
                        {
                            int dif = line.Length - fixedlength;
                            err = string.Format("Line {0} character(s) too {1}, ", (dif > 0 ? dif : -1 * dif), (dif > 0 ? "long" : "short"));
                            counterr++;
                        }

                    }
                    else
                    {
                        // delimited columns
                        values = line.Split(csvdef.Separator).ToList();
                    }

                    // too many or too few columns
                    if (values.Count != csvdef.Fields.Count)
                    {
                        err = err + string.Format("Too {0} columns, ", (values.Count > csvdef.Fields.Count ? "many" : "few"));
                        counterr++;
                    }

                    // too many or too few columns
                    for (var i = 0; i < values.Count; i++)
                    {
                        // next value and column number
                        String val = values[i];
                        val = val.Trim();

                        // null values
                        if (val == Main.Settings.NullValue)
                        {
                            val = "";
                        }

                        if (val != "")
                        {
                            // adjust for quoted values
                            if (val[0] == '"')
                            {
                                val = val.Trim('"');
                            }

                            // within bounds of column definition and non-empty value
                            if (i < csvdef.Fields.Count)
                            {
                                // column header or actual data value
                                if ((lineCount == 1) && (csvdef.ColNameHeader))
                                {
                                    // column header
                                    if (val != csvdef.Fields[i].Name)
                                    {
                                        err = err + string.Format("unexpected column name \"{0}\", ", val);
                                        counterr++;
                                    }
                                }
                                else
                                {
                                    // data values
                                    string evalerr = this.EvaluateDataValue(val, csvdef.Fields[i], i);
                                    if (evalerr != "")
                                    {
                                        err = err + evalerr;
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
                        this.log.Add(new logline(err, lineCount, 1));
                    }
                }
            }

            // final ready message
            line = string.Format("Inspected {0} lines, {1} data errors found.", lineCount, (this.log.Count == 0 ? "no" : "" + counterr));
            this.log.Add(new logline(line, -1, -1));
        }

        /// <summary>
        ///     validate csv data against the definition of a CsvDefinition 
        /// </summary>
        /// <param name="data"> csv data </param>
        private string EvaluateDataValue(string val, CsvColumn coldef, int idx)
        {

            // error result
            string err = "";
            int colnr = idx + 1;

            // ignore null values
            if (val == Main.Settings.NullValue) val = "";
            
            // check if value is too long
            if (val.Length > coldef.MaxWidth)
            {
                err = err + string.Format("Column {0} value \"{1}\" is too long, ", colnr, val);
            }
            else {
                // validation based on datetype
                bool valid = true;
                string typ = "";
                string msg = "";
                switch (coldef.DataType)
                {
                    case ColumnType.Integer:
                        typ = "integer";
                        valid = EvaluateInteger(val, coldef);
                        break;
                    case ColumnType.Decimal:
                        typ = "decimal";
                        valid = EvaluateDecimal(val, coldef, out msg);
                        break;
                    case ColumnType.DateTime:
                        typ = "datetime";
                        valid = EvaluateDateTime(val, coldef);
                        break;
                };

                // report if value is invalid
                if (!valid)
                {
                    if (msg == "")
                    {
                        err = err + string.Format("Column {0} value \"{1}\" not a valid {2} value, ", colnr, val, typ);
                    }
                    else
                    {
                        err = err + string.Format("Column {0} value \"{1}\" {2}, ", colnr, val, msg);
                    };
                }
            }

            // result
            return err;
        }

        /// <summary>
        ///     validate integer value
        /// </summary>
        /// <param name="val"> integer value, examples "1", "23", "-456" etc.</param>
        private bool EvaluateInteger(string val, CsvColumn coldef)
        {
            bool isNumeric = int.TryParse(val, out int n);
            return isNumeric;
        }

        /// <summary>
        ///     validate decimal value
        /// </summary>
        /// <param name="val"> decimal value, example "1.23", "-4,56", ".5" etc.</param>
        private bool EvaluateDecimal(string val, CsvColumn coldef, out string err)
        {
            // cannot be converted to decimal
            bool isFloat = float.TryParse(val, out float n);

            err = "";

            // incorrect decimal character
            int thopos = val.IndexOf(coldef.sTag[0]);
            int decpos = val.IndexOf(coldef.sTag[1]);
            if (thopos > decpos)
            {
                isFloat = false;
                err = "has incorrect decimal character";
            }

            // too many decimals
            if ( (decpos != -1) && (val.Length - decpos - 1 > coldef.iTag) )
            {
                isFloat = false;
                if (err != "") err = err + " and ";
                err = err + "has too many decimals";
            }

            return isFloat;
        }

        /// <summary>
        ///     validate datetime value
        /// </summary>
        /// <param name="val"> datetime value, example "31-12-2019", "12/31/2019", "2019-12-31 23:59" etc.</param>
        private bool EvaluateDateTime(string val, CsvColumn coldef)
        {
            bool isDate = false;
            DateTime dateValue;

            // check if valid date using DateTime
            if (DateTime.TryParseExact(val, coldef.Mask,
                                       new CultureInfo("en-US"),
                                       DateTimeStyles.None,
                                       out dateValue))
            {
                // check year range
                int year = dateValue.Year;
                isDate = (year >= Main.Settings.YearMinimum && year <= Main.Settings.YearMaximum);
            };

            return isDate;
        }

        public string report()
        {
            string str = "";

            // output as one string
            foreach (var line in this.log)
            {
                // add line number and error/warning
                string msg = (line.LineNumber > 0 ? "line " + line.LineNumber : "");
                msg = (line.Severity >= 0 ? (line.Severity == 0 ? "** warning " : "** error ") + msg + ": " : "");

                // add the message
                msg = msg + line.Message;
                str = str + msg + "\r\n";
            }

            return str;
        }
    }
}
