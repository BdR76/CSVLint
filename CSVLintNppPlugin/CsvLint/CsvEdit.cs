// -------------------------------------
// CsvEdit
// make edits to csv data
// based input and CsvDefinition
// makes changes to both the data and the CsvDefinition
// -------------------------------------
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET;
using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVLint
{
    class CsvEdit
    {

        /// <summary>
        /// variable safe name, no spaces
        /// </summary>
        public static string StringToVariable(string strinput)
        {
            return strinput.Replace(" ", "_");
        }

        /// <summary>
        /// apply quotes to value
        /// </summary>
        public static String ApplyQuotesToString(String strinput, int ApplyCode, char Separator, ColumnType DataType)
        {
            // default = none / minimal
            bool apl = (strinput.IndexOf(Separator) >= 0);

            if ((ApplyCode > 0) && (apl == false))
            {
                apl = (   (ApplyCode == 1 && strinput.IndexOf(" ") >= 0)    // space
                       || (ApplyCode == 2 && DataType == ColumnType.String) // string
                       || (ApplyCode == 3 && DataType != ColumnType.Integer && DataType != ColumnType.Decimal) // non-numeric
                       || (ApplyCode == 4) // all
                       );
            }

            // apply quotes
            if (apl) strinput = String.Format("\"{0}\"", strinput);

            return strinput;
        }

        /// <summary>
        /// reformat file for date, decimal and separator
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void ReformatDataFile(CsvDefinition csvdef, string reformatDatTime, string reformatDecimal, string reformatSeparator, bool updateSeparator, int ApplyQuotes, bool trimAll, bool align)
        {
            // TODO: nullable parameters

            // align vertically widths
            List<int> alignwidths = new List<int>();

            ColumnType tmpColumnType = ColumnType.String;

            if (align)
            {
                // add header column names
                for (int c = 0; c < csvdef.Fields.Count; c++)
                {
                    // get maximum of column name length and column width
                    var algwid = csvdef.Fields[c].MaxWidth;

                    if (algwid < csvdef.Fields[c].Name.Length) algwid = csvdef.Fields[c].Name.Length;

                    alignwidths.Add(algwid);
                }
            }

            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            //var s = new StringReader(data);
            int linenr = 0;

            StringBuilder datanew = new StringBuilder();

            char newSep = (updateSeparator ? reformatSeparator[0] : csvdef.Separator);

            // convert to fixed width, skip header line from source data because there is no room for column names in Fixed Width due to columns width can be 1 or 2 characters
            bool skipheader = ((updateSeparator) && (newSep == '\0') && (csvdef.ColNameHeader));

            // convert from fixed width to separated values, add header line
            if ((updateSeparator) && (newSep != '\0') && (!csvdef.ColNameHeader))
            {
                // add header column names
                for (int c = 0; c < csvdef.Fields.Count; c++)
                {
                    datanew.Append(csvdef.Fields[c].Name + (c < csvdef.Fields.Count - 1 ? newSep.ToString() : ""));
                }
                datanew.Append("\n");
            }


            // read all lines
            while (!strdata.EndOfStream)
            {
                // get values from line
                List<String> values = csvdef.ParseNextLine(strdata);

                linenr++;

                // skip header line in source data, no header line in Fixed Width output data
                if ((linenr == 1) && (skipheader)) continue;

                // reformat data line to new line
                for (int c = 0; c < values.Count; c++)
                {
                    // next value
                    String val = values[c];

                    // fixed width align
                    int wid = val.Length;
                    bool alignleft = true;

                    // trim all values
                    if (trimAll) val = val.Trim();

                    // check if data contains more columns than expected in csvdef
                    if (c < csvdef.Fields.Count)
                    {
                        // datetime reformat
                        if ((csvdef.Fields[c].DataType == ColumnType.DateTime) && (reformatDatTime != ""))
                        {
                            // convert
                            try
                            {
                                val = DateTime.ParseExact(val, csvdef.Fields[c].Mask, Main.dummyCulture).ToString(reformatDatTime, Main.dummyCulture);
                            }
                            catch
                            {
                            }
                        };

                        // decimals reformat
                        if ((csvdef.Fields[c].DataType == ColumnType.Decimal) && (reformatDecimal != ""))
                        {
                            val = val.Replace(csvdef.DecimalSymbol, reformatDecimal[0]);
                        };


                        // remember datatype for quotes
                        tmpColumnType = csvdef.Fields[c].DataType;

                        // align vertically OR fixed width, text align left - numeric align right
                        wid = (align ? alignwidths[c] : csvdef.Fields[c].MaxWidth);
                        alignleft = !((tmpColumnType == ColumnType.Integer) || (tmpColumnType == ColumnType.Decimal));

                        // exception for header -> always left align
                        if ((linenr == 1) && csvdef.ColNameHeader) alignleft = true;
                    }

                    // construct new output data line
                    if (newSep == '\0')
                    {
                        // fixed width
                        if (alignleft)
                            datanew.Append(val.PadRight(wid, ' '));
                        else
                            datanew.Append(val.PadLeft(wid, ' '));
                    }
                    else
                    {
                        // if value contains separator character then put value in quotes
                        val = ApplyQuotesToString(val, ApplyQuotes, newSep, tmpColumnType);
                        //if (val.IndexOf(newSep) >= 0) val = string.Format("\"{0}\"", val);

                        // separator
                        if (c > 0) datanew.Append(newSep.ToString());

                        // vertically align
                        if (align)
                        {
                            // align value to left (so pad right) or vice versa
                            if (alignleft)
                                datanew.Append(val.PadRight(wid, ' '));
                            else
                                datanew.Append(val.PadLeft(wid, ' '));
                        }
                        else
                        {
                            // character separated
                            datanew.Append(val);
                        }
                    }
                };

                // add line break
                datanew.Append("\n");
            };

            strdata.Dispose();

            // update text in editor
            scintillaGateway.SetText(datanew.ToString());
        }


        /// <summary>
        /// comment lines for scripts
        /// </summary>
        private static List<String> ScriptInfo(INotepadPPGateway notepad)
        {
            List<String> list = new List<String>();

            string VERSION_NO = Main.GetVersion();
            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());

            list.Add(string.Format("CSV Lint plug-in: v{0}", VERSION_NO));
            list.Add(string.Format("File: {0}", FILE_NAME));
            list.Add(string.Format("Date: {0}", DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));

            return list;
        }

        /// <summary>
        /// convert to SQL insert script
        /// </summary>
        public static void ConvertToSQL(CsvDefinition csvdef)
        {
            StringBuilder sb = new StringBuilder();
            string VERSION_NO = Main.GetVersion();
            int MAX_SQL_ROWS = Main.Settings.SQLBatchRows;

            // if csv already contains "_record_number"
            var recidname = csvdef.GetUniqueColumnName("_record_number", out int postfix);
            if (postfix > 0) recidname = String.Format("{0} ({1})", recidname, postfix);

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            string TABLE_NAME = StringToVariable(Path.GetFileNameWithoutExtension(notepad.GetCurrentFilePath()));

            sb.Append("-- -------------------------------------\r\n");
            sb.Append(string.Format("-- CSV Lint plug-in v{0}\r\n", VERSION_NO));
            sb.Append(string.Format("-- File: {0}\r\n", FILE_NAME));
            sb.Append(string.Format("-- Date: {0}\r\n", DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
            sb.Append(string.Format("-- SQL ANSI: {0}\r\n", (Main.Settings.SQLansi ? "mySQL" : "MS-SQL")));
            sb.Append("-- -------------------------------------\r\n");
            sb.Append(string.Format("CREATE TABLE {0}(\r\n\t", TABLE_NAME));

            if (Main.Settings.SQLansi)
                sb.Append(string.Format("`{0}` int AUTO_INCREMENT NOT NULL,\r\n\t", recidname)); // mySQL
            else
                sb.Append(string.Format("[{0}] int IDENTITY(1,1) PRIMARY KEY,\r\n\t", recidname)); // MS-SQL
            var cols = "\t";

            for (var r = 0; r < csvdef.Fields.Count; r++)
            {
                // determine sql column name -> mySQL = `colname`, MS-SQL = [colname]
                string sqlname = string.Format((Main.Settings.SQLansi ? "`{0}`" : "[{0}]"), csvdef.Fields[r].Name);

                // determine sql datatype
                var sqltype = "varchar";
                if (csvdef.Fields[r].DataType == ColumnType.Integer) sqltype = "integer";
                if (csvdef.Fields[r].DataType == ColumnType.DateTime) sqltype = "datetime";
                //if (csvdef.Fields[r].DataType == ColumnType.Guid) sqltype = "varchar(36)";
                if (csvdef.Fields[r].DataType == ColumnType.Decimal)
                {
                    sqltype = string.Format("numeric({0},{1})", csvdef.Fields[r].MaxWidth, csvdef.Fields[r].Decimals);
                }
                // for SQL date format always needs to be ISO format
                if (csvdef.Fields[r].DataType == ColumnType.String)
                {
                    sqltype = string.Format("varchar({0})", csvdef.Fields[r].MaxWidth);
                }
                // for SQL date format always needs to be ISO format
                //if (csvdef.Fields[r].DataType == ColumnType.DateTime)
                //{
                //    string masknew = "";
                //    if (csvdef.Fields[r].Mask.IndexOf("yy") >= 0) masknew += "yyyy-MM-dd";
                //    if (csvdef.Fields[r].Mask.IndexOf("H") >= 0) masknew += " HH:mm";
                //    if (csvdef.Fields[r].Mask.IndexOf("s") >= 0) masknew += ":ss";
                //    if (csvdef.Fields[r].Mask.IndexOf("f") >= 0) masknew += ".fff";
                //
                //    csvdef.Fields[r].Mask = masknew.Trim();
                //}

                sb.Append(String.Format("{0} {1}", sqlname, sqltype));
                cols += sqlname;
                if (r < csvdef.Fields.Count - 1)
                {
                    sb.Append(",\r\n\t");
                    cols += ",\r\n\t";
                };
            };

            // primary key definition for mySQL
            if (Main.Settings.SQLansi) sb.Append(string.Format(",\r\n\tprimary key(`{0}`)", recidname));

            sb.Append("\r\n)");

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            int lineCount = (csvdef.ColNameHeader ? -1 : 0);
            int batchcomm = -1;  // batch comment line
            int batchstart = -1; // batch starting line

            while (!strdata.EndOfStream)
            {
                // add in batches of maximal <MAX_SQL_ROWS> rows
                if (lineCount % MAX_SQL_ROWS == 0)
                {
                    // batch comment, insert record count
                    // note: not possible to insert now because the ammount of records is unknown at this point
                    if (batchcomm > -1) sb.Insert(batchcomm, string.Format("{0} - {1}", batchstart, lineCount));

                    // remember next batch
                    batchstart = lineCount + 1;

                    sb.Append(";\r\n\r\n");
                    sb.Append("-- -------------------------------------\r\n");
                    sb.Append("-- insert records \r\n");
                    batchcomm = sb.Length - 2; // -2 because of the 2 characters \r\n
                    sb.Append("-- -------------------------------------\r\n");
                    sb.Append(string.Format("insert into {0}(\r\n", TABLE_NAME));
                    sb.Append(cols);
                    sb.Append("\r\n) values");
                }

                // get next 'record' from csv data
                List<string> list = csvdef.ParseNextLine(strdata);

                // skip header line
                if (lineCount >= 0)
                {
                    // add comma, except on last row (of this batch)
                    if (lineCount % MAX_SQL_ROWS != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append("\r\n(");

                    for (var r = 0; r < list.Count; r++)
                    {
                        // format next value, quotes for varchar and datetime
                        var str = list[r];

                        // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
                        var strtrim = str.Trim();
                        if ((strtrim.Length > 0) && (strtrim[0] == Main.Settings.DefaultQuoteChar))
                        {
                            str = str.Trim();
                            str = str.Trim(Main.Settings.DefaultQuoteChar);
                        }

                        // next value to evaluate
                        if (Main.Settings.TrimValues) str = str.Trim();

                        if (str == "")
                        {
                            str = "NULL";
                        }
                        else if (csvdef.Fields[r].DataType == ColumnType.Decimal)
                        {
                            str = str.Replace((csvdef.Fields[r].DecimalSymbol == '.' ? "," : "."), ""); // remove thousand separator
                            str = str.Replace(csvdef.Fields[r].DecimalSymbol, '.');
                        }
                        else if ((csvdef.Fields[r].DataType == ColumnType.String)
                                || (csvdef.Fields[r].DataType == ColumnType.DateTime))
                        //|| (csvdef.Fields[r].DataType == ColumnType.Guid))
                        {
                            // sql datetime format
                            if (csvdef.Fields[r].DataType == ColumnType.DateTime)
                            {
                                try
                                {
                                    var dt = DateTime.ParseExact(str, csvdef.Fields[r].Mask, Main.dummyCulture);
                                    str = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    // do nothing, just keep old value if error in date value
                                    //str = ??
                                }
                            }
                            // sql single quotes
                            str = str.Replace("'", "''");
                            str = string.Format("'{0}'", str);
                        }

                        // add data value, preceded by a comma
                        sb.Append((r > 0 ? ", " : "") + str);
                    }

                    sb.Append(")");
                }

                // next line
                lineCount++;
            }

            // batch comment, insert record count
            // note: not possible to insert now because the ammount of records is unknown at this point
            if (batchcomm > -1) sb.Insert(batchcomm, string.Format("{0} - {1}", batchstart, lineCount));

            // finalise script
            sb.Append(";\r\n\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
        }

        /// <summary>
        /// convert to XML data
        /// </summary>
        public static void ConvertToXML(CsvDefinition csvdef)
        {
            StringBuilder sb = new StringBuilder();
            int MAX_SQL_ROWS = Main.Settings.SQLBatchRows;

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // default comment
            List<String> comment = ScriptInfo(notepad);

            // build XML
            sb.Append("<xml>\r\n");
            sb.Append("\t<!--\r\n");
            foreach (var str in comment)
            {
                sb.Append(string.Format("\t\t{0}\r\n", str));
            }
            sb.Append("\t-->\r\n");

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            int lineCount = (csvdef.ColNameHeader ? -1 : 0);

            while (!strdata.EndOfStream)
            {
                // get next 'record' from csv data
                List<string> list = csvdef.ParseNextLine(strdata);

                // skip header line
                if (lineCount >= 0)
                {
                    // next record
                    sb.Append("\t<record>\r\n");

                    for (var col = 0; col < csvdef.Fields.Count; col++)
                    {
                        // format next value, quotes for varchar and datetime
                        var colvalue = "";
                        if (col <= list.Count) colvalue = list[col];

                        var colname = csvdef.Fields[col].Name;

                        // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
                        var strtrim = colvalue.Trim();
                        if ((strtrim.Length > 0) && (strtrim[0] == Main.Settings.DefaultQuoteChar))
                        {
                            colvalue = colvalue.Trim();
                            colvalue = colvalue.Trim(Main.Settings.DefaultQuoteChar);
                        }

                        // next value to evaluate
                        if (Main.Settings.TrimValues) colvalue = colvalue.Trim();

                        if (csvdef.Fields[col].DataType == ColumnType.Decimal)
                        {
                            colvalue = colvalue.Replace((csvdef.Fields[col].DecimalSymbol == '.' ? "," : "."), ""); // remove thousand separator
                            colvalue = colvalue.Replace(csvdef.Fields[col].DecimalSymbol, '.');
                        }
                        else if ((csvdef.Fields[col].DataType == ColumnType.String)
                                || (csvdef.Fields[col].DataType == ColumnType.DateTime))
                        //|| (csvdef.Fields[col].DataType == ColumnType.Guid))
                        {
                            // sql datetime format
                            if (csvdef.Fields[col].DataType == ColumnType.DateTime)
                            {
                                try
                                {
                                    var dt = DateTime.ParseExact(colvalue, csvdef.Fields[col].Mask, Main.dummyCulture);
                                    colvalue = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    // do nothing, just keep old value if error in date value
                                    //str = ??
                                }
                            }
                            // sql single quotes
                            colvalue = colvalue.Replace("'", "''");
                            colvalue = string.Format("'{0}'", colvalue);
                        }

                        if (colvalue == "")
                        {
                            sb.Append(string.Format("\t\t<{0}/>\r\n", colname));
                        }
                        else
                        {
                            sb.Append(string.Format("\t\t<{0}>{1}</{0}>\r\n", colname, colvalue));
                        }
                    }

                    sb.Append("\t</record>\r\n");
                }

                // next line
                lineCount++;
            }

            // finalise script
            sb.Append("</xml>\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
        }

        /// <summary>
        /// convert to JSON data
        /// </summary>
        public static void ConvertToJSON(CsvDefinition csvdef)
        {
            StringBuilder sb = new StringBuilder();
            int MAX_SQL_ROWS = Main.Settings.SQLBatchRows;

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // build JSON
            sb.Append("{\r\n");
            // JSON doesn't support comments, add as values
            List<String> comment = ScriptInfo(notepad);
            foreach (var str in comment)
            {
                sb.Append(string.Format("\t\"{0}\",\r\n", str.Replace(": ", "\": \"")));
            }

            sb.Append("\t\"JSONdata\":[");

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            int lineCount = (csvdef.ColNameHeader ? -1 : 0);

            while (!strdata.EndOfStream)
            {
                // get next 'record' from csv data
                List<string> list = csvdef.ParseNextLine(strdata);

                // skip header line
                if (lineCount >= 0)
                {

                    // close previous line with comma (not the last records)
                    if (lineCount > 0) sb.Append(",");

                    // next record
                    sb.Append("\r\n\t\t{\r\n");

                    for (var col = 0; col < csvdef.Fields.Count; col++)
                    {
                        // format next value, quotes for varchar and datetime
                        var colvalue = "";
                        if (col <= list.Count) colvalue = list[col];

                        var colname = csvdef.Fields[col].Name;

                        // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
                        var strtrim = colvalue.Trim();
                        if ((strtrim.Length > 0) && (strtrim[0] == Main.Settings.DefaultQuoteChar))
                        {
                            colvalue = colvalue.Trim();
                            colvalue = colvalue.Trim(Main.Settings.DefaultQuoteChar);
                        }

                        // next value to evaluate
                        if (Main.Settings.TrimValues) colvalue = colvalue.Trim();

                        if (csvdef.Fields[col].DataType == ColumnType.Decimal)
                        {
                            colvalue = colvalue.Replace((csvdef.Fields[col].DecimalSymbol == '.' ? "," : "."), ""); // remove thousand separator
                            colvalue = colvalue.Replace(csvdef.Fields[col].DecimalSymbol, '.');
                        }
                        else if ((csvdef.Fields[col].DataType == ColumnType.String)
                                || (csvdef.Fields[col].DataType == ColumnType.DateTime))
                        //|| (csvdef.Fields[col].DataType == ColumnType.Guid))
                        {
                            // sql datetime format
                            if (csvdef.Fields[col].DataType == ColumnType.DateTime)
                            {
                                try
                                {
                                    var dt = DateTime.ParseExact(colvalue, csvdef.Fields[col].Mask, Main.dummyCulture);
                                    colvalue = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    // do nothing, just keep old value if error in date value
                                    //str = ??
                                }
                            }
                            // sql single quotes
                            colvalue = colvalue.Replace("'", "''");
                            colvalue = string.Format("'{0}'", colvalue);
                        }

                        var comma = (col < csvdef.Fields.Count - 1 ? "," : "");

                        if (colvalue != "")
                        {
                            sb.Append(string.Format("\t\t\t\"{0}\": \"{1}\"{2}\r\n", colname, colvalue, comma));
                        }
                    }

                    sb.Append("\t\t}");
                }

                // next line
                lineCount++;
            }

            // finalise script
            sb.Append("\r\n\t]\r\n}\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
        }

        /// <summary>
        /// split column into new columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void ColumnSplit(CsvDefinition csvdef, int ColumnIndex, int SplitCode, string Parameter1, string Parameter2, bool bRemove)
        {
            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            //var s = new StringReader(data);
            int linenr = 0;
            int.TryParse(Parameter1, out int IntPar);
            var IntPar2 = -1 * IntPar;

            // decode
            List<String> decode1 = new List<String>();
            List<String> decode2 = new List<String>();

            // decode
            if (SplitCode == 5)
            {
                decode1 = Parameter1.Split(Parameter2[0]).Select(item => item.Trim()).ToList();
            }

            StringBuilder datanew = new StringBuilder();

            var sep = csvdef.Separator.ToString();

            var csvvalid = new CsvValidate();

            // convert from fixed width to separated values, add header line
            if (csvdef.ColNameHeader)
            {
                // if first line is header column names, then consume line and ignore
                csvdef.ParseNextLine(strdata);

                // add header column names
                for (int c = 0; c < csvdef.Fields.Count; c++)
                {

                    // add column header to output, except when remove original column
                    if ((c != ColumnIndex) || (bRemove == false))
                    {
                        datanew.Append((c > 0 ? sep : "") + csvdef.Fields[c].Name);
                    }

                    // add new split columns headers
                    if (c == ColumnIndex)
                    {
                        // determine new column header names, check for existing postfix
                        var newname = csvdef.GetUniqueColumnName(csvdef.Fields[c].Name, out int postfix);

                        // when decoding csv values (SplitCode == 5) add more new columns, when normal split then just 2
                        var addmax = (SplitCode == 5 ? decode1.Count + 1 : 2); // +1 = one extra column of any left-over values
                        for (var cnew = 0; cnew < addmax; cnew++)
                        {
                            datanew.Append(String.Format("{0}{1} ({2})", sep, newname, postfix + cnew));
                        }
                    }
                }
                datanew.Append("\n");
            }

            // read all lines
            while (!strdata.EndOfStream)
            {
                // get values from line
                List<string> values = csvdef.ParseNextLine(strdata);

                linenr++;

                // reformat data line to new line
                for (int c = 0; c < values.Count; c++)
                {
                    // next value
                    String val = values[c];

                    // if value contains separator character then put value in quotes
                    if (val.IndexOf(sep) >= 0) val = string.Format("\"{0}\"", val);

                    // add column to output, except when remove original column
                    if ((c != ColumnIndex) || (bRemove == false))
                    {
                        datanew.Append(val);
                        datanew.Append(sep);
                    }

                    // add new split columns values
                    if (c == ColumnIndex)
                    {
                        String val0 = values[c]; // original value without quotes
                        String val1 = val0;
                        String val2 = "";

                        // how to split value
                        if (SplitCode == 1)
                        {
                            // valid/invalid
                            var str = csvvalid.EvaluateDataValue(val, csvdef.Fields[ColumnIndex], ColumnIndex);
                            if (str != "")
                            {
                                val1 = "";
                                val2 = val0; // invalid value
                            }
                        }
                        else if (SplitCode == 2)
                        {
                            // split on char
                            int pos = val0.IndexOf(Parameter1);
                            if (pos >= 0)
                            {
                                val1 = val0.Substring(0, pos);
                                val2 = val0.Substring(pos + Parameter1.Length, val0.Length - pos - Parameter1.Length);
                            }
                        }
                        else if (SplitCode == 3)
                        {
                            // split on position
                            int pos = val0.IndexOf(Parameter1);
                            if ((IntPar > 0) && (IntPar < val0.Length))
                            {
                                // positive, left string
                                val1 = val0.Substring(0, IntPar);
                                val2 = val0.Substring(IntPar, val0.Length - IntPar);
                            }
                            else if (IntPar < 0)
                            {
                                // negative, right string
                                if (IntPar2 < val0.Length)
                                {
                                    val1 = val0.Substring(0, val0.Length - IntPar2);
                                    val2 = val0.Substring(val0.Length - IntPar2);
                                }
                                else
                                {
                                    // take all as right string
                                    val1 = "";
                                    val2 = val;
                                }
                            }
                        }
                        else if (SplitCode == 4)
                        {
                            // split when contains
                            int pos = val0.IndexOf(Parameter1);
                            if (pos >= 0)
                            {
                                val1 = "";
                                val2 = val0;
                            }
                        }
                        else if (SplitCode == 5)
                        {
                            // decode multiple values, example val0 = "1;2;3"
                            decode2.Clear();
                            decode2 = val0.Split(Parameter2[0]).Select(item => item.Trim()).ToList();

                            // split value into into columns
                            val1 = "";
                            foreach (var dec in decode1)
                            {
                                // check if separated value in list of decode values
                                var decidx = decode2.IndexOf(dec);

                                // separate column
                                val1 += (decidx >= 0 ? dec : "") + sep;

                                // remove from original value
                                if (decidx >= 0) decode2.RemoveAt(decidx);
                            }
                            val1 = val1.Remove(val1.Length - 1); // remove last separator

                            // put any left-over values in the extra column
                            val2 = "";
                            foreach (var dec in decode2)
                            {
                                val2 += dec + Parameter2[0];
                            }
                            if (val2.Length > 0) val2 = val2.Remove(val2.Length - 1); // remove last separator"; "
                        }

                        // add split column values
                        datanew.Append(val1 + sep);
                        datanew.Append(val2 + sep);
                    }
                };

                // remove last separator
                datanew.Length -= 1;

                // add line break
                datanew.Append("\n");
            };

            strdata.Dispose();

            // update text in editor
            scintillaGateway.SetText(datanew.ToString());

            // add columns to csv definition
            //csvdef.AddColumn() ??
        }
    }
}
