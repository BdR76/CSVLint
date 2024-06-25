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
        /// Standard disclaimer for generated scripts
        /// </summary>
        private static void ScriptDisclaimer(StringBuilder sb)
        {
            sb.Append("#\r\n# NOTE:\r\n");
            sb.Append("# This is a generated script and it doesn't handle all potential data errors.\r\n");
            sb.Append("# The script is meant as a starting point for processing your data files.\r\n");
            sb.Append("# Adjust and expand the script for your specific data processing needs.\r\n");
            sb.Append("# Always back-up your data files to prevent data loss.\r\n\r\n");
        }

        /// <summary>
        /// Standard disclaimer for generated scripts
        /// </summary>
        private static void ScriptHeader(StringBuilder sb, String stext)
        {
            sb.Append("# --------------------------------------\r\n");
            sb.Append(string.Format("# {0}\r\n", stext));
            sb.Append("# --------------------------------------\r\n");
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

            // file format
            jsonmeta.Append("\t\"dialect\": {");
            if (csvdef.Separator == '\0')
                jsonmeta.Append(string.Format("\r\n\t\t\"columnpositions\": [0, {0}]", csvdef.GetColumnWidths(true)));
            else
                jsonmeta.Append(string.Format("\r\n\t\t\"delimiter\": \"{0}\"", separator));
            jsonmeta.Append(string.Format(",\r\n\t\t\"header\": \"{0}\"", (csvdef.ColNameHeader ? "true" : "false")));
            if (csvdef.SkipLines > 0)
                jsonmeta.Append(string.Format(",\r\n\t\t\"skipRows\": \"{0}\"", csvdef.SkipLines));
            if (csvdef.CommentChar != '\0')
                jsonmeta.Append(string.Format(",\r\n\t\t\"commentPrefix\": \"{0}\"", csvdef.CommentChar));
            jsonmeta.Append("\r\n\t},\r\n");

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

                if (coldef.isCodedValue)
                {
                    var codedlist = string.Join("|", coldef.CodedList);
                    jsonmeta.Append(",\r\n\t\t\t\t\"datatype\": {");
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\t\"base\": \"{0}\"", dattyp));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\t\"format\": \"{0}\"", codedlist));
                    jsonmeta.Append("\r\n\t\t\t\t}");
                }
                else if ((mask != "") && (dec != ""))
                {
                    jsonmeta.Append(",\r\n\t\t\t\t\"datatype\": {");
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
                    jsonmeta.Append(",\r\n\t\t\t\t\"datatype\": {");
                    jsonmeta.Append(string.Format("\r\n\t\t\t\t\t\"base\": \"{0}\"", dattyp));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\t\"length\": \"{0}\"", len));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\t\"format\": \"{0}\"", mask));
                    jsonmeta.Append("\r\n\t\t\t\t}");
                }
                else
                {
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\"datatype\": \"{0}\"", dattyp));
                    jsonmeta.Append(string.Format(",\r\n\t\t\t\t\"length\": \"{0}\"", len));
                }

                jsonmeta.Append("\r\n\t\t\t}");
            }

            jsonmeta.Append("\r\n\t\t]\r\n");
            jsonmeta.Append("\t}\r\n");
            jsonmeta.Append("}\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(jsonmeta.ToString());
            if (jsonmeta.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_JSON);
            }
        }

        /// <summary>
        /// generate CSV datadictionary metadata
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void GenerateDatadictionaryCSV(CsvDefinition csvdef)
        {
            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            CsvDefinition datadict = new CsvDefinition(',');

            datadict.AddColumn("Nr", 8, ColumnType.Integer);
            datadict.AddColumn("ColumnName", 1000, ColumnType.String);
            datadict.AddColumn("DataType", 1000, ColumnType.String);
            datadict.AddColumn("Width", 8, ColumnType.Integer);
            datadict.AddColumn("Decimals", 8, ColumnType.Integer);
            datadict.AddColumn("Mask", 1000, ColumnType.String);
            datadict.AddColumn("Enumeration", 9999, ColumnType.String);

            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            var separator = (csvdef.Separator == '\0' ? "{fixed-width}" : csvdef.Separator.ToString());
            if (separator == "\t") separator = "\\t";

            StringBuilder csvmeta = new StringBuilder();
            List<string> sl = new List<string>();

            // build CSV
            csvmeta.Append("Nr,ColumnName,DataType,Width,Decimals,Mask,Enumeration\r\n");

            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // prepare JSON variables
                var dattyp = "String";
                var mask = "";
                var dec = "";
                var colwid = coldef.MaxWidth.ToString();
                var enumvals = "";
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        mask = coldef.Mask;
                        dattyp = (mask.IndexOf("y") >= 0 ? "Date" : "") + (mask.IndexOf("H") >= 0 ? "Time" : "");
                        break;
                    case ColumnType.Integer:
                        dattyp = "Integer";
                        break;
                    case ColumnType.Decimal:
                        dattyp = "Decimal";
                        //mask = coldef.Mask;
                        mask = "#0" + coldef.DecimalSymbol + "".PadRight(coldef.Decimals, '0');
                        dec = coldef.Decimals.ToString();
                        break;
                };

                // enumeration
                if (coldef.isCodedValue) enumvals = string.Join("|", coldef.CodedList);

                // add values as columns
                sl.Clear();
                sl.Add((c + 1).ToString()); // Nr
                sl.Add(coldef.Name);        // ColumnName
                sl.Add(dattyp);             // DataType
                sl.Add(colwid);             // Width
                sl.Add(dec);                // Decimals
                sl.Add(mask);               // Mask
                sl.Add(enumvals);           // Enumeration

                // Construct/format csv line
                csvmeta.Append(string.Format("{0}\r\n", datadict.ConstructLine(sl, false)));
            }

            // create new file
            notepad.FileNew();
            editor.SetText(csvmeta.ToString());
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

            // add standard disclaimer for generated scripts
            ScriptDisclaimer(python);

            // start Python script
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
            var col_enums = "";

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

                // enumeration
                if (coldef.isCodedValue) {
                    var enumvals = string.Join("\", \"", coldef.CodedList);
                    // Constrains for string or integer values
                    if (coldef.DataType == ColumnType.String)
                    {
                        enumvals = string.Format("\"{0}\"", enumvals); // use quotes
                    }
                    else
                    {
                        enumvals = enumvals.Replace("\"", ""); // no quotes
                    };
                    col_enums += string.Format("    \"{0}\": [{1}],\r\n", coldef.Name, enumvals);
                }

                // indent for next lines
                //if (c > 0) col_types += "              ";

                // Python datetypes
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        // build Python fomat example "M/d/yyyy HH:m:s" -> "%m/%d/%Y %H:%M:%S"
                        var msk = coldef.Mask;
                        msk = DateMaskStandardToCstr(msk);

                        col_datef += string.Format("'{0}', ", msk); // formats
                        col_dates += string.Format("'{0}', ", colname); // names
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

            // Python skip comment lines
            if (csvdef.SkipLines > 0) nameparam += string.Format(", skiprows={0}", csvdef.SkipLines);

            // Python comment character
            if (csvdef.CommentChar != '\0') nameparam += string.Format(", comment='{0}'", csvdef.CommentChar);

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
                python.Append(string.Format("# fixed width, positions {0}\r\n", csvdef.GetColumnWidths(true)));
                python.Append(string.Format("col_widths = [{0}]\r\n", csvdef.GetColumnWidths(false)));
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

            // Python enumeration check
            if (col_enums != "")
            {
                col_enums = col_enums.Remove(col_enums.Length - 3); // remove last comma + CrLf ",\r\n"
                python.Append(string.Format("# enumeration allowed values\r\nallowed_values = {{\r\n{0}\r\n}}\r\n\r\n", col_enums));
                python.Append("# check enumeration\r\ndf_invalid = {\r\n    column_name: df[~df[column_name].isin(allowed_values)]\r\n                 .value_counts(subset = column_name)\r\n                 .to_frame().reset_index(names = \"Invalid_value\")\r\n    for column_name, allowed_values in allowed_values.items()\r\n}\r\ndf_chk = pd.concat(df_invalid, names = (\"Column_name\", None)).droplevel(1)\r\n");
                python.Append("if not df_chk.empty:\r\n    print(\"Invalid values found:\")\r\n    print(df_chk)\r\n\r\n");
            }

            // Python examples of filtering, transformation, merge
            if (exampleDate == "") exampleDate = "myDateField";
            var exampleYear = DateTime.Now.Year;
            python.Append("# Remove or uncomment the script parts below to filter, transform, merge as needed\r\n\r\n");

            // -------------------------------------
            ScriptHeader(python, "Data filtering suggestions");
            // -------------------------------------
            python.Append("# filter on value or date range\r\n");
            python.Append("#df = df[(df[\"date_column\"] == \"test\")]\r\n");
            python.Append(string.Format("#df = df[(df[\"{0}\"] >= \"{1}-01-01\") & (df[\"{0}\"] < \"{1}-07-01\")]\r\n\r\n", exampleDate, exampleYear));

            python.Append("# Reorder or remove columns (edit code below)\r\n");
            python.Append(string.Format("df = df[[\r\n{0}]]\r\n\r\n", col_names));

            // -------------------------------------
            ScriptHeader(python, "Data transformation suggestions");
            // -------------------------------------
            python.Append("# Date to string example, format as MM/dd/yyyy\r\n");
            python.Append(string.Format("#df['{0}'] = df['{0}'].dt.strftime('%m/%d/%Y')\r\n\r\n", exampleDate));

            python.Append("# Replace labels with codes example, when column contains 'Yes' or 'No' replace with '1' or '0'\r\n");
            python.Append("#lookuplist = {'Yes': 1, 'No': 0}\r\n");
            python.Append("#df['yesno_int'] = df['yesno_str'].map(lookuplist)\r\n\r\n");

            python.Append("# Calculate new values example\r\n");
            python.Append("#df['bmi_calc'] = round(df['weight'] / (df['height'] / 100) ** 2, 1)\r\n");
            python.Append("#df['center_patient'] = df['centercode'].str.slice(0, 2) + '-' + df['patientcode'].map(str) # '01-123' etc\r\n\r\n");

            // -------------------------------------
            ScriptHeader(python, "Data merge example");
            // -------------------------------------

            python.Append("# Merge dataframes example, to join on multiple columns use a list, for example: on=['patient_id', 'center_id']\r\n");
            python.Append("#merged_df = pd.merge(df1, df2, how='left', on='patient_id') # same key column name\r\n");
            python.Append("#merged_df = pd.merge(df1, df2, how='left', left_on='df1 key', right_on='df2 id') # different key column names\r\n\r\n");

            if (csvdef.Separator == '\0') separator = ",";
            python.Append("# csv write new output\r\n");
            python.Append("filenew = \"output.txt\"\r\n");
            python.Append(string.Format("df.to_csv(filenew, sep='{0}', decimal=',', na_rep='', header=True, index=False, encoding='utf-8')\r\n", separator));

            // create new file
            notepad.FileNew();
            editor.SetText(python.ToString());
            if (python.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_PYTHON);
            }
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
            var comment = "";
            if (csvdef.Separator == '\0') comment += string.Format("\r\n; Fixed Length positions {0}\r\n", csvdef.GetColumnWidths(true));

            // check for non-supported file features
            var notsup = false;
            if ((csvdef.SkipLines > 0) || (csvdef.CommentChar != '\0')) notsup = true;
            // check for non-supported column features
            foreach (var col in csvdef.Fields) {
                if ( (col.isCodedValue)
                        || ((col.DataType == ColumnType.Decimal) && (col.Decimals != csvdef.NumberDigits))
                        || ((col.DataType == ColumnType.DateTime) && (col.Mask != csvdef.DateTimeFormat))
                    ) notsup = true;
            }
            if (notsup) comment += "\r\n; NOTE: some CSV Lint features are not supported by the ODBC Text driver, that is why these lines are commented out";

            // also add filename
            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());
            var txt = string.Format("[{0}]\r\n{1}{2}", FILE_NAME, csvdef.GetIniLines().ToString(), comment);

            // create new file
            notepad.FileNew();
            editor.SetText(txt);
            if (txt.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_INI);
            }
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

            // add standard disclaimer for generated scripts
            ScriptDisclaimer(rscript);

            // start R-script
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
            var col_enums = "";

            var exampleDate = "myDateField";

            var r_dec = "";

            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // R-script safe tag, replace .
                var colname = coldef.Name;
                colname = Regex.Replace(colname, "[^a-zA-Z0-9_]", "."); // not letter or digit

                var comma = (c < csvdef.Fields.Count - 1 ? "," : ")");

                var indent = (c > 0 ? "              " : "");

                col_names += string.Format("{0}\"{1}\"{2}\r\n", indent, colname, comma);

                // indent for next lines
                if (c > 0) col_types += "              ";

                // enumeration
                if (coldef.isCodedValue)
                {
                    var enumvals = string.Join("\", \"", coldef.CodedList);
                    // Constrains for string or integer values
                    if (coldef.DataType == ColumnType.String)
                    {
                        enumvals = string.Format("\"{0}\"", enumvals); // use quotes
                    }
                    else
                    {
                        enumvals = enumvals.Replace("\"", ""); // no quotes
                    };
                    col_enums += string.Format("  \"{0}\" = c({1}),\r\n", coldef.Name, enumvals);
                }

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

            // R-script skip comment lines
            if (csvdef.SkipLines > 0) nameparam += string.Format("skip={0}, ", csvdef.SkipLines);

            // R-script comment character
            if (csvdef.CommentChar != '\0') nameparam += string.Format("comment.char=\"{0}\", ", csvdef.CommentChar);

            // column types
            rscript.Append(string.Format("colTypes <- {0}\r\n", col_types));

            // read csv file
            var separator = (csvdef.Separator == '\0' ? "{fixed-width}" : csvdef.Separator.ToString());
            if (separator == "\t") separator = "\\t";
            var header = (csvdef.ColNameHeader ? "TRUE" : "FALSE");

            if (csvdef.Separator == '\0')
            {
                // fixed width
                rscript.Append(string.Format("# fixed width, positions {0}\r\n", csvdef.GetColumnWidths(true)));
                rscript.Append(string.Format("colWidths <- c({0})\r\n", csvdef.GetColumnWidths(false)));
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

            // R-script enumeration check
            if (col_enums != "")
            {
                col_enums = col_enums.Remove(col_enums.Length - 3); // remove last comma + CrLf ",\r\n"
                rscript.Append(string.Format("# enumeration allowed values\r\nallowed_values <- list(\r\n{0}\r\n)\r\n\r\n", col_enums));

                // The following R code was generated using ChatGPT based on the Python code
                // If anyone can refactor it to something more readable or more sensible code,
                // please let me know or submit as a pull request
                rscript.Append("# check enumeration\r\ndf_invalid <- lapply(names(allowed_values), function(column_name) {\r\n  df[[column_name]] <- as.character(df[[column_name]])  # Convert values to strings\r\n  invalid_values <- df[!df[[column_name]] %in% as.character(allowed_values[[column_name]]), column_name]\r\n  invalid_counts <- table(invalid_values)\r\n  data.frame(Column_name = column_name, Invalid_value = names(invalid_counts), Count = as.numeric(invalid_counts), stringsAsFactors = FALSE)\r\n})\r\n");
                rscript.Append("df_chk <- bind_rows(df_invalid)\r\nif (nrow(df_chk) > 0) {\r\n  cat(\"Invalid values found:\\n\")\r\n  print(df_chk)\r\n}\r\n\r\n");
            }

            // R-script examples of typical data transformations
            var exampleYear = DateTime.Now.Year;
            rscript.Append("# Remove or uncomment the script parts below to filter, transform, merge as needed\r\n\r\n");

            // -------------------------------------
            ScriptHeader(rscript, "Filter suggestions");
            // -------------------------------------

            rscript.Append("# filter on value or date range\r\n");
            rscript.Append("#filtered_df <- df[df$study == \"123\"), ]\r\n");
            rscript.Append(string.Format("#filtered_df <- df[df${0} >= as.Date(\"{1}-01-01\") & df${0} < as.Date(\"{1}-07-01\"), ]\r\n\r\n", exampleDate, exampleYear));

            rscript.Append("# Reorder or remove columns (edit code below)\r\n");
            rscript.Append(string.Format("colOrder <- {0}", col_names));
            rscript.Append("df <- df[, colOrder]\r\n\r\n");

            // -------------------------------------
            ScriptHeader(rscript, "Data transformation suggestions");
            // -------------------------------------

            rscript.Append("# Date to string example, format as MM/dd/yyyy\r\n");
            rscript.Append(string.Format("#df${0} <- format(df${0}, \"%m/%d/%Y\")\r\n\r\n", exampleDate));

            rscript.Append("# Replace labels with codes example, when column contains 'Yes' or 'No' replace with '1' or '0'\r\n");
            rscript.Append("#lookuplist <- data.frame(\"code\" = c(\"0\", \"1\"),\r\n");
            rscript.Append("#                    \"label\" = c(\"No\", \"Yes\") )\r\n");
            rscript.Append("#df$yesno_int <- lookuplist$code[match(df$yesno_str, lookuplist$label)]\r\n\r\n");
            
            rscript.Append("# Calculate new values example\r\n");
            rscript.Append("#df$bmi_calc <- df$weight / (df$height / 100) ^ 2\r\n");
            rscript.Append("#df$center_patient <- paste(substr(df$centercode, 1, 2), '-', df$patientcode) # '01-123' etc.\r\n\r\n");

            // -------------------------------------
            ScriptHeader(rscript, "Merge examples");
            // -------------------------------------

            rscript.Append("# Merge dataframes example, all.x=TRUE meaning take all df1 records(=x) and left outer join with df2(=y)\r\n");
            rscript.Append("#merged_df <- merge(df1, df2, all.x=TRUE, by=c('patient_id')) # same key column name\r\n");
            rscript.Append("#merged_df <- merge(df1, df2, all.x=TRUE, by.x=c('df1 key'), by.y=c('df2 id')) # different key column name\r\n\r\n");

            rscript.Append("# csv write new output\r\n");
            rscript.Append("filenew = \"output.txt\"\r\n");
            rscript.Append("write.table(df, file=filenew, sep=\";\", dec=\",\", na=\"\", row.names=FALSE)\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(rscript.ToString());
            if (rscript.Length < Main.Settings.AutoSyntaxLimit) {
                notepad.SetCurrentLanguage(LangType.L_R);
            }
        }

        /// <summary>
        /// generate PowerShell code based on columns (sometimes asked on stackoverflow)
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void GeneratePowerShell(CsvDefinition csvdef)
        {
            // get access to Notepad++
            INotepadPPGateway notepad = new NotepadPPGateway();
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // Python requires forward slash for filepaths
            string FILE_PATH = Path.GetDirectoryName(notepad.GetCurrentFilePath()).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string FILE_NAME = Path.GetFileName(notepad.GetCurrentFilePath());

            StringBuilder ps1 = new StringBuilder();

            // build Python script
            ps1.Append("# PowerShell - read csv with datatypes\r\n");

            // default comment
            List<String> comment = CsvEdit.ScriptInfo(notepad);
            foreach (var str in comment) ps1.Append(string.Format("# {0}\r\n", str));

            // add standard disclaimer for generated scripts
            ScriptDisclaimer(ps1);

            // start PowerShell script
            ps1.Append("# working directory and filename\r\n");
            ps1.Append(string.Format("$pathname = \"{0}\"\r\n", FILE_PATH));
            ps1.Append(string.Format("$filename = $pathname + \"{0}\"\r\n\r\n", FILE_NAME));

            var col_names = "";
            var col_fixed = "";
            var col_fixed_write1 = "";
            var col_fixed_write2 = "";
            var col_order = "";
            var col_types = "";
            var col_enums = "";
            var check_enums = "";

            var exampleDate = "";

            var r_dec = "";
            var startpos = 0;

            // get max column name length, for aligning columns
            var MAX_COLNAME = 1;
            for (int c = 0; c < csvdef.Fields.Count; c++) { if (csvdef.Fields[c].Name.Length > MAX_COLNAME) MAX_COLNAME = csvdef.Fields[c].Name.Length; };

            // process all columns
            for (int c = 0; c < csvdef.Fields.Count; c++)
            {
                // next field
                var coldef = csvdef.Fields[c];

                // any characters are allowed in Python column names
                var colname = coldef.Name;
                var colname_fix = Regex.Replace(colname, "[^a-zA-Z0-9]", "_"); // not letter or digit
                if (colname != colname_fix) colname = string.Format("\"{0}\"", colname); // if columns name contains spaces

                var colnamepad = colname.PadRight(MAX_COLNAME, ' ');

                var comma = (c < csvdef.Fields.Count - 1 ? ", " : "");

                // list all column names
                col_names += string.Format("\"{0}\"{1}", coldef.Name, comma);
                var datemask = (coldef.DataType == ColumnType.DateTime ? string.Format(".ToString(\"{0}\")", coldef.Mask) : ""); 
                col_order += string.Format("\t\t{0} = $_.{1}{2}\r\n", colnamepad, colname, datemask);

                // enumeration
                if (coldef.isCodedValue)
                {
                    var enumvals = string.Join("\", \"", coldef.CodedList);
                    // Constrains for string or integer values
                    if (coldef.DataType == ColumnType.String)
                    {
                        enumvals = string.Format("\"{0}\"", enumvals); // use quotes
                    }
                    else
                    {
                        enumvals = enumvals.Replace("\"", ""); // no quotes
                    };
                    col_enums += string.Format("${0}_array = @({1})\r\n", colname_fix, enumvals);
                    check_enums += string.Format("\tif ($row.{1} -and !(${0}_array -contains $row.{1})) {{$errmsg += \"Invalid {2} \"\"$($row.{1})\"\" \"}}\r\n", colname_fix, colname, colname.Replace("\"", "\"\""));
                }

                // indent for next lines
                //if (c > 0) col_types += "              ";

                // Python datetypes
                switch (coldef.DataType)
                {
                    case ColumnType.DateTime:
                        // build Python fomat example "M/d/yyyy HH:m:s" -> "%m/%d/%Y %H:%M:%S"
                        var msk = coldef.Mask;
                        //msk = DateMaskStandardToCstr(msk);
                        col_types += string.Format("\t\t$row.{0} = [datetime]::parseexact($row.{1}, '{2}', $null)\r\n", colnamepad, colname, msk);
                        if (exampleDate == "") exampleDate = colname;
                        break;
                    case ColumnType.Integer:
                        col_types += string.Format("\t\t$row.{0} = [int]($row.{1} -replace '{2}', '')\r\n", colnamepad, colname, Main.Settings.NullKeyword);

                        break;
                    case ColumnType.Decimal:
                        var repl = (coldef.DecimalSymbol.ToString() == "." ? "-replace ',', ''" : "-replace '\\.', '' -replace ',', '.'");
                        col_types += string.Format("\t\t$row.{0} = [decimal]($row.{1} {2})\r\n", colnamepad, colname, repl);

                        // just use the first decimal symbol
                        if (r_dec == "") r_dec = coldef.DecimalSymbol.ToString();
                        break;
                    default:
                        col_types += string.Format("\t\t$row.{0} = $row.{1}.Trim(' \"')\r\n", colnamepad, colname);
                        break;
                };

                // fixed width columns
                if (csvdef.Separator == '\0')
                {
                    var strpos = startpos.ToString().PadLeft(3, ' ');
                    var strwid = coldef.MaxWidth.ToString().PadLeft(2, ' ');
                    col_fixed += string.Format("\t\t{0} = $line.Substring({1}, {2}).Trim(' \"')\r\n", colnamepad, strpos, strwid);
                    startpos += coldef.MaxWidth;
                }
                col_fixed_write1 += string.Format("{{{0},{1}{2}}} ", c, (coldef.DataType == ColumnType.Integer || coldef.DataType == ColumnType.Decimal ? "" : "-"), coldef.MaxWidth);
                col_fixed_write2 += string.Format("$row.{0}{1}", colname, comma);
            }

            // no decimals, then not technically needed but nice to have as example code
            if (r_dec == "") r_dec = ".";
            if (exampleDate == "") exampleDate = "myDateField";

            // csv-parameters
            var nameparam = "";
            var separator = csvdef.Separator.ToString();
            if (separator != "\0")
            {
                if (separator == "\t") separator = "`t";
                nameparam += string.Format(" -Delimiter \"{0}\"", separator);
            }

            if (!csvdef.ColNameHeader)
            {
                nameparam += string.Format(" -Header @({0})", col_names);
            }

            // PowerShell comment character not supported(?)
            //if (csvdef.CommentChar != '\0') nameparam += string.Format(" -Comment '{0}'", csvdef.CommentChar);

            // read csv file
            if (csvdef.Separator == '\0')
            {
                // fixed width
                ps1.Append(string.Format("# read fixed width data file, positions {0}\r\n", csvdef.GetColumnWidths(true)));
                ps1.Append("$stream_in = [System.IO.StreamReader]::new($filename)\r\n\r\n");

                if (csvdef.SkipLines > 0)
                {
                    ps1.Append(string.Format("# skip first {0} lines\r\n", csvdef.SkipLines));
                    ps1.Append(string.Format("for ($i=0; $i -lt {0}; $i=$i+1) { $skipline = $stream_in.ReadLine() }\r\n", csvdef.SkipLines));
                }

                if (csvdef.ColNameHeader)
                {
                    ps1.Append("# skip header\r\n");
                    ps1.Append("$skipline = $stream_in.ReadLine()\r\n");
                }

                ps1.Append("# read fixed width data\r\n");
                ps1.Append("$csvdata = while ($line = $stream_in.ReadLine()) {\r\n");
                ps1.Append("\t[PSCustomObject]@{\r\n");
                ps1.Append(col_fixed);
                ps1.Append("\t}\r\n}\r\n$stream_in.Dispose()\r\n\r\n");
            }
            else
            {
                // character separated
                ps1.Append("# read csv data file\r\n");

                // PowerShell skip lines requires different csv function
                if (csvdef.SkipLines > 0) {
                    ps1.Append(string.Format("$csvdata = Get-Content -Path $filename | Select-Object -Skip {0} | ConvertFrom-Csv{1}\r\n\r\n", csvdef.SkipLines, nameparam));
                } else {
                    ps1.Append(string.Format("$csvdata = Import-Csv -Path $filename{0}\r\n\r\n", nameparam));
                }
            }

            // column types
            if (col_types != "")
            {
                ps1.Append("# Explicit datatypes\r\n");
                ps1.Append("# WARNING: PowerShell has very basic error handling for null or invalid values,\r\n");
                ps1.Append("# so if your data file contains integer, decimal or datetime columns with empty or incorrect values,\r\n");
                ps1.Append("# this script can throw errors, silently change values to '0' or omit rows in the output csv, so beware.\r\n");
                ps1.Append("$line = 0\r\n");
                ps1.Append("foreach ($row in $csvdata)\r\n{\r\n");
                ps1.Append("\t$line += 1\r\n");
                ps1.Append("\ttry {\r\n");
                ps1.Append(col_types);
                ps1.Append("\t} catch {\r\n");
                ps1.Append("\t\tWrite-Error \"Data conversion error(s) on line $line\" -TargetObject $row\r\n");
                ps1.Append("\t}\r\n");
                ps1.Append("}\r\n\r\n");
            }

            // PowerShell enumeration check
            if (col_enums != "")
            {
                ps1.Append("# Enumeration allowed values\r\n");
                ps1.Append(string.Format("{0}\r\n", col_enums));
                ps1.Append("# enumeration check invalid values\r\n");
                ps1.Append("$line = 0\r\n");
                ps1.Append("foreach ($row in $csvdata)\r\n{\r\n");
                ps1.Append("\t# check invalid values\r\n");
                ps1.Append("\t$errmsg = \"\"\r\n");
                ps1.Append(string.Format("{0}\r\n", check_enums));
                ps1.Append("\t# report invalid values\r\n");
                ps1.Append("\t$line = $line + 1\r\n");
                ps1.Append("\tif ($errmsg) {Write-Error \"$errmsg on line $line\" -TargetObject $row}\r\n}\r\n\r\n");
            }

            // PowerShell examples of typical data transformations
            var exampleYear = DateTime.Now.Year;
            ps1.Append("# Remove or uncomment the script parts below to filter, transform, merge as needed\r\n\r\n");
            // -------------------------------------
            ScriptHeader(ps1, "Data filter suggestions");
            // -------------------------------------
            ps1.Append("# filter on value or date range\r\n");
            ps1.Append(string.Format("#$csvdata = $csvdata | Where-Object {{ $_.{0} -gt [DateTime]::Parse(\"{1}-01-01\") -and $_.{0} -lt [DateTime]::Parse(\"{1}-07-01\") }}\r\n", exampleDate, exampleYear));

            ps1.Append("# Reorder or remove columns (edit code below)\r\n");
            ps1.Append("$csvnew = $csvdata | ForEach-Object {\r\n");
            ps1.Append("\t[PSCustomObject]@{\r\n");
            ps1.Append("\t\t# Reorder columns\r\n");
            ps1.Append(col_order);

            // -------------------------------------
            // ScriptHeader(ps1, "Data transformation suggestions");
            // -------------------------------------

            ps1.Append("#\t\t# Data transformation suggestions\r\n");
            ps1.Append(string.Format("#\t\t{0} = $_.{1}.ToString(\"yyyy-MM-dd\")\r\n", exampleDate.PadRight(MAX_COLNAME, ' '), exampleDate));
            ps1.Append(string.Format("#\t\t{0} = switch ($_.YesNo_str) {{\r\n", "YesNo_int".PadRight(MAX_COLNAME, ' ')));
            ps1.Append("#\t\t\t\t\"No\" {\"0\"}\r\n");
            ps1.Append("#\t\t\t\t\"Yes\" {\"1\"}\r\n");
            ps1.Append("#\t\t\t\tdefault {$_}\r\n");
            ps1.Append("#\t\t\t}\r\n");
            ps1.Append(string.Format("#\t\t{0} = [math]::Round($_.Weight / ($_.Height * $_.Height), 2)\r\n", "bmi".PadRight(MAX_COLNAME, ' ')));
            ps1.Append(string.Format("#\t\t{0} = $_.centercode.SubString(0, 2) + \"-\" + patientcode # '01-123' etc\r\n", "cent_pat".PadRight(MAX_COLNAME, ' ')));
            ps1.Append("\t}\r\n");
            ps1.Append("}\r\n\r\n");

            // -------------------------------------
            ScriptHeader(ps1, "Merge data example");
            // -------------------------------------

            ps1.Append("## Merge datasets in PowerShell requires custom external modules which goes beyond the scope of this generated script\r\n");
            ps1.Append("##Install-Module -Name Join-Object\r\n");
            ps1.Append("##$merged_df = Join-Object -Left $patients -Right $visits -LeftJoinProperty 'PATIENT_ID' -RightJoinProperty 'PATIENT_ID' -ExcludeRightProperties 'Junk' -Prefix 'R_' | Format-Table\r\n\r\n");

            ps1.Append("# csv write new output\r\n");
            ps1.Append("$filenew = $pathname + \"output.txt\"\r\n");
            ps1.Append(string.Format("$csvnew | Export-Csv -Path $filenew -Encoding utf8 -Delimiter \"`t\" -NoTypeInformation\r\n\r\n", separator));

            ps1.Append("# alternatively, write as fixed width\r\n");
            ps1.Append("#$stream_out = New-Object System.IO.StreamWriter $filenew\r\n");
            ps1.Append("#foreach ($row in $csvnew)\r\n");
            ps1.Append("#{\r\n");
            ps1.Append("#\t# {colnr,width} space etc, negative width means left aligned\r\n");
            ps1.Append(string.Format("#\t$stream_out.WriteLine((\"{0}\" -f {1}))\r\n", col_fixed_write1.Trim(), col_fixed_write2));
            ps1.Append("#}\r\n");
            ps1.Append("#$stream_out.Dispose()\r\n");

            // create new file
            notepad.FileNew();
            editor.SetText(ps1.ToString());
            if (ps1.Length < Main.Settings.AutoSyntaxLimit)
            {
                notepad.SetCurrentLanguage(LangType.L_POWERSHELL);
            }
        }
    }
}
