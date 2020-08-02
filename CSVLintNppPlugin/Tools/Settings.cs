using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CsvQuery.PluginInfrastructure;
using System.Text.RegularExpressions;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Manages application settings
    /// </summary>
    public class Settings : SettingsBase
    {

        [Description("Preferred characters when automatically detecting the separator character. For special characters like tab, use \\t or \\u0009."), Category("General"), DefaultValue(",;\\t|")]
        public String Separators
        {
            get
            {
                return this._strSeparators;
            }
            set
            {
                // set string representation, may include "SysYear"
                this._strSeparators = value;
                if (this._strSeparators.Trim() == "") this._strSeparators = ",;\\t|"; // default

                // clear characters list
                this._charSeparators = "";

                // set actual character values
                // replace escaped special characters, example "\t" "\x1b" etc.
                Regex ItemRegex = new Regex(@"(\\u[0-9a-fA-F]{4}|\\[trnae]|.)", RegexOptions.Compiled);
                foreach (Match ItemMatch in ItemRegex.Matches(this._strSeparators))
                {
                    char c = '\0';
                    if (ItemMatch.Length > 1)
                    {
                        String ltr = ItemMatch.ToString().Substring(1, 1);
                        c = '\0';

                        // check for default characters
                        if (ltr == "t") c = '\t'; // tab
                        if (ltr == "r") c = '\r'; // carriage return
                        if (ltr == "n") c = '\n'; // life feed
                        if (ltr == "a") c = '\a'; // bell
                        if (ltr == "e") c = '\u0027'; // escape

                        // check for x09 etc.
                        if (ltr == "u")
                        {
                            String hexnr = ItemMatch.ToString().Substring(2, ItemMatch.Length - 2);
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
        public String _charSeparators;
        private String _strSeparators;

        [Description("Keyword for null values, case-sensitive."), Category("General"), DefaultValue("NULL")]
        public String NullValue { get; set; }

        [Description("Trim values before analyzing or editing (recommended)."), Category("General"), DefaultValue(true)]
        public bool TrimValues { get; set; }

        [Description("Default quote escape character when quotes exists inside text"), Category("General"), DefaultValue('\"')]
        public char DefaultQuoteChar { get; set; }

        [Description("Maximum rows to analyze to automatically detect data types. Set to 0 to analyze all rows, set to 1000 for better performance with large files."), Category("Analyze"), DefaultValue(0)]
        public int ScanRows { get; set; }

        [Description("When detecting date or datetime values, years smaller than this value will be considered as invalid dates."), Category("Analyze"), DefaultValue(1900)]
        public int YearMinimum { get; set; }

        [Description("When detecting date or datetime values, years larger than this value will be considered as invalid dates."), Category("Analyze"), DefaultValue(2050)]
        public int YearMaximum { get; set; }

        [Description("Maximum length of an integer before it's considered a string instead"), Category("General"), DefaultValue(10)]
        public int IntegerDigitsMax { get; set; }

        [Description("Maximum errors output, limit errors logging, or 0 for no limit."), Category("Validate"), DefaultValue(0)]
        public int MaxErrors { get; set; }

        [Description("Pivot year for two digit year date values. For example, when set to 2025 the year values 24 and 26 will be interpreted as year 2024 and 1926 respectively. Set as SysYear+5 for current year plus 5."), Category("Edit"), DefaultValue("SysYear")]
        public String TwoDigitYearMax
        {
            get
            {
                return this._strTwoDigitYearMax;
            }
            set
            {
                // set string representation, may include "SysYear"
                this._strTwoDigitYearMax = this.checkYearString(value);
                // set actual integer value
                this.intTwoDigitYearMax = this.getYearFromString(value);
            }
        }

        // actual year value as int
        public int intTwoDigitYearMax;
        private String _strTwoDigitYearMax;

        [Description("Create new file when making edits."), Category("Edit"), DefaultValue(true)]
        public bool CreateNewFile { get; set; }

        [Description("Decimal values remove leading zero, for example change 0.12 to .12"), Category("Edit"), DefaultValue(true)]
        public bool DecimalLeadingZero { get; set; }

        // helper function for "SysYear" as year values
        private int getYearFromString(String yr)
        {
            // test if it's a valid int
            int ret;
            int.TryParse(yr, out ret);

            // replace SysYear with current year, for example 2020
            if ((ret == 0) || (yr.ToLower() == "sysyear"))
            {
                ret = DateTime.Now.Year;
            };

            return ret;
        }
        private String checkYearString(String yr)
        {
            String ret = yr.Trim();

            // test if it's a valid int
            int test;
            int.TryParse(ret, out test);

            // replace SysYear with current year, for example 2020
            if ((test == 0) || (yr.ToLower() == "sysyear"))
            {
                ret = "SysYear"; // default value
            };
        
            return ret;
        }
    }
}
