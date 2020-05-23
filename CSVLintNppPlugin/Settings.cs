using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CsvQuery.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Manages application settings
    /// </summary>
    public class Settings : SettingsBase
    {

        [Description("Separators that are detected automatically."), Category("General"), DefaultValue(",;|\\t")]
        public string Separators { get; set; } = ",;|\\t:";

        [Description("Keyword for null values."), Category("General"), DefaultValue("NULL")]
        public String NullValue { get; set; }

        [Description("Trim values before analyzing or editing"), Category("General"), DefaultValue(true)]
        public bool TrimValues { get; set; }

        [Description("Default quote escape character when quotes exists inside text"), Category("General"), DefaultValue('\"')]
        public char DefaultQuoteChar { get; set; }

        [Description("Maximum rows to analyze to automatically detect data types. Set to 0 to analyze all rows, set to 1000 for better performance with large files."), Category("Analyze"), DefaultValue(0)]
        public int ScanRows { get; set; }

        [Description("Year minimum when detecting date or datetime values."), Category("Analyze"), DefaultValue(1900)]
        public int YearMinimum { get; set; }

        [Description("Year maximum when detecting date or datetime values."), Category("Analyze"), DefaultValue(2100)]
        public int YearMaximum { get; set; }

        [Description("Maximum length of an integer before it's considered a string instead"), Category("General"), DefaultValue(10)]
        public int IntegerDigitsMax { get; set; }

        [Description("Maximum errors output, limit errors logging."), Category("Validate"), DefaultValue(100)]
        public int MaxErrors { get; set; }

        [Description("Pivot year for two digit year date values, for example set to 2025 and date values with year 18, 25, 26, 48 will become year 2018, 2025, 1926, 1948. Set as SysYear+5 for current year plus 5 or set to 99 to disable."), Category("Edit"), DefaultValue(2020)]
        public int TwoDigitYearMax { get; set; }

        [Description("Create new file when making edits."), Category("Edit"), DefaultValue(true)]
        public bool CreateNewFile { get; set; }

        [Description("Decimal values remove leading zero, for example change 0.12 to .12"), Category("Edit"), DefaultValue(true)]
        public bool DecimalLeadingZero { get; set; }
    }
}
