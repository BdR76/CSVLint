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
                var dec = "";
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
                        //mask = coldef.Mask;
                        mask = "#0" + coldef.DecimalSymbol + "".PadRight(coldef.Decimals, '0');
                        dec = coldef.DecimalSymbol.ToString();
                        break;
                };

                // JSON definition per field
                if (c > 0) jsonmeta.Append(",");
                jsonmeta.Append("\r\n\t\t\t{\r\n");

                jsonmeta.Append(string.Format("\t\t\t\t\"name\": \"{0}\"", coldef.Name));
                if ((mask != "") && (dec != ""))
                {
                    jsonmeta.Append("\r\n\t\t\t\t\"datatype\": {");
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\t\"base\": \"{0}\"", dattyp));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\t\"length\": \"{0}\"", len));
                    jsonmeta.Append(",\r\n\t\t\t\t\t\"format\": {");
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\t\t\"decimalChar\": \"{0}\"", dec));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\t\t\"pattern\": \"{0}\"", mask));
                    jsonmeta.Append("\r\n\t\t\t\t\t}");
                    jsonmeta.Append("\r\n\t\t\t\t}");
                }
                else if (mask != "")
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
            rscript.Append("# NOTE: using colClasses parameter doesn't work when for example integers are in quotes etc.\r\n");
            rscript.Append("# and read.csv will mostly interpret datatypes correctly anyway\r\n");

            var col_names = "c(";
            var col_types = "c(";
            var col_dates = "";
            var col_numbs = "";
            var col_widths = "c(";

            var exampleDate = "myDateField";

            var r_dec = "";

            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // R-script safe tag, replace .
                var colname = coldef.Name;
                colname = Regex.Replace(colname, "[^a-zA-Z0-9]", "_"); // not letter or digit

                var comma = (c < csvdef.Fields.Count - 1 ? "," : ")");


                //col_widths += coldef.MaxWidth.ToString() + comma;
                col_widths += string.Format("{0}{1} ", coldef.MaxWidth, comma);

                var indent = (c > 0 ? "              " : "");

                col_names += string.Format("{0}\"{1}\"{2}\r\n", indent, colname, comma);

                // indent for next lines
                if (c > 0) col_types += "              ";

                // R-script datetypes
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        col_types += string.Format("\"{0}\" = \"character\"{1} # {2}\r\n", colname, comma, coldef.Mask);
                        var msk = coldef.Mask;

                        // build R-date fomat example "M/d/yyyy HH:m:s" -> "%m/%d/%Y %H:%M:%S"
                        // Note that in R-script lowercase m = month and capital M = minutes
                        msk = msk.Replace("HH", "H");
                        msk = msk.Replace("H", "%H"); // hour
                        msk = msk.Replace("mm", "m");
                        msk = msk.Replace("m", "n"); // minutes, use temporary 'n' placeholder
                        msk = msk.Replace("ss", "s");
                        msk = msk.Replace("s", "%S"); // seconds

                        msk = msk.Replace("yy", "y");
                        msk = msk.Replace("yy", "y");
                        msk = msk.Replace("yy", "y");
                        msk = msk.Replace("y", "%Y"); // year
                        msk = msk.Replace("MM", "M");
                        msk = msk.Replace("M", "%m"); // month
                        msk = msk.Replace("dd", "d");
                        msk = msk.Replace("d", "%d"); // day

                        msk = msk.Replace("n", "%M"); // in R-script lowercase m = month and capital M = minutes, opposite of internal mask format, use temporary 'n' to work around this

                        var rtype = (msk.IndexOf("H") == -1 ? "Date" : "POSIXct"); // date OR datetime

                        col_dates += string.Format("df${0} <- as.{1}(df${0}, format=\"{2}\")\r\n", colname, rtype, msk);
                        exampleDate = colname;
                        break;
                    case ColumnType.Integer:
                        col_types += string.Format("\"{0}\" = \"integer\"{1}\r\n", colname, comma);
                        break;
                    case ColumnType.Decimal:
                        col_types += string.Format("\"{0}\" = \"character\"{1} # numeric\n", colname, comma);
                        col_numbs += string.Format("df${0} <- as.numeric(df${0})\r\n", colname);

                        // just use the first decimal symbol
                        if (r_dec == "") r_dec = coldef.DecimalSymbol.ToString();
                        break;
                    default:
                        col_types += string.Format("\"{0}\" = \"character\"{1}\r\n", colname, comma);
                        break;
                };
            }

            // no decimals, then not technically needed but nice to have as example code
            if (r_dec == "") r_dec = ".";

            // colnames
            var nameparam = "";
            if (!csvdef.ColNameHeader)
            {
                rscript.Append(string.Format("colNames <- {0}\r\n", col_names));
                nameparam = "col.name=colNames, ";
            }

            // column types
            rscript.Append(string.Format("colTypes <- {0}\r\n", col_types));

            // read csv file
            var separator = (csvdef.Separator == '\0' ? "{fixed-width}" : csvdef.Separator.ToString());
            if (separator == "\t") separator = "\\t";
            var header = (csvdef.ColNameHeader ? "TRUE" : "FALSE");

            if (csvdef.Separator == '\0')
            {
                // fixed width
                rscript.Append("# fixed width\r\n");
                rscript.Append(string.Format("colWidths <- {0}\r\n", col_widths));
                rscript.Append(string.Format("df <- read.fwf(filename, {0}colClasses=colTypes, width=colWidths, stringsAsFactors=FALSE, comment.char='', header={1})\r\n\r\n", nameparam, header));
            } else {
                // character separated
                rscript.Append("# read csv file\r\n");
                rscript.Append(string.Format("#df <- read.csv(filename, sep='{0}', dec=\"{1}\", {2}colClasses=colTypes, header={3})\r\n", separator, r_dec, nameparam, header));
                rscript.Append(string.Format("df <- read.csv(filename, sep='{0}', dec=\"{1}\", {2}header={3})\r\n\r\n", separator, r_dec, nameparam, header));
            }

            // date time format script
            if (col_dates != "")
            {
                rscript.Append("# datetime values\r\n");
                rscript.Append("# NOTE: any datetime formatting errors will result in empty/NA values without any warning\r\n");
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

            // R-script examples of typical data transformations
            rscript.Append("# --------------------------------------\r\n");
            rscript.Append("# Data transformation suggestions\r\n");
            rscript.Append("# --------------------------------------\r\n\r\n");

            rscript.Append("# reorder columns\r\n");
            rscript.Append(string.Format("colOrder <- {0}", col_names));
            rscript.Append("df <- df[, colOrder]\r\n\r\n");

            rscript.Append("# date to string format MM/dd/yyyy\r\n");
            rscript.Append(string.Format("df${0} <- format(df${0}, \"%m/%d/%Y\")\r\n\r\n", exampleDate));

            rscript.Append("# replace codes with labels\r\n");
            rscript.Append("#lookuplist <- data.frame(\"code\" = c(\"0\", \"1\"),\r\n");
            rscript.Append("#                    \"label\" = c(\"No\", \"Yes\") )\r\n");
            rscript.Append("#df$fieldyesno <- lookuplist$label[match(df$fieldyesno, lookuplist$code)]\r\n\r\n");

            rscript.Append("# csv write new output\r\n");
            rscript.Append("filenew = \"output.txt\"\r\n");
            rscript.Append("write.table(df, file=filenew, sep=\";\", dec=\",\", na=\"\", row.names=FALSE)\r\n");

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
