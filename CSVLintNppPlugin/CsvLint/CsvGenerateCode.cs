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
        /// Convert standard datetime mask (using in C#, to the string mask using in C strftime
        /// </summary>
        /// <param name="data"> csv data </param>
        private static string DateMaskStandardToCstr(string mask)
        {
            // build Python fomat example "M/d/yyyy HH:m:s" -> "%m/%d/%Y %H:%M:%S"
            // Note that in R-script lowercase m = month and capital M = minutes
            mask = mask.Replace("HH", "H");
            mask = mask.Replace("H", "%H"); // hour
            mask = mask.Replace("mm", "m");
            mask = mask.Replace("m", "n"); // minutes, use temporary 'n' placeholder
            mask = mask.Replace("ss", "s");
            mask = mask.Replace("s", "%S"); // seconds

            mask = mask.Replace("yy", "y");
            mask = mask.Replace("yy", "y");
            mask = mask.Replace("yy", "y");
            mask = mask.Replace("y", "%Y"); // year
            mask = mask.Replace("MM", "M");
            mask = mask.Replace("M", "%m"); // month
            mask = mask.Replace("dd", "d");
            mask = mask.Replace("d", "%d"); // day

            mask = mask.Replace("n", "%M"); // in C strftime lowercase m = month and capital M = minutes, opposite of internal mask format, use temporary 'n' to work around this

            return mask;
        }

        /// <summary>
        /// Get list of widths fixed width
        /// </summary>
        /// <param name="csvdef"> csvdefinition </param>
        /// <param name="abspos"> absolute column positions or column widths</param>
        private static string GetColumnWidths(CsvDefinition csvdef, bool abspos)
        {
            var res = (abspos ? "0, " : "");
            var colwidth = 0;

            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                if (abspos) {
                    colwidth += csvdef.Fields[c].MaxWidth;
                } else {
                    colwidth = csvdef.Fields[c].MaxWidth;
                }
                var comma = (c < csvdef.Fields.Count-1 ? ", " : "");
                res += string.Format("{0}{1}", colwidth, comma);
            }

            return res;
        }

        /// <summary>
        /// Convert xxx to xxx date format
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
        public static void GenerateSchemaJSON(CsvDefinition csvdef)
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

            if (csvdef.Separator == '\0')
                jsonmeta.Append(string.Format("\t\"columnpositions\": [{0}],\r\n", GetColumnWidths(csvdef, true)));

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
            notepad.SetCurrentLanguage(LangType.L_JSON);
        }

        /// <summary>
        /// generate Python Panda code based on columns (most asked on stackoverflow)
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void GeneratePythonPanda(CsvDefinition csvdef)
        {
            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // Python requires forward slash for filepaths
            string FILE_PATH = Path.GetDirectoryName(notepad.GetCurrentFilePath());
            string FILE_NAME = notepad.GetCurrentFilePath();

            FILE_PATH = FILE_PATH.Replace("\\", "\\\\");
            FILE_NAME = FILE_NAME.Replace("\\", "\\\\");

            StringBuilder python = new StringBuilder();

            // build Python script
            python.Append("# Python - read csv with datatypes\r\n");

            // default comment
            List<String> comment = CsvEdit.ScriptInfo(notepad);
            foreach (var str in comment) python.Append(string.Format("# {0}\r\n", str));

            // warning
            python.Append("#\r\n# NOTE:\r\n");
            python.Append("# This is a generated script and it doesn't handle all potential data errors.\r\n");
            python.Append("# The script is meant as a starting point for processing your data files.\r\n");
            python.Append("# Adjust and expand the script for your specific data processing needs.\r\n");
            python.Append("# Always back-up your data files to prevent data loss.\r\n\r\n");

            python.Append("# Library\r\n");
            python.Append("import os\r\n");
            python.Append("import numpy as np\r\n");
            python.Append("import pandas as pd\r\n\r\n");

            python.Append("# working directory and filename\r\n");
            python.Append(string.Format("os.chdir(\"{0}\")\r\n", FILE_PATH));
            python.Append(string.Format("filename = \"{0}\"\r\n\r\n", FILE_NAME));

            var col_names = "";
            var col_types = "";
            var col_dates = "";
            var col_datef = "";
            var col_ints = "";

            var exampleDate = "";

            var r_dec = "";

            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // any characters are allowed in Python column names
                var colname = coldef.Name;
                //colname = Regex.Replace(colname, "[^a-zA-Z0-9]", "_"); // not letter or digit

                var comma = (c < csvdef.Fields.Count - 1 ? "," : "");

                // list all column names
                col_names += string.Format("    '{0}'{1}\r\n", colname, comma);

                // indent for next lines
                //if (c > 0) col_types += "              ";

                // Python datetypes
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        // build Python fomat example "M/d/yyyy HH:m:s" -> "%m/%d/%Y %H:%M:%S"
                        var msk = coldef.Mask;
                        msk = DateMaskStandardToCstr(msk);

                        col_datef += string.Format("'{0}'{1} ", msk, comma); // formats
                        col_dates += string.Format("'{0}'{1} ", colname, comma); // names
                        if (exampleDate == "") exampleDate = colname;
                        break;
                    case ColumnType.Integer:
                        col_types += string.Format("    \"{0}\": np.int64{1}\r\n", colname, comma);
                        col_ints += string.Format("#df['{0}'] = df['{0}'].astype(str).str.rstrip('0').str.rstrip('.')\r\n", colname);

                        break;
                    case ColumnType.Decimal:
                        col_types += string.Format("    \"{0}\": str{1} # numeric\n", colname, comma);

                        // just use the first decimal symbol
                        if (r_dec == "") r_dec = coldef.DecimalSymbol.ToString();
                        break;
                    default:
                        col_types += string.Format("    \"{0}\": str{1}\r\n", colname, comma);
                        break;
                };
            }

            python.Append("# column datatypes\r\n");
            python.Append("# NOTE: using colClasses parameter doesn't work when for example integers are in quotes etc.\r\n");
            python.Append("# and read.csv will mostly interpret datatypes correctly anyway\r\n");

            // no decimals, then not technically needed but nice to have as example code
            if (r_dec == "") r_dec = ".";

            // colnames
            var nameparam = ", header=0";
            if (!csvdef.ColNameHeader)
            {
                python.Append(string.Format("col_names = [\r\n{0}]\r\n", col_names));
                nameparam = ", names=col_names, header=None";
            }

            // column types
            python.Append(string.Format("col_types = {{\r\n{0}}}\r\n", col_types));

            // read csv file
            var separator = (csvdef.Separator == '\0' ? "{fixed-width}" : csvdef.Separator.ToString());
            if (separator == "\t") separator = "\\t";

            // date time format parameter
            if (col_dates != "")
            {
                // remove last comma
                col_datef = col_datef.Remove(col_datef.Length - 2); // remove last separator", "
                col_dates = col_dates.Remove(col_dates.Length - 2); // remove last separator", "

                python.Append("\r\n");
                python.Append(string.Format("# datetime columns; {0}\r\n", col_datef));
                python.Append(string.Format("col_dates = [{0}]\r\n\r\n", col_dates));

                col_dates = ", parse_dates=col_dates";
            }

            if (csvdef.Separator == '\0')
            {
                // fixed width
                python.Append(string.Format("# fixed width, positions {0}\r\n", GetColumnWidths(csvdef, true)));
                python.Append(string.Format("col_widths = [{0}]\r\n", GetColumnWidths(csvdef, false)));
                python.Append(string.Format("df = pd.read_fwf(filename, decimal='{0}'{1}{2}, dtype=col_types, widths=col_widths)\r\n\r\n", r_dec, nameparam, col_dates));
            }
            else
            {
                // character separated
                python.Append("# read csv file\r\n");
                python.Append(string.Format("#df = pd.read_csv(filename, sep='{0}', decimal='{1}'{2}{3}, dtype=col_types)\r\n", separator, r_dec, nameparam, col_dates));
                python.Append(string.Format("df = pd.read_csv(filename, sep='{0}', decimal='{1}'{2}{3})\r\n\r\n", separator, r_dec, nameparam, col_dates));
            }

            // integer warnings
            if (col_ints != "")
            {
                python.Append("# NOTE: Python treats NaN values as float, thus columns with Int64+NaNs are converted to float,\r\n");
                python.Append("# you can convert them to string and then rstrip to undo the float '.0' parts\r\n");
                python.Append(col_ints);
                python.Append("\r\n");
            }

            // datetime warnings
            if (col_dates != "")
            {
                python.Append("# NOTE: Python treats datetime columns that also have NaN/string values as string\r\n\r\n");
            }

            // Python datatype warning
            if ( (col_ints != "") || (col_dates != "") ) python.Append("# double check datatypes\r\nprint(df.dtypes)\r\n\r\n");

            // Python examples of typical data transformations
            python.Append("# --------------------------------------\r\n");
            python.Append("# Data transformation suggestions\r\n");
            python.Append("# --------------------------------------\r\n\r\n");

            python.Append("# reorder or remove columns\r\n");
            python.Append(string.Format("df = df[[\r\n{0}]]\r\n", col_names));

            if (exampleDate == "") exampleDate = "myDateField";
            python.Append("# date to string format MM/dd/yyyy\r\n");
            python.Append(string.Format("#df['{0}'] = df['{0}'].dt.strftime('%m/%d/%Y')\r\n\r\n", exampleDate));

            python.Append("# replace labels with codes, for example column contains 'Yes' or 'No' replace with '1' or '0'\r\n");
            python.Append("#lookuplist = {'Yes': 1, 'No': 0}\r\n");
            python.Append("#df['fieldyesno_code'] = df['fieldyesno'].map(lookuplist)\r\n\r\n");

            if (csvdef.Separator == '\0') separator = ",";
            python.Append("# csv write new output\r\n");
            python.Append("filenew = \"output.txt\"\r\n");
            python.Append(string.Format("df.to_csv(filenew, sep='{0}', decimal=',', na_rep='', header=True, index=False, encoding='utf-8')\r\n", separator));

            // create new file
            notepad.FileNew();
            editor.SetText(python.ToString());
            notepad.SetCurrentLanguage(LangType.L_PYTHON);
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

            // fixed width, also output absolute column positions
            var colwidth = "";
            if (csvdef.Separator == '\0') colwidth = string.Format("\r\n; Fixed Length positions {0}\r\n", GetColumnWidths(csvdef, true));

            // also add filename
            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            var txt = string.Format("[{0}]\r\n{1}{2}", FILE_NAME, csvdef.GetIniLines().ToString(), colwidth);

            // create new file
            notepad.FileNew();
            editor.SetText(txt);
            notepad.SetCurrentLanguage(LangType.L_INI);
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
            rscript.Append("# This is a generated script and it doesn't handle all potential data errors.\r\n");
            rscript.Append("# The script is meant as a starting point for processing your data files.\r\n");
            rscript.Append("# Adjust and expand the script for your specific data processing needs.\r\n");
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

                        // build R-script fomat example "M/d/yyyy HH:m:s" -> "%m/%d/%Y %H:%M:%S"
                        msk = DateMaskStandardToCstr(msk);

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
                rscript.Append(string.Format("# fixed width, positions {0}\r\n", GetColumnWidths(csvdef, true)));
                rscript.Append(string.Format("colWidths <- c({0})\r\n", GetColumnWidths(csvdef, false)));
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

            rscript.Append("# replace labels with codes, for example column contains 'Yes' or 'No' replace with '1' or '0'\r\n");
            rscript.Append("#lookuplist <- data.frame(\"code\" = c(\"0\", \"1\"),\r\n");
            rscript.Append("#                    \"label\" = c(\"No\", \"Yes\") )\r\n");
            rscript.Append("#fieldyesno_code <- lookuplist$code[match(df$fieldyesno, lookuplist$label)]\r\n\r\n");

            rscript.Append("# csv write new output\r\n");
            rscript.Append("filenew = \"output.txt\"\r\n");
            rscript.Append("write.table(df, file=filenew, sep=\";\", dec=\",\", na=\"\", row.names=FALSE)\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(rscript.ToString());
            notepad.SetCurrentLanguage(LangType.L_R);
        }
    }
}
