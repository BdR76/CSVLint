using System;
using System.ComponentModel;
using CsvQuery.PluginInfrastructure;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Manages application settings
    /// </summary>
    public class Settings : SettingsBase
    {
        [Description("Comment character, when the first lines start with this character they will be skipped as comment lines"), Category("Analyze"), DefaultValue('#')]
        public char CommentCharacter { get; set; }

        [Description("Maximum unique values when reporting or detecting coded values, if column contains more than it's not reported."),
            Category("Analyze"), DefaultValue(15)]
        public int UniqueValuesMax { get; set; }

        //[Description("Maximum rows to analyze to automatically detect data types. Set to 0 to analyze all rows, set to 1000 for better performance with large files."),
        //    Category("Analyze"), DefaultValue(0)]
        //public int ScanRows { get; set; }

        [Description("Maximum amount of digits for integer values, if a value has more then it's considered a text value. Applies to both autodetecting datatypes and validating data. Useful to distinguish (bar)codes and actual numeric values."),
            Category("Analyze"), DefaultValue(12)]
        public int IntegerDigitsMax { get; set; }

        [Description("Maximum amount of decimals for decimal values, if a value has more then it's considered a text value. Applies to both autodetecting datatypes and validating data."),
            Category("Analyze"), DefaultValue(20)]
        public int DecimalDigitsMax { get; set; }

        [Description("Decimal values must have leading zero, set to false to accept values like .5 or .01"),
            Category("Analyze"), DefaultValue(true)]
        public bool DecimalLeadingZero { get; set; }

        private int _ErrorTolerance;
        public float _ErrorTolerancePerc;

        [Description("Error tolerance percentage, when analyzing allow X % errors. For example when a column with a 1000 values contains all integers except for 9 or fewer non-integer values, then it's still interpreted as an integer column."), Category("Analyze"), DefaultValue(1)]
        public int ErrorTolerance
        {
            get
            {
                return this._ErrorTolerance;
            }
            set
            {
                this._ErrorTolerance = value;
                this._ErrorTolerancePerc = (float)0.01 * value;
            }
        }

        [Description("When detecting or validating date or datetime values, years smaller than this value will be considered as out-of-range."),
            Category("Analyze"), DefaultValue(1900)]
        public int YearMinimum { get; set; }

        [Description("When detecting or validating date or datetime values, years larger than this value will be considered as out-of-range."),
            Category("Analyze"), DefaultValue(2050)]
        public int YearMaximum { get; set; }

        [Description("Maximum year for two digit year date values. For example, when set to 2030 the year values 30 and 31 will be interpreted as 2030 and 1931. Set as CurrentYear for current year."),
            Category("Edit"), DefaultValue("CurrentYear")]
        public string TwoDigitYearMax
        {
            get
            {
                return this._strTwoDigitYearMax;
            }
            set
            {
                // set string representation, may include "CurrentYear"
                this._strTwoDigitYearMax = this.CheckYearString(value);
                // set actual integer value
                this.intTwoDigitYearMax = this.GetYearFromString(value);
            }
        }

        // actual year value as int
        public int intTwoDigitYearMax;
        private string _strTwoDigitYearMax;

        //[Description("Decimal values remove leading zero, for example output 0.5 as .5"), Category("Edit"), DefaultValue(false)]
        //public bool DecimalLeadingZeroOut { get; set; }

        [Description("Trim values when editing, sorting or analyzing data. Recommeneded, because when disabled the column datatypes will not always be detected correctly."),
            Category("Edit"), DefaultValue(true)]
        public bool TrimValues { get; set; }

        [Description("Reformat dataset, apply quotes option. 0 = None minimal, 1..3, 4 = Always."), Category("Edit"), DefaultValue(0)] // TODO change to proper dropdownlist (0=None minial, 1=Values with spaces, 2=All string values, 3=All non-numeric values, 4=All values)
        public int ReformatQuotes { get; set; }

        [Description("Convert data, automatically apply syntax highlighting to resulting file, only when it's smaller than this size. Prevent Notepad++ from freezing on large files."), Category("General"), DefaultValue(1024*1024)]
        public int AutoSyntaxLimit { get; set; }

        [Description("Default quote character, typically double quote \" or single quote '"), Category("General"), DefaultValue('"')]
        public char DefaultQuoteChar { get; set; }

        //[Description("Default font for text boxes in CSV Lint docking window. Changing this requires closing and opening the CSV docked window."), Category("General"), DefaultValue("Courier, 11.25pt")]
        //public string Font { get; set; }

        private const string FontDockDefault = "Courier New, 11.25pt";

        [Description("Default font for text boxes in CSV Lint docking window. Changing the font requires closing and opening the CSV docked window."), Category("General"), DefaultValue(typeof(Font), FontDockDefault)]
        public Font FontDock { get; set; }

        [Description("A case-sensitive keyword that will be treated as an empty value, typically NULL, NaN, NA or None depending on your data."),
            Category("General"), DefaultValue("NaN")]
        public string NullKeyword { get; set; }

        [Description("Include separator in syntax highlighting colors. Set to false and the separator characters are not colored."),
            Category("General"), DefaultValue(false)]
        public bool SeparatorColor { get; set; }

        [Description("Preferred characters when automatically detecting the separator character. For special characters like tab, use \\t or hexadecimal escape sequence \\u0009 or \\x09."),
            Category("General"), DefaultValue(",;\\t|")]
        public string Separators
        {
            get
            {
                return this._strSeparators;
            }
            set
            {
                // set string representation of separator characters
                this._strSeparators = "";
                if (value.Trim() == "") value = ",;\\t|"; // default

                // convert control characters to escaped hex format, example DC4 -> "\x14", ESC -> "\x1b" etc.
                foreach (var ch in value) {
                    if (ch < 32) {
                        this._strSeparators += string.Format("\\x{0:x2}", (ushort)ch); // control characters
                    } else {
                        this._strSeparators += ch;
                    };
                }

                // clear characters list
                this._charSeparators = "";

                // set actual character values
                // replace escaped special characters, example "\t" "\x1b" etc.
                Regex ItemRegex = new Regex(@"(\\u[0-9a-fA-F]{4}|\\x[0-9a-fA-F]{2}|\\[trnae]|.)", RegexOptions.Compiled);
                foreach (Match ItemMatch in ItemRegex.Matches(this._strSeparators))
                {
                    char c = '\0';
                    if (ItemMatch.Length > 1)
                    {
                        string ltr = ItemMatch.ToString().Substring(1, 1);

                        // check for default characters
                        if (ltr == "t") c = '\t'; // tab
                        if (ltr == "r") c = '\r'; // carriage return
                        if (ltr == "n") c = '\n'; // line feed
                        if (ltr == "a") c = '\a'; // bell
                        if (ltr == "e") c = '\u0027'; // escape

                        // check for u0009 or x09 etc.
                        if ((ltr == "u") || (ltr == "x") )
                        {
                            string hexnr = ItemMatch.ToString().Substring(2, ItemMatch.Length - 2);
                            int number = Convert.ToInt32(hexnr, 16);
                            c = Convert.ToChar(number);
                        }
                    }
                    else
                    {
                        c = ItemMatch.ToString()[0];
                    }
                    // add to list
                    if (c != '\0')
                    {
                        this._charSeparators += c;
                    }
                }
            }
        }
        // actual year value as int
        public string _charSeparators;
        private string _strSeparators;

        [Description("Transparent cursor line, changing this setting will require a restart of Notepad++."), Category("General"), DefaultValue(true)]
        public bool TransparentCursor { get; set; }

        //[Description("Maximum errors output, limit errors logging, or 0 for no limit."),
        //    Category("Validate"), DefaultValue(0)]
        //public int MaxErrors { get; set; }

        //[Description("Month abbreviations for detecting or generating date format 'mmmm', comma separated list of 12 names."),
        //    Category("Validate"), DefaultValue("jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec")]
        //public String MonthAbbrev { get; set; }

        [Description("various dialogs, save last used settings"), Category("UserDialogs"), Browsable(false), DefaultValue(true)]
        public bool AutoDetectColumns { get; set; }
		
        // CONVERT DATA user preferences
        private int _sqlbatch;

        [Description("Convert data dialog"), Category("UserDialogs"), Browsable(false), DefaultValue(1000)]
        public int DataConvertBatch
        {
            get { return _sqlbatch; }
            set
            {
                _sqlbatch = Math.Max(value, 10);
            }
        }

        [Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string DataConvertName { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(0)]
        public int DataConvertSQL { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(0)]
        public int DataConvertType { get; set; }
		
        // DETECT COLUMNS MANUALLY user preferences
        [Description("Detect columns manually"), Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool DetectColumnHeader { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(",")]
        public string DetectColumnSep { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string DetectColumnWidths { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool DetectCommentChar { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool DetectSkipLines { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(0)]
        public int DetectSkipLinesCount { get; set; }
		
        // ADD NEW COLUMNS (1) user preferences
        [Description("Add new columns (1)"), Category("UserDialogs"), Browsable(false), DefaultValue("0")]
        public string EditColPad { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(10)]
        public int EditColPadLength { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("xyz")]
        public string EditColReplace { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("abc")]
        public string EditColSearch { get; set; }
		
        // GENERATE METADATA OR SCRIPT user preferences
        [Description("Generate metadata or script"), Category("UserDialogs"), Browsable(false), DefaultValue(0)]
        public int MetadataType { get; set; }

        // REFORMAT DATA user preferences
        [Description("Reformat data"), Category("UserDialogs"), Browsable(false), DefaultValue(";")]
        public string ReformatColSep { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("yyyy-MM-dd")]
        public string ReformatDateFormat { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(".")]
        public string ReformatDecSep { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string ReformatOptions { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("<br>")]
        public string ReformatReplaceCrLf { get; set; }
		
        // SORT DATA user preferences
        [Description("Sort data"), Category("UserDialogs"), Browsable(false), DefaultValue(true)]
        public bool SortAscending { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string SortColName { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(true)]
        public bool SortValue { get; set; }

        // ADD NEW COLUMNS (2) user preferences
        [Description("Add new columns (2)"), Category("UserDialogs"), Browsable(false), DefaultValue("/")]
        public string SplitChar { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(1)]
        public int SplitCharNth { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string SplitColName { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(0)]
        public int SplitOption { get; set; }


        [Category("UserDialogs"), Browsable(false), DefaultValue(3)]
        public int SplitPos { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool SplitRemoveOrg { get; set; }

        // SELET COLUMNS user preferences
        [Description("Select columns"), Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string SelectCols { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(true)]
        public bool SelectColsNewfile { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool SelectColsDistinct { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(true)]
        public bool SelectColsSort { get; set; }

        // COUNT UNIQUE VALUES user preferences
        [Description("Count unique values"), Category("UserDialogs"), Browsable(false), DefaultValue("")]
        public string UniqueColumns { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(true)]
        public bool UniqueSortBy { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool UniqueSortAsc { get; set; }

        [Category("UserDialogs"), Browsable(false), DefaultValue(false)]
        public bool UniqueSortValue { get; set; }

        // helper function for "CurrentYear" as year values
        private int GetYearFromString(string yr)
        {
            // test if it's a valid int
            int.TryParse(yr, out int ret);

            // check if valid year number, note "CurrentYear" results in ret=0
            if ((ret <= 0) || (ret >= 9999))
            {
                ret = DateTime.Now.Year;
            };

            return ret;
        }

        private string CheckYearString(string yr)
        {
            string ret = yr.Trim();

            // test if it's a valid int
            int.TryParse(ret, out int test);

            // check if valid year number, note "CurrentYear" results in test=0
            if ((test <= 0) || (test >= 9999))
            {
                ret = "CurrentYear"; // default value
            };
        
            return ret;
        }
    }
}
