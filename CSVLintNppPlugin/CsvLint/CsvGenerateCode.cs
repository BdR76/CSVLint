// -------------------------------------
// CsvEdit
// make edits to csv data
// based input and CsvDefinition
// makes changes to both the data and the CsvDefinition
// -------------------------------------
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVLint
{
    class CsvGenerateCode
    {

        /// <summary>
        /// determine short variable names based on column names
        /// </summary>
        /// <param name="data"> csv data </param>
        public void DetermineVariableNames(string data)
        {
            // shorten variable names based on column names
        }

        /// <summary>
        /// determine short variable names based on column names
        /// </summary>
        /// <param name="data"> csv data </param>
        public string HeaderComment()
        {
            // default comment for all scripts
            //return "Generated using Notepad++ CSV Lint plug-in"
            //"Date: 10-jul-2020 12:22"
            //"Inputfile: xxx.txt"
            //"Comma separated data, contains header row, date format dd-mm-yyyy, decimal is '.'"
            //"Output: xxx(processed).txt"
            return "Header Comment";
        }

        /// <summary>
        /// generate JSON metadata
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void GenerateJSONmetadata(CsvDefinition csvdef)
        {
            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            var separator = (csvdef.Separator == '\0' ? "{fixed-width}" : csvdef.Separator.ToString());
            if (separator == "\t") separator = "\\t";

            StringBuilder jsonmeta = new StringBuilder();

            // build JSON
            jsonmeta.Append("{\r\n");
            jsonmeta.Append(string.Format("\t\"url\": \"{0}\",\r\n", FILE_NAME));
            jsonmeta.Append(string.Format("\t\"separator\": \"{0}\",\r\n", separator));

            jsonmeta.Append("\t\"tableSchema\": {\r\n");
            jsonmeta.Append("\t\t\"columns\": [");


            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // prepare JSON variables
                var dattyp = "string";
                var mask = "";
                var len = coldef.MaxWidth.ToString();
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        mask = coldef.Mask;
                        dattyp = (mask.IndexOf("y") >= 0 ? "date" : "") + (mask.IndexOf("H") >= 0 ? "time" : "");
                        break;
                    case ColumnType.Integer:
                        dattyp = "integer";
                        break;
                    case ColumnType.Decimal:
                        dattyp = "number";
                        mask = coldef.Mask;
                        break;
                };

                // JSON definition per field
                if (c > 0) jsonmeta.Append(",");
                jsonmeta.Append("\r\n\t\t\t{\r\n");

                jsonmeta.Append(string.Format("\t\t\t\t\"name\": \"{0}\"", coldef.Name));
                if (mask != "")
                {
                    jsonmeta.Append("\r\n\t\t\t\t\"datatype\": {");
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\t\"base\": \"{0}\"", dattyp));
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\t\"length\": \"{0}\"", len));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\t\"format\": \"{0}\"", mask));
                    jsonmeta.Append("\r\n\t\t\t\t}");
                }
                else
                {
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\"datatype\": \"{0}\"", dattyp));
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\"length\": \"{0}\"", len));
                }

                jsonmeta.Append("\r\n\t\t\t}");
            }

            jsonmeta.Append("\r\n\t\t]\r\n");
            jsonmeta.Append("\t}\r\n");
            jsonmeta.Append("}\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(jsonmeta.ToString());
        }

        /// <summary>
        /// generate Python code based on columns (most asked on stackoverflow)
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePython(string data)
        {
            return "Python";
        }

        /// <summary>
        /// generate Python Panda code based on columns (most asked on stackoverflow)
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePythonPanda(string data)
        {
            // fixed
            // import pandas
            // # Using Pandas with a column specification
            // col_specification = [(0, 20), (21, 30), (31, 50), (51, 100)]
            // data = pandas.read_fwf(path, colspecs=col_specification)

            // separator
            //import pandas
            //df = pandas.read_csv('hrdata.csv', 
            //            index_col='Employee', 
            //            parse_dates=['Hired'],
            //            sep=';',
            //            header=0, 
            //            names=['Employee', 'Hired', 'Salary', 'Sick Days'])
            //df.to_csv('hrdata_modified.csv')

            return "Python Panda";
        }

        /// <summary>
        /// generate SPSS code based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GenerateSPSS(string data)
        {
            return "SPSS";
        }

        /// <summary>
        /// generate SQL based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GenerateSQL(string data)
        {
            return "SQL";
        }

        /// <summary>
        /// generate schema.ini based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void GenerateSchemaIni(CsvDefinition csvdef)
        {
            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // also add filename
            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            var txt = string.Format("[{0}]\r\n{1}", FILE_NAME, csvdef.GetIniLines().ToString());

            // create new file
            notepad.FileNew();
            editor.SetText(txt);
        }

        /// <summary>
        /// generate R-scripting code based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void GenerateRScript(CsvDefinition csvdef)
        {
            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // R-studio requires forward slash for filepaths
            string FILE_PATH = Path.GetDirectoryName(notepad.GetCurrentFilePath());
            string FILE_NAME = notepad.GetCurrentFilePath();

            FILE_PATH = FILE_PATH.Replace("\\", "/");
            FILE_NAME = FILE_NAME.Replace("\\", "/");

            StringBuilder rscript = new StringBuilder();

            // build R-script
            rscript.Append("# R-script - read csv with datatypes\r\n");

            // default comment
            List<String> comment = CsvEdit.ScriptInfo(notepad);
            foreach (var str in comment) rscript.Append(string.Format("# {0}\r\n", str));

            // warning
            rscript.Append("#\r\n# NOTE:\r\n");
            rscript.Append("# This is a generated script and it doesn't handle potential data errors.\r\n");
            rscript.Append("# The script is meant as a starting point for processing your data files.\r\n");
            rscript.Append("# Always back-up your data files to prevent data loss.\r\n\r\n");

            rscript.Append("# Library\r\n");
            rscript.Append("library(dplyr)\r\n\r\n");

            rscript.Append(string.Format("setwd(\"{0}\")\r\n\r\n", FILE_PATH));

            rscript.Append(string.Format("filename = \"{0}\"\r\n\r\n", FILE_NAME));

            rscript.Append("# column datatypes\r\n");

            var col_types = "c(";
            var col_dates = "";
            var col_numbs = "";

            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // R-script safe tag, replace .
                var colname = coldef.Name;
                colname = Regex.Replace(colname, "[^a-zA-Z0-9]", "."); // not letter or digit

                var comma = (c < csvdef.Fields.Count-1 ? "," : ")");

                // indent for next lines
                if (c > 0) col_types += "              ";

                // R-script datetypes
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        col_types += string.Format("\"{0}\"=\"character\"{1} # {2}\r\n", colname, comma, coldef.Mask);
                        var msk = coldef.Mask;

                        // build R-date fomat example "M/d/yyyy" -> "%m/%d/%Y"
                        msk = msk.Replace("HH", "H");
                        msk = msk.Replace("H", "%H"); // hour
                        msk = msk.Replace("mm", "m");
                        msk = msk.Replace("m", "%n"); // minutes
                        msk = msk.Replace("ss", "s");
                        msk = msk.Replace("s", "%s"); // seconds

                        msk = msk.Replace("yy", "y");
                        msk = msk.Replace("yy", "y");
                        msk = msk.Replace("yy", "y");
                        msk = msk.Replace("y", "%Y"); // year
                        msk = msk.Replace("MM", "M");
                        msk = msk.Replace("M", "%m"); // month
                        msk = msk.Replace("dd", "d");
                        msk = msk.Replace("d", "%d"); // day

                        col_dates += string.Format("df${0} <- as.Date(df${0}, format=\"{1}\")\r\n", colname, msk);
                        break;
                    case ColumnType.Integer:
                        col_types += string.Format("\"{0}\"=\"integer\"{1}\r\n", colname, comma);
                        break;
                    case ColumnType.Decimal:
                        col_types += string.Format("\"{0}\"=\"character\"{1} # numeric\n", colname, comma);
                        col_numbs += string.Format("df${0} <- as.numeric(df${0})\r\n", colname);
                        break;
                    default:
                        col_types += string.Format("\"{0}\"=\"character\"{1}\r\n", colname, comma);
                        break;
                };
            }

            // column types
            rscript.Append(string.Format("colTypes <- {0}\r\n", col_types));

            // read csv file
            var separator = (csvdef.Separator == '\0' ? "{fixed-width}" : csvdef.Separator.ToString());
            if (separator == "\t") separator = "\\t";
            var header = (csvdef.ColNameHeader ? "TRUE" : "FALSE");

            rscript.Append("# read csv file\r\n");
            rscript.Append(string.Format("df <- read.csv(filename, sep='{0}', dec=\".\", fileEncoding='UTF-8-BOM', colClasses=colTypes, header={1})\r\n\r\n", separator, header));

            // date time format script
            if (col_dates != "")
            {
                rscript.Append("# datetime values\r\n");
                rscript.Append("# NOTE: any datetime formatting errors will result in empty/NA values\r\n");
                rscript.Append(col_dates);
                rscript.Append("\r\n");
            }

            // numeric format script
            if (col_numbs != "")
            {
                rscript.Append("# numeric values\r\n");
                rscript.Append("# NOTE: the error message \"NAs introduced by coercion\" means there are decimal formatting errors\r\n");
                rscript.Append(col_numbs);
                rscript.Append("\r\n");
            }

            rscript.Append("# column update examples\r\n\r\n");

            // TODO: add R-script examples of create new column + reformat/update column + move column + delete column

            rscript.Append("# csv write new output\r\n");
            rscript.Append("filenew = \"output.txt\"\r\n");
            rscript.Append("write.table(df, file=filenew, sep=\";\", dec=\",\")\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(rscript.ToString());
        }

        /// <summary>
        /// generate PowerShell script based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePowerShell(string data)
        {
            return "PowerShell";
        }

        /// <summary>
        /// generate PHP code based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePHP(string data)
        {
            return "PHP";
        }
    }
}
