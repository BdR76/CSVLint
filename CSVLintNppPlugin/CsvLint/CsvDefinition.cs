﻿// -------------------------------------
// CsvDefinition
// Holds file and column definition of csv data
// method to infer definition from text data
// load definition from schema.ini file
// save definition to schema.ini file
// -------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLint
{
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

        private void Initialize()
        {
            this.sTag = "";
            this.iTag = -1;

            if (this.DataType == ColumnType.DateTime)
            {
            }

            if (this.DataType == ColumnType.Decimal)
            {
                int pos1 = this.Mask.IndexOf('.');
                int pos2 = this.Mask.IndexOf(',');
                int p = (pos1 > pos2 ? pos1 : pos2);

                // sTag, thousand and decimand characters
                this.sTag = (pos1 > pos2 ? ",." : ".,");

                // iTag, max decimal places
                this.iTag = this.Mask.Length - p - 1;
            }
        }
    }

    /// <summary>
    /// Csv meta data definition
    /// </summary>
    public class CsvDefinition
    {
        /// column separator character
        public char Separator { get; set; } = '\0';

        /// comment character(?)
        public char CommentCharacter { get; set; } = '#';

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

        /// field widths
        public List<int> FieldWidths { get; set; }

        /// field definitions
        public List<CsvColumn> Fields { get; set; }

        /// first line contains column names
        public bool ColNameHeader { get; set; } = true;

        /// column names
        public string[] FieldNames { get; set; }

        // This will replace TextQualifier below - only " is used anyway
        public bool? UseQuotes { get; set; }
        public char TextQualifier { get; set; } = '"';

        public CsvDefinition()
        {
            Fields = new List<CsvColumn>();
        }

        public CsvDefinition(char separator)
        {
            this.Separator = separator;

            Fields = new List<CsvColumn>();
        }

        public CsvDefinition(char separator, char quoteEscapeChar, char commentChar, bool colNameHeader, List<int> fieldWidths = null)
        {
            this.Separator = separator;
            this.TextQualifier = quoteEscapeChar;
            this.CommentCharacter = commentChar;
            this.FieldWidths = fieldWidths;
            this.ColNameHeader = colNameHeader;
            this.UseQuotes = this.TextQualifier != default(char);

            Fields = new List<CsvColumn>();
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
                if (mask == "") mask = (this.DateTimeFormat == "" ? "yyyy-MM-dd" : this.DateTimeFormat);

                // remember first datetime mask as the schema.ini global datetime mask
                if (this.DateTimeFormat == "") this.DateTimeFormat = mask;
            }

            if (datatype == ColumnType.Decimal)
            {
                // get decimal position or -1 if not found
                int pos1 = mask.LastIndexOf('.');
                int pos2 = mask.LastIndexOf(',');

                if ( (pos1 >= 0) || (pos2 >= 0) ) {
                    this.DecimalSymbol = (pos1 > pos2 ? '.' : ',');
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

        public CsvDefinition(Dictionary<String, String> inikeys)
        {
            Fields = new List<CsvColumn>();

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
                    string Val = line.Value.Trim();
                    string vallow = Val.ToLower();
                    int vint;
                    bool bint = int.TryParse(Val, out vint);

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
                            this.Separator = Val[10]; // first character after "Delimited("
                        };
                    };

                    // schema.ini DateTimeFormat for all columns
                    if (k == "datetimeformat")
                    {
                        // internally the datetime mask is c# format,         example "dd/MM/yyyy HH:mm"
                        // externally the datetime mask is schema.ini format, example "dd/mm/yyyy hh:nn"
                        // for full date format documentation see https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings?redirectedfrom=MSDN
                        string mask = Val;
                        mask = mask.Replace("m", "M");
                        mask = mask.Replace("n", "m");
                        mask = mask.Replace("h", "H"); // hh=12h, HH=24h
                        this.DateTimeFormat = mask;
                    }

                    // schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
                    if (k == "decimalsymbol") this.DecimalSymbol = Val[0];

                    // schema.ini NumberDigits, Indicates the number of decimal digits in the fractional portion of a number.
                    if (k == "numberdigits") this.NumberDigits = vint;

                    // schema.ini NumberLeadingZeros, 
                    // Specifies whether a decimal value less than 1 and more than -1 should contain leading zeros; this value can be either False (no leading zeros) or True.
                    if (k == "numberleadingzeros") this.NumberLeadingZeros = (vallow[1] == 't' ? true : false);

                    // schema.ini CurrencySymbol
                    // Indicates the currency symbol that can be used for currency values in the text file. Examples include the dollar sign ($) and Dm.
                    if (k == "currencysymbol") this.CurrencySymbol = Val;

                    // schema.ini CurrencyPosFormat
                    // Can be set to any of the following values:
                    // - Currency symbol prefix with no separation($1)
                    // - Currency symbol suffix with no separation(1$)
                    // - Currency symbol prefix with one character separation($ 1)
                    // - Currency symbol suffix with one character separation(1 $)
                    if (k == "currencyposformat") this.CurrencyPosFormat = Val;

                    // schema.ini CurrencyDigits
                    // Specifies the number of digits used for the fractional part of a currency amount.
                    if (k == "currencydigits") this.CurrencyDigits = 2; // TODO Val parseint to int?

                    // schema.ini CurrencyNegFormat
                    // Can be one of the following values:
                    // ($1)  -$1  $-1  $1 - (1$)  -1$  1 -$  1$- -1 $  -$ 1  1 $-  $ 1 -  $ -1  1 - $  ($ 1)  (1 $)
                    // This example shows the dollar sign, but you should replace it with the appropriate CurrencySymbol value in the actual program.
                    if (k == "currencynegformat") this.CurrencyNegFormat = Val;

                    // schema.ini CurrencyThousandSymbol
                    // Indicates the single-character symbol that can be used for separating currency values in the text file by thousands.
                    if (k == "currencythousandsymbol") this.CurrencyThousandSymbol = Val[0];

                    // schema.ini CurrencyDecimalSymbol
                    // Can be set to any single character that is used to separate the whole from the fractional part of a currency amount.
                    if (k == "currencydecimalsymbol") this.CurrencyDecimalSymbol = Val[0];

                    // schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
                    if (k == "colnameheader")
                    {
                        this.ColNameHeader = (vallow[0] == 't');
                    } else
                    // schema.ini DateTimeFormat for all columns
                    if (k.Substring(0, 3) == "col")
                    {
                        string s = k.Substring(3, k.Length - 3);
                        int idx = 0;
                        try
                        {
                            idx = Int32.Parse(s) - 1;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Unable to parse '{idx}'");
                        }

                        // parse column metadata, for example Val = "Test123 Int Width 3" or Val = "Test123 DateTime Width 10 NOT NULL"
                        // assume default values
                        string name = string.Format("Column{0}", idx);
                        string notnull = "";
                        string width = "";
                        string datatypestr = "";
                        int maxwidth = 50;
                        ColumnType datatype = ColumnType.String;
                        string mask = "";

                        int pos;
                        int spc;

                        // NOT NULL must be at end of line
                        pos = vallow.LastIndexOf("not null");
                        if (pos == Val.Count() - 8)
                        {
                            notnull = vallow.Substring(pos, Val.Length - pos);
                            Val = Val.Substring(0, pos).Trim();
                            vallow = Val.ToLower();
                        }

                        // WIDTH must be at end of line
                        spc = vallow.LastIndexOf(" ");
                        pos = vallow.LastIndexOf("width");
                        if (pos == spc - 5)
                        {
                            width = vallow.Substring(pos, Val.Length - pos);
                            Val = Val.Substring(0, pos).Trim();
                            vallow = Val.ToLower();

                            width = width.Replace("width ", "");
                            int n;
                            if (int.TryParse(width, out n))
                            {
                                maxwidth = n;
                            };
                        }

                        // valid datatype must be at end of line
                        spc = vallow.LastIndexOf(" ");
                        pos = vallow.LastIndexOf("text");
                        if (pos == -1) pos = vallow.LastIndexOf("datetime");
                        if (pos == -1) pos = vallow.LastIndexOf("float");
                        if (pos == -1) pos = vallow.LastIndexOf("int");
                        if ((pos >= 0) && (pos == spc + 1))
                        {
                            datatypestr = vallow.Substring(pos, Val.Length - pos);
                            Val = Val.Substring(0, pos).Trim();
                        }
                        // schema.ini datatype, string to ColumnType
                        if (datatypestr == "bit")      datatype = ColumnType.Integer;
                        if (datatypestr == "byte")     datatype = ColumnType.Integer;
                        if (datatypestr == "short")    datatype = ColumnType.Integer;
                        if (datatypestr == "long")     datatype = ColumnType.Integer;
                        if (datatypestr == "currency") datatype = ColumnType.Decimal;
                        if (datatypestr == "single")   datatype = ColumnType.Decimal;
                        if (datatypestr == "double")   datatype = ColumnType.Decimal;
                        if (datatypestr == "datetime") datatype = ColumnType.DateTime;
                        if (datatypestr == "text")     datatype = ColumnType.String;
                        if (datatypestr == "memo") datatype = ColumnType.String;
                        if (datatypestr == "float")    datatype = ColumnType.Decimal; // Float same as Double
                        if (datatypestr == "integer")  datatype = ColumnType.Integer; // Integer same as Short
                        if (datatypestr == "longchar") datatype = ColumnType.String; // LongChar same as Memo
                        if (datatypestr == "date") datatype = ColumnType.DateTime;

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

                            mask = string.Format("{0}{1}{2}", mask.PadLeft(dig, '9'), this.DecimalSymbol, mask.PadLeft(dec, '9'));
                        };

                        // any left is the name of the column
                        name = Val;

                        // if quotes around name because of spaces
                        int quote1 = Val.IndexOf('"');
                        int quote2 = Val.LastIndexOf('"');

                        // check if incorrect and just one quote
                        if (quote1 == quote2)
                        {
                            if (quote1 == 0) quote2 = Val.Length; // only quote at start
                            if (quote1 > 0) quote1 = -1;          // only quote at end
                        }

                        // if any quotes around name then remove them
                        if (quote1 > 0 || quote2 > 0) name = Val.Substring(quote1 + 1, quote2 - quote1 - 1);

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
                            idxalt = Int32.Parse(s) - 1;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Unable to parse '{idxalt}'");
                        }

                        ColumnType datatypealt = ColumnType.String;

                        // check for valid datatype must be at end of line
                        int posalt = Val.LastIndexOf("DateTime");
                        if (posalt >= 0)
                        {
                            int spcalt = Val.IndexOf(" ", posalt);
                            datatypestr = Val.Substring(posalt, spcalt - posalt);
                            Val = Val.Substring(spcalt, Val.Length-spcalt).Trim();
                        }
                        if (datatypestr == "DateTime") datatypealt = ColumnType.DateTime;

                        // if alternative datatype found
                        if (datatypealt != ColumnType.String)
                        {
                            // look for index
                            for (int x = 0; x < this.Fields.Count; x++)
                            {
                                if (this.Fields[x].Index == idxalt)
                                {
                                    this.Fields[x].DataType = datatypealt;
                                    this.Fields[x].Mask = Val;
                                }
                            };
                        }
                    }
                }
            }
        }
        public string getIniLines()
        {
            string res = "";

            // defaults
            if (this.Separator == '\t') res += "Format=TabDelimited\r\n";
            if (this.Separator == ',')  res += "Format=CSVDelimited\r\n";
            if (this.Separator == '\0') res += "Format=FixedLength\r\n";
            // custom character
            if (res == "")              res += "Format=Delimited(" + this.Separator + ")\r\n";


            res += "ColNameHeader=" + this.ColNameHeader + "\r\n";

            // schema.ini DateTimeFormat for all columns
            if (this.DateTimeFormat != "")
            {
                // internally the datetime mask is c# format,         example "dd/MM/yyyy HH:mm"
                // externally the datetime mask is schema.ini format, example "dd/mm/yyyy hh:nn"
                string mask = this.DateTimeFormat;
                mask = mask.Replace("m", "n");
                mask = mask.Replace("M", "m");
                mask = mask.Replace("H", "h");
                res += "DateTimeFormat=" + mask + "\r\n";
            }

            // schema.ini DecimalSymbol, typically ',' or '.' but can be set to any single character that is used to separate the integer from the fractional part of a number.
            if (this.DecimalSymbol != '\0') res += "DecimalSymbol=" + this.DecimalSymbol + "\r\n";

            // schema.ini NumberDigits, Indicates the number of decimal digits in the fractional portion of a number.
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

                // datatype
                if (col.DataType == ColumnType.String) def += " Text";
                if (col.DataType == ColumnType.Unknown) def += " Text";
                if (col.DataType == ColumnType.Integer) def += " Integer";
                if (col.DataType == ColumnType.Decimal) def += " Float";
                if (col.DataType == ColumnType.DateTime) {
                    // exception when datetime format different
                    if (col.Mask == this.DateTimeFormat) {
                        def += " DateTime";
                    } else {
                        // schma.ini doesn't support multiple datetime formats
                        com = string.Format(";Col{0}={1} DateTime {2}\r\n", (i + 1), def, col.Mask);
                        def += " Text";
                    }
                }

                //col += " " + this.Mask;
                def += " Width " + col.MaxWidth;

                // "Col1=LastName Text Width 50"
                res += string.Format("Col{0}={1}\r\n", (i + 1), def);

                // add alternative column format as comment
                if (com != "") res += com;
            }

            return res;
        }

    }
}
