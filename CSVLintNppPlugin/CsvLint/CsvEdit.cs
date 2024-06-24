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
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
        public static string ApplyQuotesToString(string strinput, char separator, ColumnType dataType)
        {
            var applyCode = Main.Settings.ReformatQuotes;

            // default = none / minimal
            bool apl = (strinput.Contains(separator) || strinput.Contains(Main.Settings.DefaultQuoteChar) || strinput.Contains('\r') || strinput.Contains('\n'));

            if (!apl && (applyCode > 0))
            {
                apl = (applyCode == 1 && strinput.IndexOf(" ") >= 0)    // space
                   || (applyCode == 2 && dataType == ColumnType.String) // string
                   || (applyCode == 3 && dataType != ColumnType.Integer && dataType != ColumnType.Decimal) // non-numeric
                   || (applyCode == 4) // all
                   ;
            }

            // apply quotes
            if (apl)
            {
                // escape any quote characters, example "Value "test" 123" -> "Value ""test"" 123"
                if (strinput.Contains(Main.Settings.DefaultQuoteChar))
                {
                    var quote2x = new string(Main.Settings.DefaultQuoteChar, 2);
                    strinput = strinput.Replace(Main.Settings.DefaultQuoteChar.ToString(), quote2x);
                }
                // add quotes
                strinput = string.Format("{0}{1}{0}", Main.Settings.DefaultQuoteChar, strinput);
            }

            return strinput;
        }

        /// <summary>
        /// remove quotes from value, only used for fixed width
        /// </summary>
        public static string RemoveQuotesToString(string strinput)
        {
            var res = strinput;

            // only if at least 2 characters example 28"wheel, "a", "test" etc.
            if (res.Length > 1)
            {
                // only if starts and ends with quote character
                if ( (res[0] == Main.Settings.DefaultQuoteChar) && (res[res.Length-1] == Main.Settings.DefaultQuoteChar) )
                {
                    // remove start and end
                    res = res.Trim(Main.Settings.DefaultQuoteChar);

                    // un-escape any quote characters, example res = 28""wheel -> res = 28"wheel
                    if (res.Contains(Main.Settings.DefaultQuoteChar))
                    {
                        var quote2x = new string(Main.Settings.DefaultQuoteChar, 2);
                        res = res.Replace(quote2x, Main.Settings.DefaultQuoteChar.ToString());
                    }
                }
            }

            return res;
        }

        private static string getEditorEOLchars(EndOfLine EOL)
        {
            switch (EOL)
            {
                case EndOfLine.CR: return "\r";
                case EndOfLine.LF: return "\n";
                default: return "\r\n"; // default is EndOfLine.CRLF
            }
        }

        /// <summary>
        /// reformat file for date, decimal and separator
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void ReformatDataFile(CsvDefinition csvdef, string reformatSeparator, bool updateSeparator, string reformatDatTime, string reformatDecimal, string ReplaceCrLf, bool align)
        {
            // TODO: nullable parameters

            // align vertically widths
            List<int> alignwidths = new List<int>();

            ColumnType tmpColumnType = ColumnType.String;

            if (align)
            {
                var applyCode = Main.Settings.ReformatQuotes;
                // add header column names
                for (int c = 0; c < csvdef.Fields.Count; c++)
                {
                    // get maximum of column name length and column width
                    var algwid = csvdef.Fields[c].MaxWidth;

                    if (algwid < csvdef.Fields[c].Name.Length) algwid = csvdef.Fields[c].Name.Length;

                    // add 2 extra character to width, to account for the added quotes
                    //if (applyCode == 1 && strinput.IndexOf(" ") >= 0)    // space, NOTE: cannot be sure wether or not column contains spaces
                    if ( (applyCode == 2 && csvdef.Fields[c].DataType == ColumnType.String) // string
                      || (applyCode == 3 && csvdef.Fields[c].DataType != ColumnType.Integer && csvdef.Fields[c].DataType != ColumnType.Decimal) // non-numeric
                      || (applyCode == 4) // all
                        )
                        algwid += 2;

                    alignwidths.Add(algwid);
                }
            }

            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;
            var CRLF = getEditorEOLchars(scintillaGateway.GetEOLMode());

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            //var s = new StringReader(data);
            int linenr = 0;

            StringBuilder datanew = new StringBuilder();

            char newSep = updateSeparator ? reformatSeparator[0] : csvdef.Separator;

            // convert to fixed width, skip header line from source data because there is no room for column names in Fixed Width due to columns width can be 1 or 2 characters
            bool skipheader = updateSeparator && (newSep == '\0') && csvdef.ColNameHeader;

            // convert from fixed width to separated values, add header line
            if (updateSeparator && (newSep != '\0') && (!csvdef.ColNameHeader))
            {
                // add header column names
                for (int c = 0; c < csvdef.Fields.Count; c++)
                {
                    datanew.Append(csvdef.Fields[c].Name + (c < csvdef.Fields.Count - 1 ? newSep.ToString() : ""));
                }
                datanew.Append("\n");
            }

            // copy any comment lines
            csvdef.CopyCommentLinesAtStart(strdata, datanew, "", CRLF);

            // read all lines
            while (!strdata.EndOfStream)
            {
                // get values from line
                List<String> values = csvdef.ParseNextLine(strdata, out bool iscomm);

                if (iscomm)
                {
                    csvdef.CopyCommentLine(values, datanew, "", "");
                }
                else
                {
                    // next data line
                    linenr++;

                    // skip header line in source data, no header line in Fixed Width output data
                    if ((linenr == 1) && skipheader) continue;

                    // reformat data line to new line
                    for (int c = 0; c < values.Count; c++)
                    {
                        // next value
                        string val = values[c];

                        // fixed width align
                        int wid = val.Length;
                        bool alignleft = true;

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
                            wid = align ? alignwidths[c] : csvdef.Fields[c].MaxWidth;
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
                            // replace any carriage retursn/line feeds
                            if (ReplaceCrLf != "\r\n")
                            {
                                if ((val.IndexOf("\r") >= 0) || (val.IndexOf("\n") >= 0))
                                {
                                    val = val.Replace("\r\n", ReplaceCrLf); // windows
                                    val = val.Replace("\n", ReplaceCrLf); // linux
                                    val = val.Replace("\r", ReplaceCrLf); // old macos
                                }
                            }

                            // if value contains separator character then put value in quotes
                            val = ApplyQuotesToString(val, newSep, tmpColumnType);
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
                    datanew.Append(CRLF);
                };
            }

            strdata.Dispose();

            // update text in editor
            scintillaGateway.SetText(datanew.ToString());
        }


        /// <summary>
        /// comment lines for scripts
        /// </summary>
        public static List<String> ScriptInfo(INotepadPPGateway notepad)
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
            int MAX_SQL_ROWS = Main.Settings.DataConvertBatch;

            // if csv already contains "_record_number"
            var recidname = csvdef.GetUniqueColumnName("_record_number", out int postfix);
            if (postfix > 0) recidname = SQLSafeName(string.Format("{0} ({1})", recidname, postfix));

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // user supplied table name, or use file name if empty
            string TABLE_NAME = Main.Settings.DataConvertName;
            if (TABLE_NAME == "") TABLE_NAME = StringToVariable(Path.GetFileNameWithoutExtension(notepad.GetCurrentFilePath()));

            // apply brackets or quotes, only if needed
            TABLE_NAME = SQLSafeName(TABLE_NAME);

            string SQL_TYPE = (Main.Settings.DataConvertSQL <= 1 ? (Main.Settings.DataConvertSQL == 0 ? "mySQL" : "MS-SQL") : "PostgreSQL");

            // default comment
            List<String> comment = ScriptInfo(notepad);

            // build SQL
            sb.Append("-- -------------------------------------\r\n");
            foreach (var str in comment)
            {
                sb.Append(string.Format("-- {0}\r\n", str));
            }
            sb.Append(string.Format("-- SQL type: {0}\r\n", SQL_TYPE));
            sb.Append("-- -------------------------------------\r\n");

            sb.Append(string.Format("CREATE TABLE {0} (\r\n\t", TABLE_NAME));

            switch (Main.Settings.DataConvertSQL)
            {
                case 1: // MS-SQL
                    sb.Append(string.Format("{0} int IDENTITY(1,1) PRIMARY KEY,\r\n\t", recidname));
                    break;
                case 2: // PostgreSQL
                    sb.Append(string.Format("{0} SERIAL PRIMARY KEY,\r\n\t", recidname));
                    break;
                default: // 0=mySQL
                    sb.Append(string.Format("{0} int AUTO_INCREMENT NOT NULL,\r\n\t", recidname));
                    break;
            }

            var cols = "\t";
            var enumcols1 = "";
            var enumcols2 = "";

            for (var r = 0; r < csvdef.Fields.Count; r++)
            {
                // determine sql column name -> mySQL = `colname`, MS-SQL = [colname], PostgreSQL = "colname"
                string sqlname = SQLSafeName(csvdef.Fields[r].Name);

                // determine sql datatype
                var sqltype = "varchar";
                var colcomment = "";

                if (csvdef.Fields[r].DataType == ColumnType.Integer) sqltype = "integer";
                if (csvdef.Fields[r].DataType == ColumnType.DateTime) sqltype = (Main.Settings.DataConvertSQL < 2 ? "datetime" : "timestamp"); // mySQL/MS-SQL = datetime, Postgress=timestamp
                //if (csvdef.Fields[r].DataType == ColumnType.Guid) sqltype = "varchar(36)";
                if (csvdef.Fields[r].DataType == ColumnType.Decimal)
                {
                    sqltype = string.Format("numeric({0},{1})", csvdef.Fields[r].MaxWidth, csvdef.Fields[r].Decimals);
                }
                // for SQL date format always needs to be ISO format
                if (csvdef.Fields[r].DataType == ColumnType.String)
                {
                    var wd = csvdef.Fields[r].MaxWidth;
                    if (wd == 0)
                    {
                        colcomment = (wd == 0 ? " -- width unknown" : "");
                        wd = 10;
                    }
                    sqltype = string.Format("varchar({0})", wd);
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

                // no comma after last column, except for mySQL
                var comma = (r < csvdef.Fields.Count - 1 || Main.Settings.DataConvertSQL == 0 ? "," : "");

                sb.Append(string.Format("{0} {1}{2}{3}", sqlname, sqltype, comma, colcomment));
                cols += sqlname;
                if (r < csvdef.Fields.Count - 1)
                {
                    sb.Append("\r\n\t");
                    cols += ",\r\n\t";
                };

                // build SQL for Enum columns
                if (csvdef.Fields[r].isCodedValue)
                {
                    var enumvals = string.Join("\", \"", csvdef.Fields[r].CodedList).Replace("'", "''");
                    switch (Main.Settings.DataConvertSQL)
                    {
                        case 1: // MS-SQL
                        //case 2: // PostgreSQL
                        // constrains name is based on column name, for example "[test]" -> "[CHK_test]"
                            var chkname = SQLSafeName("CHK_" + csvdef.Fields[r].Name);
                            var mscolate = "";
                            // Constrains for string or integer values
                            if (csvdef.Fields[r].DataType == ColumnType.String)
                            {
                                enumvals = string.Format("'{0}'", enumvals.Replace("\"", "'")); // use quotes
                                if (Main.Settings.DataConvertSQL == 1) mscolate = " COLLATE Latin1_General_CS_AS"; // MS-SQL case-sensitive
                            }
                            else {
                                enumvals = enumvals.Replace("\"", ""); // no quotes
                            };
                            enumcols1 += string.Format("ALTER TABLE {0} ADD CONSTRAINT {1} CHECK({2}{3} IN ({4}));\r\n", TABLE_NAME, chkname, sqlname, mscolate, enumvals);
                            break;
                        // NOTE: Could also use CONSTRAINT..CHECK for both MS-SQL and PostgreSQL,
                        // even though PostgreSQL supports custom enum TYPE but it is less flexible than just CONSTRAINT..CHECK
                        case 2: // PostgreSQL
                            var postenum = SQLSafeName("enum_" + csvdef.Fields[r].Name);
                            enumcols1 += string.Format("CREATE TYPE {0} AS ENUM ('{1}');\r\n", postenum, enumvals.Replace("\"", "'"));
                            enumcols2 += string.Format("ALTER TABLE {0} ALTER COLUMN {1} TYPE {2} USING ({1}::text)::{2};\r\n", TABLE_NAME, sqlname, postenum);
                            // for PostgreSQL, insert statements on ENUM column must always use quotes, so ('0', '1', '2') instead of (0, 1, 2)
                            if (csvdef.Fields[r].DataType == ColumnType.Integer) csvdef.Fields[r].DataType = ColumnType.String;
                            break;
                        default: // 0=mySQL
                            enumvals = enumvals.Replace("\"", "'"); // ENUM on mySQL is always treated as string value
                            enumcols1 += string.Format("ALTER TABLE {0} MODIFY COLUMN {1} ENUM('{2}');\r\n", TABLE_NAME, sqlname, enumvals);
                            // for mySQL, insert statements on ENUM column must always use quotes, so ('0', '1', '2') instead of (0, 1, 2)
                            if (csvdef.Fields[r].DataType == ColumnType.Integer) csvdef.Fields[r].DataType = ColumnType.String;
                            break;
                    }
                }
            };

            // primary key definition for mySQL
            if (Main.Settings.DataConvertSQL == 0) sb.Append(string.Format("\r\n\tprimary key({0})", recidname));

            sb.Append("\r\n);\r\n");

            // add enumeration columns
            if (enumcols1 != "") sb.Append(string.Format("-- Enumeration columns (optional)\r\n/*\r\n{0}{1}*/\r\n", enumcols1, enumcols2));

            // add comment table
            comment.Insert(0, "Table imported using");
            var tabcomment = string.Join("\r\n", comment).Replace("'", "''");
            sb.Append("-- Table comment\r\n");
            switch (Main.Settings.DataConvertSQL)
            {
                case 1:
                    sb.Append(string.Format("EXEC sp_addextendedproperty 'Comment', N'{1}', N'SCHEMA', DBO, N'TABLE', {0}\r\nGO\r\n", TABLE_NAME, tabcomment)); // MS-SQL
                    //sb.Append(string.Format("EXEC sys.sp_addextendedproperty @name = N'comment', @value = N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}'\r\nGO\r\n", coment, tablemmaf)); // MS-SQL alt
                    break;
                case 2:
                    sb.Append(string.Format("COMMENT ON TABLE {0} IS '{1}';\r\n", TABLE_NAME, tabcomment)); // PostgreSQL
                    break;
                default: // 0=mySQL
                    sb.Append(string.Format("ALTER TABLE {0} COMMENT '{1}';\r\n", TABLE_NAME, tabcomment)); // mySQL
                    break;
            }

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            int lineCount = csvdef.ColNameHeader ? -1 : 0;
            int batchcomm = -1;  // batch comment line
            int batchstart = -1; // batch starting line
            int lastend = -1;  // last line end character

            // copy any comment lines
            csvdef.CopyCommentLinesAtStart(strdata, sb, "-- ", "\r\n");

            while (!strdata.EndOfStream)
            {
                // get next 'record' from csv data
                List<string> list = csvdef.ParseNextLine(strdata, out bool iscomm);

                // copy any comment lines
                if (iscomm)
                {
                    csvdef.CopyCommentLine(list, sb, "-- ", "");
                }
                else
                {
                    // add in batches of maximal <MAX_SQL_ROWS> rows
                    if (lineCount % MAX_SQL_ROWS == 0)
                    {
                        // batch comment, insert record count
                        // note: not possible to insert now because the ammount of records is unknown at this point
                        if (batchcomm > -1) sb.Insert(batchcomm, string.Format("{0} - {1}", batchstart, lineCount));

                        // remember next batch
                        batchstart = lineCount + 1;

                        sb.Append("\r\n-- -------------------------------------\r\n");
                        sb.Append("-- insert records \r\n");
                        batchcomm = sb.Length - 2; // -2 because of the 2 characters \r\n
                        sb.Append("-- -------------------------------------\r\n");
                        sb.Append(string.Format("INSERT INTO {0} (\r\n", TABLE_NAME));
                        sb.Append(cols);
                        sb.Append("\r\n) VALUES\r\n");
                    }

                    // skip header line
                    if (lineCount >= 0)
                    {
                        // next line of values
                        sb.Append("(");

                        for (var col = 0; col < csvdef.Fields.Count; col++)
                        {
                            // format next value, quotes for varchar and datetime
                            var colvalue = "";
                            if (col < list.Count) colvalue = list[col];

                            // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
                            var strtrim = colvalue.Trim();
                            if ((strtrim.Length > 0) && (strtrim[0] == Main.Settings.DefaultQuoteChar))
                            {
                                colvalue = colvalue.Trim();
                                colvalue = colvalue.Trim(Main.Settings.DefaultQuoteChar);
                            }

                            // next value to evaluate
                            if (Main.Settings.TrimValues) colvalue = colvalue.Trim();

                            if (colvalue == "")
                            {
                                colvalue = "NULL";
                            }
                            else if (csvdef.Fields[col].DataType == ColumnType.Decimal)
                            {
                                colvalue = colvalue.Replace(csvdef.Fields[col].DecimalSymbol == '.' ? "," : ".", ""); // remove thousand separator
                                colvalue = colvalue.Replace(csvdef.Fields[col].DecimalSymbol, '.');
                            }
                            else if ((csvdef.Fields[col].DataType == ColumnType.String)
                                  || (csvdef.Fields[col].DataType == ColumnType.DateTime))
                            //|| (csvdef.Fields[r].DataType == ColumnType.Guid))
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

                            // add data value, preceded by a comma
                            sb.Append((col > 0 ? ", " : "") + colvalue);
                        }

                        // end line with comma , or semicolon at end of a batch
                        sb.Append(string.Format("){0}\r\n", ((lineCount+1) % MAX_SQL_ROWS != 0 ? "," : ";")));
                        lastend = sb.Length - 3; // -3 because -1 for ,/; character and  -2 for characters \r\n
                    }

                    // next line
                    lineCount++;
                }
            }

            // finalise script, it's possible the last lines are all comment lines, then end of data line is not a semicolon ; but a comma , 
            if (lastend > -1) {
                sb.Remove(lastend, 1);   // remove last comma
                sb.Insert(lastend, ";"); // replace with semi-colon
            }

            // batch comment, insert record count
            // note: not possible to insert now because the ammount of records is unknown at this point
            if (batchcomm > -1) sb.Insert(batchcomm, string.Format("{0} - {1}", batchstart, lineCount));

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
            if (sb.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_SQL);
            }
        }

        private static string SQLSafeName(string sqlname)
        {
            var res = sqlname;

            // use brackets or quotes only when absolutely necessary
            if (res.Contains(" ") || res.Contains("'"))
            {
                if (Main.Settings.DataConvertSQL == 1) // MS-SQL
                    res = string.Format("[{0}]", res);
                else
                    res = string.Format("{1}{0}{1}", res, (Main.Settings.DataConvertSQL == 0 ? "`" : "\""));
            }

            return res;
        }
        private static string XMLSafeName(string name)
        {
            // xml safe tag name
            name = Regex.Replace(name, "[^a-zA-Z0-9]", "_"); // not letter or digit
            name = Regex.Replace(name, "[_]{2,}", "_"); // double underscore to single underscore

            return name;
        }

        /// <summary>
        /// convert to XML data
        /// </summary>
        public static void ConvertToXML(CsvDefinition csvdef)
        {
            StringBuilder sb = new StringBuilder();

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // record tag name
            string TABLE_NAME = Main.Settings.DataConvertName;
            if (TABLE_NAME == "") TABLE_NAME = StringToVariable(Path.GetFileNameWithoutExtension(notepad.GetCurrentFilePath()));
            TABLE_NAME = XMLSafeName(TABLE_NAME);

            // default comment
            List<String> comment = ScriptInfo(notepad);

            // pre-build XML-safe column names, so doesn't have to run regex again for each data record
            List<String> xmlnames = new List<String>();
            for (var col = 0; col < csvdef.Fields.Count; col++)
            {
                var colname = csvdef.Fields[col].Name;
                xmlnames.Add(XMLSafeName(colname));
            }

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

            // copy any comment lines
            if (csvdef.SkipLines > 0) {
                sb.Append("\t<!--\n");
                csvdef.CopyCommentLinesAtStart(strdata, sb, "\t", "\r\n");
                sb.Append("\t-->\n");
            }

            while (!strdata.EndOfStream)
            {
                // get next 'record' from csv data
                List<string> list = csvdef.ParseNextLine(strdata, out bool iscomm);

                // copy any comment lines
                if (iscomm)
                {
                    csvdef.CopyCommentLine(list, sb, "\t<!-- ", " -->");
                }
                else
                {
                    // skip header line
                    if (lineCount >= 0)
                    {
                        // next record
                        sb.Append(string.Format("\t<{0}>\r\n", TABLE_NAME));

                        for (var col = 0; col < csvdef.Fields.Count; col++)
                        {
                            // format next value, quotes for varchar and datetime
                            var colvalue = "";
                            if (col < list.Count) colvalue = list[col];

                            //var colname = csvdef.Fields[col].Name;
                            var colname = xmlnames[col];

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
                            else if ((csvdef.Fields[col].DataType == ColumnType.DateTime)
                                    && (colvalue != ""))
                            {
                                // sql datetime format
                                try
                                {
                                    var dt = DateTime.ParseExact(colvalue, csvdef.Fields[col].Mask, Main.dummyCulture);
                                    colvalue = dt.ToString("s"); // "s" -> format as sortable XML Schema
                                    //colvalue = dt.ToString("yyyy-MM-ddTHH\\:mm\\:ss"); // no milliseconds or timezone
                                }
                                catch
                                {
                                    // do nothing, just keep old value if error in date value
                                    //str = ??
                                }
                            }
                            else if ((csvdef.Fields[col].DataType == ColumnType.String)
                              && (colvalue != ""))
                            {
                                // XML escape characters
                                colvalue = colvalue.Replace("&", "&amp;"); // ampersnd
                                colvalue = colvalue.Replace("<", "&lt;"); // less than
                                colvalue = colvalue.Replace(">", "&gt;"); // greater than

                                colvalue = colvalue.Replace("\b", "&#09;"); // \b Backspace(ascii code 08)
                                colvalue = colvalue.Replace("\f", "&#0C;"); // \f Form feed(ascii code 0C)
                                colvalue = colvalue.Replace("\n", "&#10;"); // \n New line
                                colvalue = colvalue.Replace("\r", "&#13;"); // \r Carriage return
                                colvalue = colvalue.Replace("\t", "&#09;"); // \t Tab
                                colvalue = colvalue.Replace("\"", "&quote;"); // quote
                                colvalue = colvalue.Replace("'", "&apos;"); // single quote/apostrophe

                                colvalue = colvalue.Replace("'", "''");
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

                        sb.Append(string.Format("\t</{0}>\r\n", TABLE_NAME));
                    }

                    // next line
                    lineCount++;
                }
            }

            // finalise script
            sb.Append("</xml>\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
            if (sb.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_XML);
            }
        }

        /// <summary>
        /// convert to JSON data
        /// </summary>
        public static void ConvertToJSON(CsvDefinition csvdef)
        {
            StringBuilder sb = new StringBuilder();

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // to evaluate integer and decimals
            CsvValidate csveval = new CsvValidate();

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

            // skip any comment lines
            csvdef.SkipCommentLinesAtStart(strdata);

            while (!strdata.EndOfStream)
            {
                // get next 'record' from csv data
                List<string> list = csvdef.ParseNextLine(strdata, out bool iscomm);

                if (!iscomm)
                {
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
                            if (col < list.Count) colvalue = list[col];

                            var colname = csvdef.Fields[col].Name.Replace("\"", "\\\""); // \"  Double quote

                            // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
                            var strtrim = colvalue.Trim();
                            if ((strtrim.Length > 0) && (strtrim[0] == Main.Settings.DefaultQuoteChar))
                            {
                                colvalue = colvalue.Trim();
                                colvalue = colvalue.Trim(Main.Settings.DefaultQuoteChar);
                            }

                            // next value to evaluate
                            if (Main.Settings.TrimValues) colvalue = colvalue.Trim();

                            if (csvdef.Fields[col].DataType == ColumnType.Integer)
                            {
                                if (!csveval.EvaluateInteger(colvalue))
                                {
                                    // only put in double quotes when it's not an integer
                                    colvalue = string.Format("\"{0}\"", colvalue);
                                }
                            }
                            else if (csvdef.Fields[col].DataType == ColumnType.Decimal)
                            {
                                if (!csveval.EvaluateDecimal(colvalue, csvdef.Fields[col], out _))
                                {
                                    // only put in double quotes when it's not a valid decimal
                                    colvalue = string.Format("\"{0}\"", colvalue);
                                }
                                else
                                {
                                    colvalue = colvalue.Replace((csvdef.Fields[col].DecimalSymbol == '.' ? "," : "."), ""); // remove thousand separator
                                    colvalue = colvalue.Replace(csvdef.Fields[col].DecimalSymbol, '.');
                                }
                            }
                            else if ((csvdef.Fields[col].DataType == ColumnType.DateTime)
                                    && (colvalue != ""))
                            {
                                // json datetime format
                                try
                                {
                                    var dt = DateTime.ParseExact(colvalue, csvdef.Fields[col].Mask, Main.dummyCulture);
                                    colvalue = dt.ToString("s"); // "s" -> format as sortable XML Schema
                                    //colvalue = dt.ToString("yyyy-MM-ddTHH\\:mm\\:ss"); // no milliseconds or timezone
                                }
                                catch
                                {
                                    // do nothing, just keep old value if error in date value
                                    //str = ??
                                }
                            }
                            else if (csvdef.Fields[col].DataType == ColumnType.String)
                            {
                                // JSON escape characters
                                colvalue = colvalue.Replace("\\", "\\\\"); // \\  Backslash character
                                colvalue = colvalue.Replace("\b", "\\b"); // \b Backspace(ascii code 08)
                                colvalue = colvalue.Replace("\f", "\\f"); // \f Form feed(ascii code 0C)
                                colvalue = colvalue.Replace("\n", "\\n"); // \n New line
                                colvalue = colvalue.Replace("\r", "\\r"); // \r Carriage return
                                colvalue = colvalue.Replace("\t", "\\t"); // \t Tab
                                colvalue = colvalue.Replace("\"", "\\\""); // \"  Double quote
                            }

                            // put string and datetime values in double quotes
                            if ((csvdef.Fields[col].DataType == ColumnType.String)
                                || (csvdef.Fields[col].DataType == ColumnType.DateTime))
                            {
                                colvalue = string.Format("\"{0}\"", colvalue);
                            }

                            var comma = (col < csvdef.Fields.Count - 1 ? "," : "");

                            if (colvalue != "")
                            {
                                sb.Append(string.Format("\t\t\t\"{0}\": {1}{2}\r\n", colname, colvalue, comma));
                            }
                        }

                        sb.Append("\t\t}");
                    }

                    // next line
                    lineCount++;
                }
            }

            // finalise script
            sb.Append("\r\n\t]\r\n}\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
            if (sb.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_JSON);
            }
        }

        private static string SortableString(string val, CsvColumn csvcol)
        {
            if ( (csvcol.DataType == ColumnType.Integer) || (csvcol.DataType == ColumnType.Decimal)) // integer or decimal
            {
                // decimal, pad decimals first
                if (csvcol.DataType == ColumnType.Decimal)
                {
                    // remove any thousand separators
                    var thous = (csvcol.DecimalSymbol == '.' ? "," : ".");
                    val = val.Replace(thous, ""); // remove thousand separators

                    // determine decimal position in string value
                    var decpos = val.IndexOf(csvcol.DecimalSymbol);
                    if (decpos == -1)
                    {
                        // no decimal, add one
                        val += csvcol.DecimalSymbol;
                        decpos = val.Length - 1;
                    }
                    // right pad decimals, example csvcol.Decimals=5 val="12.34" -> val="12.34000"
                    var paddec = csvcol.Decimals - (val.Length - decpos - 1);
                    if (paddec > 0) val = val.PadRight((val.Length + paddec), '0');
                }
                // for the rest the integer and decimal are the same

                // positive of negative numbers
                if (val.IndexOf('-') < 0)
                {
                    // positive numbers
                    return "9" + val.PadLeft(csvcol.MaxWidth, '0');
                }
                else
                {
                    // negative numbers, 'invert' each digit for sorting purposes
                    var ret = "";
                    foreach (char c in val)
                    {
                        var digitinvert = c;
                        if ((c >= '0') && (c <= '9'))
                        {
                            // '0'..'9' = ascii 48..57, invert digits so '0'->'9', '1'->'8', '2'->'7' etc.
                            digitinvert = (char)(48 + 57 - c);
                        }
                        else if (c == '-')
                        {
                            digitinvert = '9';
                        }
                        ret += digitinvert;
                    }
                    return "0" + ret.PadLeft(csvcol.MaxWidth, '9');
                };
            }
            else if (csvcol.DataType == ColumnType.DateTime)
            {
                // convert datetime to iso format for sorting purposes
                try
                {
                    val = DateTime.ParseExact(val, csvcol.Mask, Main.dummyCulture).ToString("yyyyMMddHHmmssfff", Main.dummyCulture);
                }
                catch
                {
                    val = "00000000000000000"; // invalid date sort to front(?)
                }
                return val;
            }
            else
            {
                // string or any other value
                return val;
            }
        }

        /// <summary>
        /// Sort data on column ascending or descending
        /// </summary>
        /// <param name="data">csv data</param>
        /// <param name="AscDesc">true = ascending, true = decending</param>
        public static void SortData(CsvDefinition csvdef, int SortIdx, bool AscDesc, bool ValLen)
        {
            // this should never happen
            if (SortIdx > csvdef.Fields.Count - 1)
            {
                string errmsg = string.Format("Sort on column index out of bounds, index is {0} and column count is {1}", SortIdx, csvdef.Fields.Count);
                //throw new ArgumentException(errmsg);
                MessageBox.Show(errmsg, "Sort on column error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;
            var CRLF = getEditorEOLchars(scintillaGateway.GetEOLMode());

            // examine data and keep list of all data lines
            // Note: can be a dictionary, not a list, because the sortable values are guaranteed to be unique
            Dictionary<string, string> sortlines = new Dictionary<string, string>();
            var strdata = ScintillaStreams.StreamAllText();

            // variables to read original data file
            List<string> values;
            var linecount = 0;
            var sep = csvdef.Separator;

            // sort on column, get column information
            CsvColumn csvcol = csvdef.Fields[SortIdx];
            var ApplyQuotes = Main.Settings.ReformatQuotes;

            // output in new sort order
            StringBuilder sbsort = new StringBuilder();

            // copy any comment lines
            csvdef.CopyCommentLinesAtStart(strdata, sbsort, "", CRLF);

            // if first line is header column names
            if (csvdef.ColNameHeader) {
                // consume line and copy to output
                values = csvdef.ParseNextLine(strdata, out bool iscomm);

                sbsort.Append(csvdef.ConstructLine(values, iscomm));

                sbsort.Append(CRLF);
            }

            string sortval = "";

            // read all data lines
            while (!strdata.EndOfStream)
            {
                // get next line of values
                values = csvdef.ParseNextLine(strdata, out bool iscomm);

                // when comment line then keep the same sortvalue as previous line, so that comment lines stay (more or less) in the same place in the data
                if (!iscomm)
                {
                    // construct sortable value
                    var val = (SortIdx < values.Count ? values[SortIdx] : "");

                    // sort on values or on length of values
                    if (ValLen)
                        sortval = SortableString(val, csvcol); // sort on values
                    else
                        sortval = val.Length.ToString().PadLeft(8, '0');  // sort on length of values. Implicit assumption: max. string length = 99999999 characters
                }

                // reconstruct original line of data
                var line = csvdef.ConstructLine(values, iscomm);

                // add to list
                sortlines.Add(sortval + linecount.ToString("D10"), line); // add linecount so guaranteed unique + retain original sort order for equal values

                // next line
                linecount += 1;
            }
            strdata.Dispose();

            // apply sorting, obj.Key contains sortable value and obj.Value contains the original line of data
            if (AscDesc == true)
                sortlines = sortlines.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            else
                sortlines = sortlines.OrderByDescending(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

            // add all lines, sort by count
            foreach (KeyValuePair<string, string> rec in sortlines)
            {
                sbsort.Append(string.Format("{0}{1}", rec.Value, CRLF));
            }

            // update text in editor
            scintillaGateway.SetText(sbsort.ToString());
        }

        /// <summary>
        /// split column into new columns
        /// </summary>
        /// <param name="data">csv data</param>
        /// <param name="SplitCode">1 = Pad, 2, Search Replace, 3 = Split valid/invalid, 4 = Split character, 5 = Split position</param>
        public static void ColumnSplit(CsvDefinition csvdef, int ColumnIndex, int SplitCode, string Parameter1, string Parameter2, bool bRemove)
        {
            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;
            var CRLF = getEditorEOLchars(scintillaGateway.GetEOLMode());

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            //var s = new StringReader(data);
            int linenr = 0;

            // when split csv values then add 2 new columns, when edit then just 1
            var addmax = (SplitCode > 0 ? (SplitCode > 2 ? 2 : 1) : 0); // 0=no new column, 1=new column (edit) or 2=new colunms (split)

            // parameter 2 as integer and abs(integer)
            int.TryParse(Parameter2, out int IntPar2);
            var IntPar2a = -1 * IntPar2;

            // variables for new output
            StringBuilder datanew = new StringBuilder();
            var sep = csvdef.Separator.ToString();
            var csvvalid = new CsvValidate();

            // copy csvdefinition to add columns
            CsvDefinition csvnew = new CsvDefinition(csvdef);

            // when split csv values then add 2 new columns, when edit then just 1
            if (ColumnIndex < csvdef.Fields.Count) {
                // insert new columns into definition
                for (var cnew = 0; cnew < addmax; cnew++)
                {
                    // get new name
                    var newname = csvnew.GetUniqueColumnName(csvdef.Fields[ColumnIndex].Name, out int postfix);
                    newname = string.Format("{0} ({1})", newname, postfix);

                    // when splitting values, the new column width is unknown
                    var wid = csvdef.Fields[ColumnIndex].MaxWidth;
                    if (SplitCode == 5) { // except when split on exact position
                        if (IntPar2 > 0)
                        {
                            if (cnew == 0) wid = IntPar2;
                            if (cnew != 0) wid = csvdef.Fields[ColumnIndex].MaxWidth - IntPar2;
                        }
                        else //if (IntPar2 < 0)
                        {
                            if (cnew == 0) wid = csvdef.Fields[ColumnIndex].MaxWidth - IntPar2a;
                            if (cnew != 0) wid = IntPar2a;
                        }
                    }

                    //var newcol = new CsvColumn(csvdef.Fields[ColumnIndex]);
                    var newcol = new CsvColumn(ColumnIndex, newname, wid, ColumnType.String, "");
                    //var newcol = new CsvColumn(csvdef.Fields[ColumnIndex]);

                    // insert into 
                    csvnew.Fields.Insert(ColumnIndex + cnew + 1, newcol);
                }
            }
            // remove column
            if (bRemove) {
                csvnew.Fields.RemoveAt(ColumnIndex);
            }

            // copy any comment lines
            csvdef.CopyCommentLinesAtStart(strdata, datanew, "", CRLF);

            // list for building new columns
            List<string> newcols = new List<string>();

            // convert from fixed width to separated values, add header line
            if (csvdef.ColNameHeader)
            {
                // if first line is header column names, then consume line and ignore
                csvdef.ParseNextLine(strdata, out _); // bool iscomm should always be false here, because already skipped any comment lines and ColNameHeader=true

                // add new header with new column names
                for (int colhead = 0; colhead < csvnew.Fields.Count; colhead++)
                {
                    newcols.Add(csvnew.Fields[colhead].Name);
                }

                datanew.Append(csvnew.ConstructLine(newcols, false));

                datanew.Append(CRLF);
            }

            // read all lines
            while (!strdata.EndOfStream)
            {
                // clear temp list
                newcols.Clear();

                // get values from line
                List<string> values = csvdef.ParseNextLine(strdata, out bool iscomm);

                if (iscomm)
                {
                    csvdef.CopyCommentLine(values, datanew, "", "");
                }
                else
                {
                    linenr++;

                    // reformat data line to new line
                    for (int col = 0; col < values.Count; col++)
                    {
                        // next value
                        string val = values[col];

                        // add column to output, except when remove original column
                        if ((col != ColumnIndex) || (bRemove == false))
                        {
                            newcols.Add(val);
                        }

                        if ((col == ColumnIndex) && (SplitCode > 0))
                        {
                            // split column
                            string val1 = val;
                            string val2 = "";

                            if (SplitCode == 1)
                            {
                                // pad with character
                                if (IntPar2 > 0)
                                {
                                    val1 = val.PadLeft(IntPar2, Parameter1[0]); // left pad
                                }
                                else
                                {
                                    val1 = val.PadRight(IntPar2a, Parameter1[0]); // right pad
                                }
                            }
                            else if (SplitCode == 2)
                            {
                                // search and replace
                                val1 = val.Replace(Parameter1, Parameter2);
                            }
                            else if (SplitCode == 3)
                            {
                                // valid/invalid
                                var str = csvvalid.EvaluateDataValue(val, csvdef.Fields[ColumnIndex], ColumnIndex);
                                if (str != "")
                                {
                                    val1 = "";
                                    val2 = val; // invalid value
                                }
                            }
                            else if (SplitCode == 4)
                            {
                                // split on char
                                int pos = -1;
                                int index = 0;

                                int i = 0;
                                if (IntPar2 >= 0)
                                {
                                    // first Nth occurance
                                    while (++i <= IntPar2 && (index = val.IndexOf(Parameter1, index)) != -1)
                                    {
                                        if (i == IntPar2)
                                        {
                                            pos = index;
                                            break;
                                        }
                                        index++;
                                    }
                                }
                                else
                                {
                                    // last Nth occurance
                                    index = val.Length;
                                    while (--i >= IntPar2 && (index = val.LastIndexOf(Parameter1, index)) != -1)
                                    {
                                        if (i == IntPar2)
                                        {
                                            pos = index;
                                            break;
                                        }
                                        if (--index < 0) break; // LastIndexOf crashes on a negative startIndex
                                    }
                                }

                                if (pos >= 0)
                                {
                                    val1 = val.Substring(0, pos);
                                    val2 = val.Substring(pos + Parameter1.Length, val.Length - pos - Parameter1.Length);
                                }
                            }
                            else if (SplitCode == 5)
                            {
                                // split on position
                                if ((IntPar2 > 0) && (IntPar2 < val.Length))
                                {
                                    // positive, left string
                                    val1 = val.Substring(0, IntPar2);
                                    val2 = val.Substring(IntPar2, val.Length - IntPar2);
                                }
                                else if (IntPar2 < 0)
                                {
                                    // negative, right string
                                    if (IntPar2a < val.Length)
                                    {
                                        val1 = val.Substring(0, val.Length - IntPar2a);
                                        val2 = val.Substring(val.Length - IntPar2a);
                                    }
                                    else
                                    {
                                        // take all as right string
                                        val1 = "";
                                        val2 = val;
                                    }
                                }
                            }

                            // add edit/split column values
                            newcols.Add(val1);

                            // add split column values
                            if (SplitCode > 2) newcols.Add(val2);
                        }
                    };

                    // reconstruct new line of data (with extra columns)
                    datanew.Append(csvnew.ConstructLine(newcols, iscomm));

                    // add line break
                    datanew.Append(CRLF);
                }
            };

            strdata.Dispose();

            // update text in editor
            scintillaGateway.SetText(datanew.ToString());

            // add columns to csv definition
            //csvdef.AddColumn() ??
        }
    }
}
