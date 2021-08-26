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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLint
{
    class CsvEdit
    {

        /// <summary>
        ///     variable safe name, no spaces
        /// </summary>
        public static string StringToVariable(string strinput)
        {
            return strinput.Replace(" ", "_");
        }

        /// <summary>
        ///     reformat file for date, decimal and separator
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void ReformatDataFile(CsvDefinition csvdef, string reformatDatTime, string reformatDecimal, string reformatSeparator, bool updateSeparator, bool trimAll)
        {
            // TODO: nullable parameters

            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;

            // use stringreader to go line by line
            var strdata = ScintillaStreams.StreamAllText();

            //var s = new StringReader(data);
            int linenr = 0;
            String line;

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
                List<string> values = csvdef.ParseNextLine(strdata);

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
                                val = DateTime.ParseExact(val, csvdef.Fields[c].Mask, CultureInfo.InvariantCulture).ToString(reformatDatTime, CultureInfo.InvariantCulture);
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

                        // fixed width, text align left - numeric align right
                        wid = csvdef.Fields[c].MaxWidth;
                        alignleft = !((csvdef.Fields[c].DataType == ColumnType.Integer) || (csvdef.Fields[c].DataType == ColumnType.Decimal));
                    }

                    // construct new output data line
                    if (newSep == '\0')
                    {
                        // fixed width
                        if (alignleft)
                        {
                            datanew.Append(val.PadRight(wid, ' '));
                        }
                        else
                        {
                            datanew.Append(val.PadLeft(wid, ' ')); 
                        };

                    }
                    else
                    {
                        // if value contains separator character then put value in quotes
                        if (val.IndexOf(newSep) >= 0) val = string.Format("\"{0}\"", val);

                        // character separated
                        datanew.Append((c > 0 ? newSep.ToString() : "") + val);
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
        ///     update all date columns to new date format
        /// </summary>
        public static void ConvertToSQL(CsvDefinition csvdef)
        {
            StringBuilder sb = new StringBuilder();
            string VERSION_NO = Main.GetVersion();
            //int MAX_SQL_ROWS = Main.Settings.SQLBatchRows;
            int MAX_SQL_ROWS = 1000;

            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            string TABLE_NAME = StringToVariable(Path.GetFileNameWithoutExtension(notepad.GetCurrentFilePath()));

            sb.Append("-- -------------------------------------\r\n");
            sb.Append(string.Format("-- CSV Lint plug-in v{0}\r\n", VERSION_NO));
            sb.Append(string.Format("-- File: {0}\r\n", FILE_NAME));
            sb.Append(string.Format("-- Date: {0}\r\n", DateTime.Now.ToString("dd-MMM-yyyy HH:mm")));
            sb.Append("-- -------------------------------------\r\n");
            sb.Append(string.Format("CREATE TABLE {0}(\r\n\t", TABLE_NAME));
            var cols = "\t";

            for (var r = 0; r < csvdef.Fields.Count; r++)
            {
                // determine sql column name
                var sqlname = '[' + csvdef.Fields[r].Name + ']';

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

                    sb.Append("\r\ngo\r\n");
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
                        if (str == "")
                        {
                            str = "NULL";
                        }
                        else if (csvdef.Fields[r].DataType == ColumnType.Decimal)
                        {
                            str = str.Replace(",", ".");
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
                                    var dt = DateTime.ParseExact(str, csvdef.Fields[r].Mask, System.Globalization.CultureInfo.InvariantCulture);
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
            sb.Append("\r\ngo\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(sb.ToString());
        }

        /// <summary>
        ///     update all date columns to new date format
        /// </summary>
        public void UpdateAllDateFormat()
        {
            // read all text and replace
            //var sr = ScintillaStreams.StreamAllText();
            //ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;
        }

        /// <summary>
        ///     update all decimal columns to new decimal format
        /// </summary>
        public void UpdateAllDecimal()
        {
            // TODO implement
        }

        /// <summary>
        ///     update a single column to new data type
        /// </summary>
        public void UpdateColumn()
        {
            // TODO implement
        }

        /// <summary>
        ///     split invalid values of column into a new column
        /// </summary>
        public void SplitColumn()
        {
            // TODO implement
        }
    }
}
