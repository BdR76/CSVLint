// -------------------------------------
// CsvDefinition
// Holds file and column definition of csv data
// method to infer definition from text data
// load definition from schema.ini file
// save definition to schema.ini file
// -------------------------------------
using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CSVLint
{
    /// <summary>
    /// CSV Definition state
    /// </summary>
    public enum CsvScanState
    {
        None = 0, // quick scan first 20 lines, file is not of csv type (.csv .ssv .tsv etc) but still determing column separator for when user selects csv syntax
        QuickScan = 1, // quick scan first 20 lines, file is not of csv type (.csv .ssv .tsv etc) but still determing column separator for when user selects csv syntax
        FullScan = 2,  // full file scan, scan entire file for column data types, widths, codes etc
        LoadIni = 3,   // loaded from ini file, presumably all column data types, widths, codes etc is available
        TooBig = 99    // file exceeds 2GB, cannot be scanned
    }

    /// <summary>
    /// Type of data in a column. Higher values can always include lower (i.e. a decimal column can have an integer, but not the other way)
    /// </summary>
    public enum ColumnType
    {
        Unknown = 0,
        Integer = 1,
        Decimal = 2,
        String = 4,
        DateTime = 8
    }

    /// <summary>
    /// Use Windows StrCmpLogicalW function
    /// Compares two Unicode strings and digits in the strings are considered as numerical content rather than text.
    /// 
    /// For example:
    /// Normal  .Sort() -> "fu_12mnd", "fu_1mnd", "fu_24mnd", "fu_3mnd_1", "fu_3mnd_10", "fu_3mnd_5", "fu_6mnd"
    /// Special .Sort() -> "fu_1mnd", "fu_3mnd_1", "fu_3mnd_5", "fu_3mnd_10", "fu_6mnd", "fu_12mnd", "fu_24mnd"
    /// C# example code by Scott Chamberlain https://stackoverflow.com/a/40943999/1745616
    /// </summary>
    public class StrCmpLogicalComparer : Comparer<string>
    {
        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string x, string y);

        public override int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }

    /// <summary>
    /// Column definition
    /// </summary>
    public class CsvColumn
    {
        public int Index;
        public string Name;
        public int MaxWidth;
        public ColumnType DataType;
        public string Mask;
        public char DecimalSymbol = '.';
        public int Decimals = 0;
        public string sTag; // depends on datatype, datetime="" , float=",." or ".,"
        public int iTag;    // depends on datatype, datetime=nr digits year (2 or 4), float=max decimals
        public bool isCodedValue = false;
        public List<string> CodedList;

        public CsvColumn(int idx)
        {
            this.Index = idx;
            this.Name = string.Format("F{0}", idx);
            this.MaxWidth = 50;
            this.DataType = ColumnType.String;
            this.Mask = "";

            this.Initialize();
        }
        public CsvColumn(int idx, string name, int maxwidth, ColumnType datatype, string mask)
        {
            this.Index = idx;
            this.Name = name;
            this.MaxWidth = maxwidth;
            this.DataType = datatype;
            this.Mask = mask;

            this.Initialize();
        }

        public CsvColumn(CsvColumn copyobj)
        {
            this.Index    = copyobj.Index;
            this.Name     = copyobj.Name;
            this.MaxWidth = copyobj.MaxWidth;
            this.DataType = copyobj.DataType;
            this.Mask     = copyobj.Mask;

            this.Initialize();
        }

        public void Initialize()
        {
            this.sTag = "";
            this.iTag = -1;

            if (this.DataType == ColumnType.DateTime)
            {
            }

            if (this.DataType == ColumnType.Decimal)
            {
                // get decimal position or -1 if not found
                int pos1 = Mask.LastIndexOf('.');
                int pos2 = Mask.LastIndexOf(',');
                int p = pos1 > pos2 ? pos1 : pos2;

                // decimal character
                this.DecimalSymbol = pos1 > pos2 ? '.' : ',';

                // sTag, thousand and decimand characters
                this.sTag = pos1 > pos2 ? ",." : ".,";

                // iTag, max decimal places
                this.iTag     = this.Mask.Length - p - 1;
                this.Decimals = this.Mask.Length - p - 1;
            }
        }
        public void UpdateDateTimeMask(string newmask)
        {
            // update both mask and width based on mask
            this.Mask = newmask;
            this.MaxWidth = newmask.Length;

            // For any mask part without leading zeros, add 1 to the max length
            // For example Mask "d-m-yyyy h:nn" is length 13 but valid value "31-12-1999 23:59" is length 16
            string[] tmp = new[] { "dd", "mm", "hh", "DD", "MM", "HH" };
            foreach (var s in tmp)
            {
                if ((newmask.IndexOf(s) < 0) && (newmask.IndexOf(s[0]) >= 0)) this.MaxWidth++;
            }
        }
        public void AddCodedValues(Dictionary<string, int> slcodes)
        {
            // if any contains Carriage Retunr/Line Feed, then it's probably text not codes
            var containsCrLf = false;
            var total = 0;
            foreach (var s in slcodes)
            {
                if (s.Key.Contains('\r') || s.Key.Contains('\n')) containsCrLf = true;
                total += s.Value;
            }

            // check if could be coded values
            if ( (containsCrLf == false) && (slcodes.Count > 0) && (slcodes.Count <= Main.Settings.UniqueValuesMax) )
            {
                // check enumeration ratio, this is to avoid interpreting a column with 100 rows and only 3 text values to be interpreted as enumeration
                var ratio = 1.0 * total / slcodes.Count;

                // in a coded values column each unique value must be used at least 2 times or more (on average)
                if (ratio >= 2.0)
                {
                    // set coded values
                    this.isCodedValue = true;

                    this.CodedList = new List<string>();

                    foreach (var s in slcodes)
                    {
                        this.CodedList.Add(s.Key);
                    }

                    // Sort list, with a hack to sort integers correctly
                    // i.e. list of integers should not be sorted like [1, 10, 11, 2, 3, .. etc]
                    this.CodedList.Sort(new StrCmpLogicalComparer());
                }
            }
        }
    }

    /// <summary>
    /// Csv meta data definition
    /// </summary>
    public class CsvDefinition
    {
        /// <summary>TooBig if the current file has more than <see cref="int.MaxValue"/> bytes</summary>
        public CsvScanState ScanState { get; set; } = CsvScanState.None;

        public int DefaultLanguageId { get; set; } = 0;

        /// column separator character
        public char Separator { get; set; } = '\0';

        /// schema.ini DateTimeFormat for all columns
        public string DateTimeFormat { get; set; } = "";

        /// schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
        public char DecimalSymbol { get; set; } = '\0';

        /// schema.ini NumberDigits, Indicates the number of decimal digits in the fractional portion of a number.
        public int NumberDigits { get; set; } = 0;

        /// schema.ini NumberLeadingZeros, 
        /// Specifies whether a decimal value less than 1 and more than -1 should contain leading zeros; this value can be either False (no leading zeros) or True.
        public bool NumberLeadingZeros { get; set; } = true;

        /// schema.ini CurrencySymbol
        /// Indicates the currency symbol that can be used for currency values in the text file. Examples include the dollar sign ($) and Dm.
        public string CurrencySymbol { get; set; } = "";

        /// schema.ini CurrencyPosFormat
        /// Can be set to any of the following values:
        /// - Currency symbol prefix with no separation($1)
        /// - Currency symbol suffix with no separation(1$)
        /// - Currency symbol prefix with one character separation($ 1)
        /// - Currency symbol suffix with one character separation(1 $)
        public string CurrencyPosFormat { get; set; } = "($1)";

        /// schema.ini CurrencyDigits
        /// Specifies the number of digits used for the fractional part of a currency amount.
        public int CurrencyDigits { get; set; } = 2; // xxx0.99

        /// schema.ini CurrencyNegFormat
        /// Can be one of the following values:
        /// ($1)  -$1  $-1  $1 - (1$)  -1$  1 -$  1$- -1 $  -$ 1  1 $-  $ 1 -  $ -1  1 - $  ($ 1)  (1 $)
        /// This example shows the dollar sign, but you should replace it with the appropriate CurrencySymbol value in the actual program.
        public string CurrencyNegFormat { get; set; } = "-$1";

        /// schema.ini CurrencyThousandSymbol
        /// Indicates the single-character symbol that can be used for separating currency values in the text file by thousands.
        public char CurrencyThousandSymbol { get; set; } = '\0';

        /// schema.ini CurrencyDecimalSymbol
        /// Can be set to any single character that is used to separate the whole from the fractional part of a currency amount.
        public char CurrencyDecimalSymbol { get; set; } = '.';

        /// first line contains column names
        public bool ColNameHeader { get; set; } = true;

        // This will replace TextQualifier below - only " is used anyway
        public bool? UseQuotes { get; set; }
        public char TextQualifier { get; set; } = '"';

        /// SkipLines, skip first X lines of data file (Note: SkipLines is not formally of schema.ini standard)
        public int SkipLines { get; set; } = 0;

        /// Comment character, skip lines if this is the first character
        public char CommentChar { get; set; } = '\0';

        /// column names
        public string[] FieldNames { get; set; }

        /// field widths
        public List<int> FieldWidths { get; set; }

        /// field definitions
        public List<CsvColumn> Fields { get; set; } = new List<CsvColumn>(); // always create

        public CsvDefinition()
        {
        }

        public CsvDefinition(char separator)
        {
            this.Separator = separator;
        }

        public CsvDefinition(CsvDefinition copyobj)
        {
            this.DefaultLanguageId      = copyobj.DefaultLanguageId;
            this.Separator              = copyobj.Separator;
            this.DateTimeFormat         = copyobj.DateTimeFormat;
            this.DecimalSymbol          = copyobj.DecimalSymbol;
            this.NumberDigits           = copyobj.NumberDigits;
            this.NumberLeadingZeros     = copyobj.NumberLeadingZeros;
            this.CurrencySymbol         = copyobj.CurrencySymbol;
            this.CurrencyPosFormat      = copyobj.CurrencyPosFormat;
            this.CurrencyDigits         = copyobj.CurrencyDigits;
            this.CurrencyNegFormat      = copyobj.CurrencyNegFormat;
            this.CurrencyThousandSymbol = copyobj.CurrencyThousandSymbol;
            this.CurrencyDecimalSymbol  = copyobj.CurrencyDecimalSymbol;
            this.ColNameHeader          = copyobj.ColNameHeader;

            this.FieldNames             = copyobj.FieldNames;
            this.UseQuotes              = copyobj.UseQuotes;
            this.TextQualifier          = copyobj.TextQualifier;
            this.SkipLines              = copyobj.SkipLines;
            this.CommentChar            = copyobj.CommentChar;

            this.FieldWidths = new List<int>();
            foreach (var wid in copyobj.FieldWidths)
            {
                this.FieldWidths.Add(wid);
            }

            foreach (var col in copyobj.Fields)
            {
                this.Fields.Add(new CsvColumn(col));
            }
        }

        public void AddColumn(string name = "Col")
        {
            // name is optional
            //if (name == "") name = "Col";
            this.AddColumn(name, 50);
        }
        public void AddColumn(string name, int maxwidth)
        {
            // name is optional
            this.AddColumn(name, maxwidth, ColumnType.String);
        }

        public void AddColumn(string name, int maxwidth, ColumnType datatype)
        {
            this.AddColumn(name, maxwidth, datatype, "");
        }
        public void AddColumn(string name, int maxwidth, ColumnType datatype, string mask)
        {
            int idx = this.Fields.Count + 1;
            this.AddColumn(idx, name, maxwidth, datatype, mask);
        }
        public void AddColumn(int idx, string name, int maxwidth, ColumnType datatype, string mask)
         {
            if (datatype == ColumnType.DateTime)
            {
                // allow different datemask formats per column, but keep track of first datetime format as schema.ini global

                // datetime column MUST have a mask
                if (mask == "") mask = this.DateTimeFormat == "" ? "yyyy-MM-dd" : this.DateTimeFormat;

                // remember first datetime mask as the schema.ini global datetime mask
                if (this.DateTimeFormat == "") this.DateTimeFormat = mask;
            }

            if (datatype == ColumnType.Decimal)
            {
                // get decimal position or -1 if not found
                int pos1 = mask.LastIndexOf('.');
                int pos2 = mask.LastIndexOf(',');

                if ( (pos1 >= 0) || (pos2 >= 0) ) {
                    this.DecimalSymbol = pos1 > pos2 ? '.' : ',';
                    this.NumberDigits = mask.Length - (pos1 > pos2 ? pos1 : pos2) - 1;
                }
            }

            // new column
            CsvColumn col = new CsvColumn(idx, name, maxwidth, datatype, mask);

            this.Fields.Add(col);
        }

        public void RemoveColumn(string name)
        {
            int idx = -1;

            // remove column
            for (int i = 0; i < this.Fields.Count; i++)
            {
                if (this.Fields[i].Name == name) idx = i;
            }
            if (idx != -1) this.RemoveColumn(idx);

            // rebuild indexes
            for (int i = 0; i < this.Fields.Count; i++) this.Fields[i].Index = i;
        }

        public void RemoveColumn(int index)
        {
            this.Fields.RemoveAt(index);
        }

        /// <summary>
        /// Check if fieldname contains a number in parentheses
        /// For example namein = "test123"            will return = "test123"       postfix = -1
        /// For example namein = "SBP (mmHg)"         will return = "SBP (mmHg)"    postfix = -1
        /// For example namein = "labvalue (2)"       will return = "labvalue"      postfix = 2
        /// For example namein = "Copy of field (99)" will return = "Copy of field" postfix = 99
        /// </summary>
        /// <param name="namein">fieldname with or without a number in parentheses</param>
        /// <param name="postfix">outputs the number in parentheses, or -1 if none</param>
        /// <returns></returns>
        public string SplitColumnNamePostfix(string namein, out int postfix)
        {
            string res = namein;
            postfix = -1;

            // check if contains number in parentheses, for example "labvalue (2)"
            var pos1 = namein.LastIndexOf('(');
            var pos2 = namein.LastIndexOf(')');
            if ((pos1 < pos2) && (pos2 == namein.Length - 1))
            {
                var strnr = namein.Substring(pos1 + 1, pos2 - pos1 - 1);
                if (int.TryParse(strnr, out int inr))
                {
                    res = namein.Substring(0, pos1).Trim();
                    postfix = inr;
                }
            }

            return res;
        }

        /// <summary>
        /// Check if fieldname is unique in Fields
        /// returns 0 if already unique
        /// returns 2 or higher as the highest suggested postfix to make the fieldname unique
        /// For example Fields = {"abc", "def"}                              fieldname="xyz"       will return return 0
        /// For example Fields = {"FirstName", "LastName"}                   fieldname="FirstName" will return return 2
        /// For example Fields = {"labvalue (1)", "labvalue(3)", "labvalue"} fieldname="labvalue"  will return return 4
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public string GetUniqueColumnName(string fieldname, out int postfix)
        {
            // check if fieldname already has a postfix
            // example when fieldname="labvalue (3)" then should return "labvalue (4)" if it already exists,
            // so instead of "labvalue (3) (1)"
            var namepart = SplitColumnNamePostfix(fieldname, out postfix);

            // check all current fieldnames if name already exists
            foreach (var col in Fields)
            {
                // check if exact name already exist
                if ((namepart == col.Name) && (postfix == -1)) postfix = 2;

                // check if name with postfix parentheses already exist
                var colname2 = SplitColumnNamePostfix(col.Name, out int inr);
                if ((namepart == colname2) && (inr >= postfix)) postfix = inr + 1;
            }

            return namepart;
        }

        /// <summary>
        /// Create a new csv definition from ini lines in docked CSV Window text box.
        /// </summary>
        /// <param name="inilines">contains inifile sections
        /// <example>"NumberDigits=1\nCol1=participant_id etc."</example></param>
        public CsvDefinition(string inilines)
        {
            // get key values from  ini lines
            var enstr = inilines.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<string> dup = new List<string>();
            foreach (string line in enstr)
            {
                // ignore empty lines and lines without equals sign '='
                var nextline = line.Trim();
                if ( (nextline != "") && (nextline.IndexOf('=') > 0) )
                {
                    string[] parts = nextline.Split(new[]{'='}, 2); // value can contain '=' as well, for example "Format=Separator(=)"
                    if (result.ContainsKey(parts[0]))
                    {
                        dup.Add(parts[0]);
                    }
                    else
                    {
                        result.Add(parts[0], parts[1]);
                    }
                }
            }

            if (dup.Count > 0)
            {
                string errmsg = string.Format("Duplicate key(s) found ({0})", string.Join(",", dup));
                //throw new ArgumentException(errmsg);
                MessageBox.Show(errmsg, "Error in schema.ini", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // create dictionary
                this.CsvDefInitFromKeys(result);
            }
        }

        /// <summary>
        /// Creates a new csv definition from key value pairs.
        /// </summary>
        /// <param name="inikeys">the key values pairs</param>
        public CsvDefinition(Dictionary<string, string> inikeys)
        {
            this.CsvDefInitFromKeys(inikeys);
        }

        private void CsvDefInitFromKeys(Dictionary<string, string> inikeys)
        {
            // TODO: really needed to also keep widths separate from Fields? because each Fields also contains a MaxWidth
            FieldWidths = new List<int>();

            // evaluate key values
            foreach (KeyValuePair<string, string> line in inikeys)
            {
                // section header or comment line
                if (line.Value == null)
                {
                    // ignore for now
                    // TODO: handle header or comment line
                }
                else
                {
                    // schema.ini structure
                    string k = line.Key.ToLower();
                    string val = line.Value.Trim();
                    string vallow = val.ToLower();
                    int.TryParse(val, out int vint);

                    // most important, what is the separator
                    if (k == "format")
                    {
                        // defaults
                        if (vallow == "tabdelimited") this.Separator = '\t';
                        if (vallow == "csvdelimited") this.Separator = ',';
                        if (vallow == "fixedlength") this.Separator = '\0';

                        // custom character
                        if ((vallow.Substring(0, 10) == "delimited(") && (vallow.Substring(vallow.Length - 1, 1) == ")"))
                        {
                            this.Separator = val[10]; // first character after "Delimited("
                        };
                    };

                    // schema.ini DateTimeFormat for all columns
                    if (k == "datetimeformat")
                    {
                        // internally the datetime mask is c# format,         example "dd/MM/yyyy HH:mm"
                        // externally the datetime mask is schema.ini format, example "dd/mm/yyyy hh:nn"
                        // for full date format documentation see https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings?redirectedfrom=MSDN
                        string tmp = val;
                        tmp = tmp.Replace("m", "M");
                        tmp = tmp.Replace("n", "m");
                        tmp = tmp.Replace("h", "H"); // hh=12h, HH=24h
                        this.DateTimeFormat = tmp;
                    }

                    // schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
                    if (k == "decimalsymbol") this.DecimalSymbol = val[0];

                    // schema.ini NumberDigits, Indicates the number of decimal digits in the fractional portion of a number.
                    if (k == "numberdigits") this.NumberDigits = vint;

                    // schema.ini NumberLeadingZeros, 
                    // Specifies whether a decimal value less than 1 and more than -1 should contain leading zeros; this value can be either False (no leading zeros) or True.
                    if (k == "numberleadingzeros") this.NumberLeadingZeros = vallow[1] == 't';

                    // schema.ini CurrencySymbol
                    // Indicates the currency symbol that can be used for currency values in the text file. Examples include the dollar sign ($) and Dm.
                    if (k == "currencysymbol") this.CurrencySymbol = val;

                    // schema.ini CurrencyPosFormat
                    // Can be set to any of the following values:
                    // - Currency symbol prefix with no separation($1)
                    // - Currency symbol suffix with no separation(1$)
                    // - Currency symbol prefix with one character separation($ 1)
                    // - Currency symbol suffix with one character separation(1 $)
                    if (k == "currencyposformat") this.CurrencyPosFormat = val;

                    // schema.ini CurrencyDigits
                    // Specifies the number of digits used for the fractional part of a currency amount.
                    if (k == "currencydigits") this.CurrencyDigits = 2; // TODO Val parseint to int?

                    // schema.ini CurrencyNegFormat
                    // Can be one of the following values:
                    // ($1)  -$1  $-1  $1 - (1$)  -1$  1 -$  1$- -1 $  -$ 1  1 $-  $ 1 -  $ -1  1 - $  ($ 1)  (1 $)
                    // This example shows the dollar sign, but you should replace it with the appropriate CurrencySymbol value in the actual program.
                    if (k == "currencynegformat") this.CurrencyNegFormat = val;

                    // schema.ini CurrencyThousandSymbol
                    // Indicates the single-character symbol that can be used for separating currency values in the text file by thousands.
                    if (k == "currencythousandsymbol") this.CurrencyThousandSymbol = val[0];

                    // schema.ini CurrencyDecimalSymbol
                    // Can be set to any single character that is used to separate the whole from the fractional part of a currency amount.
                    if (k == "currencydecimalsymbol") this.CurrencyDecimalSymbol = val[0];

                    // schema.ini SkipLines
                    // How many lines to skip at start of data file
                    if (k == ";skiplines") this.SkipLines = vint;

                    // schema.ini Comment character
                    // Skip lines if they start with this character
                    if (k == ";commentchar") this.CommentChar = val[0];

                    // schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
                    if (k == "colnameheader")
                    {
                        this.ColNameHeader = vallow[0] == 't';
                    } else
                    // schema.ini DateTimeFormat for all columns
                    if (k.Substring(0, 3) == "col")
                    {
                        string s = k.Substring(3, k.Length - 3);
                        int idx = 0;
                        try
                        {
                            idx = int.Parse(s) - 1;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Unable to parse '{idx}'");
                        }

                        // parse column metadata, for example Val = "Test123 Int Width 3" or Val = "Test123 DateTime Width 10 NOT NULL"
                        // assume default values
                        string name = string.Format("Column{0}", idx);
                        string datatypestr = "";
                        int maxwidth = 50;
                        ColumnType datatype = ColumnType.String;
                        string mask = "";

                        int pos;
                        int spc;

                        // WIDTH must be at end of line
                        spc = vallow.LastIndexOf(" ");
                        pos = vallow.LastIndexOf("width");
                        if (pos == spc - 5)
                        {
                            string width = vallow.Substring(pos, val.Length - pos);
                            val = val.Substring(0, pos).Trim();
                            vallow = val.ToLower();

                            width = width.Replace("width ", "");
                            if (int.TryParse(width, out int n))
                            {
                                maxwidth = n;
                            };
                        }

                        // valid datatype must be at end of line
                        spc = vallow.LastIndexOf(" ");
                        pos = vallow.IndexOf("text", (spc >= 0 ? spc : 0));
                        if (pos == -1) pos = vallow.LastIndexOf("datetime");
                        if (pos == -1) pos = vallow.LastIndexOf("float");
                        if (pos == -1) pos = vallow.LastIndexOf("int");
                        if ((pos >= 0) && (pos == spc + 1))
                        {
                            datatypestr = vallow.Substring(pos, val.Length - pos);
                            val = val.Substring(0, pos).Trim();
                        }
                        // schema.ini datatype, string to ColumnType
                        if (datatypestr == "bit")      datatype = ColumnType.Integer;
                        if (datatypestr == "bit")      datatype = ColumnType.Integer;
                        if (datatypestr == "byte")     datatype = ColumnType.Integer;
                        if (datatypestr == "short")    datatype = ColumnType.Integer;
                        if (datatypestr == "long")     datatype = ColumnType.Integer;
                        if (datatypestr == "currency") datatype = ColumnType.Decimal;
                        if (datatypestr == "single")   datatype = ColumnType.Decimal;
                        if (datatypestr == "double")   datatype = ColumnType.Decimal;
                        if (datatypestr == "datetime") datatype = ColumnType.DateTime;
                        if (datatypestr == "text")     datatype = ColumnType.String;
                        if (datatypestr == "memo")     datatype = ColumnType.String;
                        if (datatypestr == "float")    datatype = ColumnType.Decimal; // Float same as Double
                        if (datatypestr == "integer")  datatype = ColumnType.Integer; // Integer same as Short
                        if (datatypestr == "longchar") datatype = ColumnType.String; // LongChar same as Memo
                        if (datatypestr == "date")     datatype = ColumnType.DateTime;

                        // mask for datetime
                        if (datatype == ColumnType.DateTime)
                        {
                            mask = this.DateTimeFormat;
                        };

                        // mask for decimal numeric
                        if (datatype == ColumnType.Decimal)
                        {
                            int dec = this.NumberDigits;
                            int dig = maxwidth - dec - 1;

                            // data definition error; width shorter than nr of decimals
                            if (dig < 0) dig = 1;

                            mask = string.Format("{0}{1}{2}", new string('9', dig), this.DecimalSymbol, new string('9', dec));
                        };

                        // any left is the name of the column
                        if (val.Trim() != "") name = val;

                        // if quotes around name because of spaces
                        int quote1 = val.IndexOf('"');
                        int quote2 = val.LastIndexOf('"');

                        // check if incorrect and just one quote
                        if (quote1 == quote2)
                        {
                            if (quote1 == 0) quote2 = val.Length; // only quote at start
                            if (quote1 > 0) quote1 = -1;          // only quote at end
                        }

                        // if any quotes around name then remove them
                        if (quote1 > 0 || quote2 > 0) name = val.Substring(quote1 + 1, quote2 - quote1 - 1);

                        // add columns
                        this.AddColumn(idx, name, maxwidth, datatype, mask);
                    };
                    // allow for comments to alter datatype+mask of certain columns
                    if (k.Substring(0, 4) == ";col")
                    {
                        string s = k.Substring(4, k.Length - 4);
                        int idxalt = 0;
                        string datatypestr = "";
                        try
                        {
                            idxalt = int.Parse(s) - 1;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Unable to parse '{idxalt}'");
                        }

                        ColumnType datatypealt = ColumnType.String;

                        var isCoded = false;
                        var CodedValues = "";
                        // check for enumeration metadata
                        int posalt = val.LastIndexOf("Enumeration");
                        if (posalt >= 0)
                        {
                            int spcalt = val.IndexOf(" ", posalt);
                            if (spcalt >= 0) {
                                isCoded = true;
                                CodedValues = val.Substring(spcalt, val.Length - spcalt).Trim();
                            }
                            val = "";
                        }

                        // check for valid datatype must be at end of line
                        posalt = val.LastIndexOf("DateTime");
                        if (posalt >= 0)
                        {
                            int spcalt = val.IndexOf(" ", posalt);
                            datatypestr = val.Substring(posalt, spcalt - posalt);
                            val = val.Substring(spcalt, val.Length - spcalt).Trim();
                        }
                        posalt = val.LastIndexOf("Float");
                        if (posalt >= 0)
                        {
                            int spcalt = val.IndexOf(" ", posalt);
                            datatypestr = val.Substring(posalt, spcalt - posalt);
                            val = val.Substring(spcalt, val.Length - spcalt).Trim();
                        }
                        if (datatypestr == "DateTime") datatypealt = ColumnType.DateTime;
                        if (datatypestr == "Float") datatypealt = ColumnType.Decimal;

                        // additional metadata found, alternative datatype or coded values
                        if ( (datatypealt != ColumnType.String) || (isCoded))
                        {
                            // look for index
                            for (int x = 0; x < this.Fields.Count; x++)
                            {
                                if (this.Fields[x].Index == idxalt)
                                {
                                    if (isCoded)
                                    {
                                        this.Fields[x].isCodedValue = true;
                                        this.Fields[x].CodedList = new List<string>(CodedValues.Split('|'));
                                    }
                                    else
                                    {
                                        this.Fields[x].DataType = datatypealt;
                                        this.Fields[x].Mask = val;
                                        this.Fields[x].Initialize();
                                    }
                                }
                            };
                        }
                    }
                }
            }

            // rebuild widths for fixed width data
            this.FieldWidths.Clear();
            for (var f = 0; f < this.Fields.Count; f++)
            {
                var w = this.Fields[f].MaxWidth;
                this.FieldWidths.Add(w);
            }
        }
        public string GetIniLines()
        {
            string res = "";

            // defaults
            if (this.Separator == '\t') res += "Format=TabDelimited\r\n";
            if (this.Separator == ',') res += "Format=CSVDelimited\r\n";
            if (this.Separator == '\0') res += "Format=FixedLength\r\n";
            // custom character
            if (res == "") res += "Format=Delimited(" + this.Separator + ")\r\n";


            res += "ColNameHeader=" + this.ColNameHeader + "\r\n";

            // schema.ini DateTimeFormat for all columns
            if (this.DateTimeFormat != "")
            {
                // internally the datetime mask is c# format,         example "dd/MM/yyyy HH:mm"
                // externally the datetime mask is schema.ini format, example "dd/mm/yyyy hh:nn"
                string tmp = this.DateTimeFormat;
                tmp = tmp.Replace("m", "n");
                tmp = tmp.Replace("M", "m");
                tmp = tmp.Replace("H", "h");
                res += "DateTimeFormat=" + tmp + "\r\n";
            }

            // schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
            if (this.DecimalSymbol != '\0') res += "DecimalSymbol=" + this.DecimalSymbol + "\r\n";

            // schema.ini NumberDigits, Indicates the number of decimal digits in the fractional portion of a number.
            Dictionary<int, int> DecimalOccurance = new Dictionary<int, int>();
            foreach (var fld in this.Fields)
            {
                if (fld.DataType == ColumnType.Decimal)
                {
                    int dec = fld.Decimals;
                    if (DecimalOccurance.ContainsKey(dec))
                        DecimalOccurance[dec]++;
                    else
                        DecimalOccurance.Add(dec, 1);

                }
            }
            // determine most common nr of factional digits for float/decimal columns
            this.NumberDigits = 0;
            int deccommon = 0;
            foreach (var deckey in DecimalOccurance)
                if (deccommon < deckey.Value)
                {
                    this.NumberDigits = deckey.Key; // how many factional digits, example 1234.56 is two digits
                    deccommon = deckey.Value;       // how many columns have this nr of factional digits
                }

            if (this.NumberDigits > 0) res += "NumberDigits=" + this.NumberDigits + "\r\n";

            // schema.ini NumberLeadingZeros, 
            // Specifies whether a decimal value less than 1 and more than -1 should contain leading zeros; this value can be either False (no leading zeros) or True.
            //if (this.NumberLeadingZeros != "") res += "NumberLeadingZeros=" + this.NumberLeadingZeros + "\r\n";

            // schema.ini CurrencySymbol
            // Indicates the currency symbol that can be used for currency values in the text file. Examples include the dollar sign ($) and Dm.
            //if (this.CurrencySymbol != "") res += "CurrencySymbol=" + this.CurrencySymbol + "\r\n";

            // schema.ini CurrencyPosFormat
            // Can be set to any of the following values:
            // - Currency symbol prefix with no separation($1)
            // - Currency symbol suffix with no separation(1$)
            // - Currency symbol prefix with one character separation($ 1)
            // - Currency symbol suffix with one character separation(1 $)
            //if (this.CurrencyPosFormat != "") res += "CurrencyPosFormat=" + this.CurrencyPosFormat + "\r\n";

            // schema.ini CurrencyDigits
            // Specifies the number of digits used for the fractional part of a currency amount.
            //if (this.CurrencyDigits > 0) res += "CurrencyDigits=" + this.CurrencyDigits + "\r\n";

            // schema.ini CurrencyNegFormat
            // Can be one of the following values:
            // ($1)  -$1  $-1  $1 - (1$)  -1$  1 -$  1$- -1 $  -$ 1  1 $-  $ 1 -  $ -1  1 - $  ($ 1)  (1 $)
            // This example shows the dollar sign, but you should replace it with the appropriate CurrencySymbol value in the actual program.
            //if (this.CurrencyNegFormat != "") res += "CurrencyNegFormat=" + this.CurrencyNegFormat + "\r\n";

            // schema.ini CurrencyThousandSymbol
            // Indicates the single-character symbol that can be used for separating currency values in the text file by thousands.
            //if (this.CurrencyThousandSymbol != '\0') res += "CurrencyThousandSymbol=" + this.CurrencyThousandSymbol + "\r\n";

            // schema.ini CurrencyDecimalSymbol
            // Can be set to any single character that is used to separate the whole from the fractional part of a currency amount.
            //if (this.CurrencyDecimalSymbol != '\0') res += "CurrencyDecimalSymbol=" + this.CurrencyDecimalSymbol + "\r\n";

            // schema.ini SkipLines (Not part of official schema.ini format)
            // How many lines to skip at start of data file
            if (this.SkipLines > 0) res += ";SkipLines=" + this.SkipLines + "\r\n";

            // schema.ini Comment character
            // Skip lines if they start with this character
            if (this.CommentChar != '\0') res += ";CommentChar=" + this.CommentChar + "\r\n";

            // either all column names are in quotes or none, not mixed
            bool quotename = false;
            foreach (var fld in this.Fields)
                if (fld.Name.IndexOf(" ") >= 0)
                {
                    quotename = true;
                    break;
                }

            // schema.ini all columns
            for (int i = 0; i < this.Fields.Count; i++)
            {
                // format as inifile column line
                // example "Col1=LastName Text Width 50"
                CsvColumn col = this.Fields[i];
                string def = col.Name;
                string com = "";

                // add quotes "" only when name contains space
                //if (this.Name.IndexOf(" ") >= 0) def = string.Format("\"{0}\"", col.Name);
                if (quotename) def = string.Format("\"{0}\"", col.Name);

                // enumeration metadata
                if (col.isCodedValue)
                {
                    var codedlist = string.Join("|", col.CodedList);
                    // schma.ini doesn't support enumeration/coded values
                    com = string.Format(";Col{0}={1} Enumeration {2}\r\n", i + 1, def, codedlist);
                }

                // datatype
                if (col.DataType == ColumnType.String) def += " Text";
                if (col.DataType == ColumnType.Unknown) def += " Text";
                if (col.DataType == ColumnType.Integer) def += " Integer";
                if (col.DataType == ColumnType.Decimal)
                {
                    def += " Float";
                    // exception when float decimals different
                    if (col.Decimals != this.NumberDigits)
                    {
                        // schma.ini doesn't support multiple floating point formats, i.e. with different amounts of decimals
                        com = string.Format(";Col{0}={1} {2}\r\n", i + 1, def, col.Mask);
                    }
                }

                if (col.DataType == ColumnType.DateTime)
                {
                    // exception when datetime format different
                    if (col.Mask == this.DateTimeFormat)
                    {
                        def += " DateTime";
                    }
                    else
                    {
                        // schma.ini doesn't support multiple datetime formats
                        com = string.Format(";Col{0}={1} DateTime {2}\r\n", i + 1, def, col.Mask);
                        def += " Text";
                    }
                }

                //col += " " + this.Mask;
                def += " Width " + col.MaxWidth;

                // "Col1=LastName Text Width 50"
                res += string.Format("Col{0}={1}\r\n", i + 1, def);

                // add alternative column format as comment
                if (com != "") res += com;
            }

            return res;
        }

        /// <summary>
        /// when reading a data file, skip first comment lines at start of the file
        /// </summary>
        /// <param name="data"> csv data </param>
        public int SkipCommentLinesAtStart(StreamReader strdata)
        {
            var res = 0;
            // SkipLines parameter, how many lines to skip at start of file
            int skip = this.SkipLines;

            while ((skip > 0) && (!strdata.EndOfStream))
            {
                // consume and ignore lines
                strdata.ReadLine();
                skip--;
                res++;
            }

            // CommentChar parameter, skip any lines that start with comment character
            while ((strdata.Peek() == this.CommentChar) && (!strdata.EndOfStream))
            {
                // consume and ignore lines
                strdata.ReadLine();
                res++;
            }

            // keep track of actual parsed line number
            ParseCurrentLine += res;

            // return how many skipped lines
            return res;
        }

        /// <summary>
        /// when editing, sorting or copying a data file, also copy the first comment lines
        /// </summary>
        /// <param name="data"> csv data </param>
        public void CopyCommentLinesAtStart(StreamReader strdata, StringBuilder sb, string prefix, string CRLF)
        {
            // how many lines of comment to skip
            int skip = (this.SkipLines >= 0 ? this.SkipLines : 0);

            while ((skip > 0) && (!strdata.EndOfStream))
            {
                // consume and ignore lines
                String line = strdata.ReadLine();

                // copy and add prefix and line feed
                sb.Append(prefix);
                sb.Append(line);
                sb.Append(CRLF);

                skip--;
            }
        }

        /// <summary>
        /// when editing, sorting or copying a data file, also copy the comment lines throughout the data
        /// </summary>
        /// <param name="data"> csv data </param>
        public void CopyCommentLine(List<String> sldata, StringBuilder sb, string prefix, string postfix)
        {
            // skip comment lines
            if (sldata.Count > 0)
            {
                // copy and add prefix and line feed
                sb.Append(prefix);
                sb.Append(sldata[0]);
                sb.Append(postfix);
                sb.Append("\n");
            }
        }

        /// Keep track of actual line number in file, take into account skipped lines and quoted strings with CrLf
        public int ParseCurrentLine { get; set; } = 0;

        /// <summary>
        /// reformat file for date, decimal and separator
        /// </summary>
        /// <param name="data"> csv data </param>
        public List<string> ParseNextLine(StreamReader strdata, out bool iscomment)
        {
            // algorithm in part based on "How can I parse a CSV string with JavaScript, which contains comma in data?"
            // answer by user Bachor https://stackoverflow.com/a/58181757/1745616

            // initialise return list and bool
            var res = new List<string>();
            iscomment = false;

            StringBuilder value = new StringBuilder();

            if (Separator == '\0')
            {
                String line = strdata.ReadLine();
                ParseCurrentLine++;

                // fixed width columns
                int pos = 0;
                int fieldcount = FieldWidths.Count - 1;
                for (int i = 0; i <= fieldcount; i++)
                {
                    // next column width
                    int w = FieldWidths[i];

                    // if line is too short, columns missing?
                    if (pos + w > line.Length) w = line.Length - pos;

                    // get column value
                    String fixval = line.Substring(pos, w);
                    // trim values is optional, but for fixed length columns it is recommended
                    if (Main.Settings.TrimValues)
                    {
                        fixval = fixval.Trim();
                        fixval = CsvEdit.RemoveQuotesToString(fixval);
                    }
                    res.Add(fixval);

                    // start position of next column
                    pos += w;

                    // if line too long add extra column 
                    if ((i == fieldcount) && (line.Length > pos))
                    {
                        // add rest of line as one extra column
                        fixval = line.Substring(pos, line.Length - pos);
                        // trim values is optional, but for fixed length columns it is recommended
                        if (Main.Settings.TrimValues)
                        {
                            fixval = fixval.Trim();
                            fixval = CsvEdit.RemoveQuotesToString(fixval);
                        }
                        res.Add(fixval);
                    }
                }
            }
            else
            {
                // variables
                bool quote = false;
                iscomment = ((char)strdata.Peek() == CommentChar);
                bool wasquoted = false;
                bool bNextCol = false;
                bool isEOL = false;
                char quote_char = Main.Settings.DefaultQuoteChar;
                bool whitespace = true; // to catch where value is just two quotes "" right at start of line

                while (!strdata.EndOfStream)
                {
                    char cur = (char)strdata.Read();
                    char next = (char)strdata.Peek();

                    if (iscomment)
                    {
                        // comment consume to end-of-line
                        if ((cur == '\r') && (next == '\n')) { strdata.Read(); bNextCol = true; isEOL = true; } // double carriage return/linefeed so also consume next character (i.e. skip it)
                        else if ((cur == '\n') || (cur == '\r')) { bNextCol = true; isEOL = true; }
                        else if (cur != '\0') value.Append(cur); // TODO: is check '\0' really needed here?
                    }
                    else if (!quote)
                    {
                        // check if starting a quoted value or going next column or going to next line
                        if ((cur == quote_char) && whitespace) { quote = true; wasquoted = true; whitespace = false; value.Clear(); } // Exception for ..,  "12,3",.. and do value.Clear() -> i.e. ignore whitespace before quote
                        else if (cur == Separator) { bNextCol = true; }
                        else if ((cur == '\r') && (next == '\n')) { strdata.Read();  bNextCol = true; isEOL = true; } // double carriage return/linefeed so also consume next character (i.e. skip it)
                        else if ((cur == '\n') || (cur == '\r')) { bNextCol = true; isEOL = true; }
                        else if (cur != '\0') value.Append(cur); // TODO: is check '\0' really needed here?

                        // If separator directly followed by spaces then interpret spaces as empty whitespace,
                        // any quotes following  empty/white are interpreted as starting/opening quote, so for example:
                        // ..,  "12,3",..
                        // ..,"",..
                        if ((whitespace) && (cur != ' ')) whitespace = false;
                    }
                    else
                    {
                        if ((cur == quote_char) && (next == quote_char)) { value.Append(cur); strdata.Read(); } // double " within quotes so also consume next character (i.e. skip it)
                        else if (cur == quote_char) quote = false;
                        else
                        {
                            value.Append(cur);

                            // also count carriage returns within quotes
                            //if      ((cur == '\r') && (next == '\n')) ; // double \r\n do nothing, let the next '\n' character count the line break when examining the next character cur == '\n'
                            //else if ((cur == '\n') || (cur == '\r')) ParseCurrentLine++;
                            if ((cur == '\n') || ((cur == '\r') && (next != '\n'))) ParseCurrentLine++;
                        }
                    }

                    // if next col or next line
                    if (bNextCol)
                    {
                        // check if column value is NULL value
                        var csvval = value.ToString();
                        if ((wasquoted == false) && (csvval == Main.Settings.NullKeyword)) csvval = "";
                        if (Main.Settings.TrimValues) csvval = csvval.Trim();

                        // add column value
                        res.Add(csvval);
                        value.Clear();

                        bNextCol = false;
                        wasquoted = false;
                        whitespace = true;
                    }

                    if (isEOL)
                    {
                        ParseCurrentLine++;
                        break;
                    }

                    isEOL = false;
                }

                // also add last; if any left over value OR exception of file ends with separator so the very last value is empty
                if ( (value.Length > 0) || ((isEOL == false) && strdata.EndOfStream) )
                {
                    // check if column value is NULL value
                    var val = value.ToString();
                    if ((wasquoted == false) && (val == Main.Settings.NullKeyword)) val = "";
                    if (Main.Settings.TrimValues) val = val.Trim();
                    res.Add(val);
                }
            }

            return res;
        }

        /// <summary>
        /// Based on the CsvDefinition, construct one line with the columns header names
        /// </summary>
        public string ConstructHeader()
        {
            string res = "";

            for (int c = 0; c < this.Fields.Count; c++)
            {
                // get field name
                var nam  = this.Fields[c].Name;

                if (this.Separator == '\0')
                {
                    // fixed width
                    res += nam.PadRight(Fields[c].MaxWidth, ' ');
                }
                else
                {
                    // apply quotes
                    nam = CsvEdit.ApplyQuotesToString(nam, this.Separator, ColumnType.String);

                    // character separated
                    res += (c > 0 ? this.Separator.ToString() : "") + nam;
                }
            }

            return res;
        }

        /// <summary>
        /// Based on the CsvDefinition, take array of data values and (re)constructs one line of output
        /// </summary>
        public string ConstructLine(List<string> values, bool iscomment)
        {
            string res = "";

            if (iscomment)
            {
                if (values.Count > 0) res = values[0];
            }
            else
            {
                for (int c = 0; c < this.Fields.Count; c++)
                {
                    // get value
                    var val = (c < values.Count ? values[c] : "");

                    if (this.Separator == '\0')
                    {
                        // fixed width
                        if ( (Fields[c].DataType == ColumnType.Integer) || (Fields[c].DataType == ColumnType.Decimal) )
                            res += val.PadLeft(Fields[c].MaxWidth, ' ');
                        else
                            res += val.PadRight(Fields[c].MaxWidth, ' ');
                    }
                    else
                    {
                        // apply quotes
                        val = CsvEdit.ApplyQuotesToString(val, this.Separator, Fields[c].DataType);

                        // character separated
                        res += (c > 0 ? this.Separator.ToString() : "") + val;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Based on the CsvDefinition, return column positions as a comma-separated string based on column widths
        /// </summary>
        /// <param name="abspos"> absolute column positions or column widths</param>
        public string GetColumnWidths(bool abspos)
        {
            var res = "";
            var colwidth = 0;

            for (int c = 0; c < Fields.Count; c++)
            {
                // next field
                if (abspos)
                {
                    colwidth += Fields[c].MaxWidth;
                }
                else
                {
                    colwidth = Fields[c].MaxWidth;
                }
                var comma = (c < Fields.Count - 1 ? ", " : "");
                res += string.Format("{0}{1}", colwidth, comma);
            }

            return res;
        }

    }
}
