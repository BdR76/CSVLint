using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLint
{
    class CsvAnalyzeColumn
    {
        /// <summary>
        /// Csv Analyze Column, keep stats from data, determine datatype width etc.
        /// </summary>

        // column statistics
        public String Name = "";
        public int Index = 0;
        public int MinWidth = 9999;
        public int MaxWidth = 0;
        public int FixedWidth = 0; // fixed width
        public int CountAll = 0;
        public int CountEmpty = 0;
        public int CountString = 0;
        public int CountInteger = 0;
        public int CountDecimal = 0;
        public int CountDecimalComma = 0;
        public int CountDecimalPoint = 0;
        public int DecimalDigMax = 0; // maximum digits, example "1234.5" = 4 digits
        public int DecimalDecMax = 0; // maximum decimals, example "123.45" = 2 decimals
        public int CountDateTime = 0;
        public char DateSep = '\0';
        public int DateMax1 = 0;
        public int DateMax2 = 0;
        public int DateMax3 = 0;

        public CsvAnalyzeColumn(int idx)
        {
            this.Index = idx;
        }

        public void InputData(String data, bool fixedwidth)
        {
            // count how many values
            this.CountAll++;

            // next value to evaluate
            int length = data.Length;
            data = data.Trim();

            // adjust for quoted values
            if ( (data.Length > 0) && (data[0] == '"') )
            {
                data = data.Trim('"');
            }

            // assume first line only contains column header names
            if (this.CountAll == 1)
            {
                this.Name = data;

                // TODO: determine if first row is actually header names
                if ((this.Name == "") || (fixedwidth) ) {
                    this.Name = "F" + (this.Index + 1);
                }
            }
            else
            {
                // for fixed length files, the MaxWidth should be length of not-trimmed data
                if (!fixedwidth) length = data.Length;

                // check for empty values, count empty strings
                if (data.Length == 0)
                {
                    this.CountEmpty++;
                }
                else
                {
                    // keep minimum width
                    if (length < this.MinWidth) this.MinWidth = length;

                    // keep maximum width
                    if (length > this.MaxWidth) this.MaxWidth = length;

                    // check each character in string
                    int digits = 0;
                    int sign = 0;
                    int signpos = 0;
                    int point = 0;
                    int comma = 0;
                    int datesep = 0;
                    int other = 0;
                    char sep1 = '\0';
                    char sep2 = '\0';
                    int ddmax1 = 0;
                    int ddmax2 = 0;
                    char dec = '\0';

                    // inspect all characters of string
                    int vallength = data.Length;
                    for (int charidx = 0; charidx < vallength; charidx++)
                    {
                        char ch = data[charidx];

                        if (ch >= '0' && ch <= '9')
                        {
                            digits++;
                        }
                        else if (ch == ',')
                        {
                            comma++;
                            dec = ch;
                        }
                        else if ("\\/-:. ".IndexOf(ch) > 0)
                        {
                            datesep++;
                            if (sep1 == '\0')
                            {
                                // check if numeric up to the first separator
                                sep1 = ch;
                                string datedig1 = data.Substring(0, data.IndexOf(ch));
                                // keep max number example 31-12-2020 -> keep 31
                                bool isNumeric = int.TryParse(datedig1, out int n);
                                if (isNumeric)
                                {
                                    if (ddmax1 < n) ddmax1 = n;
                                }
                            }
                            else if (sep2 == '\0')
                            {
                                // check if numeric up to the first separator
                                sep2 = ch;
                                int pos1 = data.IndexOf(sep1) + 1;
                                string datedig1 = data.Substring(pos1, data.IndexOf(ch, pos1) - pos1);
                                // keep max number example 31-12-2020 -> keep 31
                                bool isNumeric = int.TryParse(datedig1, out int n);
                                if (isNumeric)
                                {
                                    if (ddmax2 < n) ddmax2 = n;
                                }
                            }
                        }
                        else
                        {
                            other++;
                        };

                        // dot is decimal separator, check separately because dot is also counted as date separator, example "12.03.2018"
                        if (ch == '.')
                        {
                            point++;
                            dec = ch;
                        }
                        // plus and minus are signs for digits, check separately because minus ('-') is also counted as date separator, example "31-12-1999"
                        if (ch == '+' || ch == '-')
                        {
                            sign++;
                            signpos = charidx;
                        }
                    }

                    // determine most likely datatype based on characters in string

                    // date, examples "31-12-2019", "1/1/2019", "2019-12-31" etc.
                    if ((length >= 8) && (length <= 10) && (datesep == 2) && (other == 0))
                    {
                        this.CountDateTime++;
                        if (this.DateSep == '\0') this.DateSep = sep1;
                        if (this.DateMax1 < ddmax1) this.DateMax1 = ddmax1;
                        if (this.DateMax2 < ddmax2) this.DateMax2 = ddmax2;
                    }
                    // or datetime, examples "31-12-2019 23:59:00", "1/1/2019 12:00", "2019-12-31 23:59:59.000" etc.
                    else if ((length >= 13) && (length <= 23) && (datesep >= 2) && (datesep <= 6) && (other == 0))
                    {
                        this.CountDateTime++;
                        if (this.DateSep == '\0') this.DateSep = sep1;
                        if (this.DateMax1 < ddmax1) this.DateMax1 = ddmax1;
                        if (this.DateMax2 < ddmax2) this.DateMax2 = ddmax2;
                    }
                    else if ((digits > 0) && (point != 1) && (comma != 1) && (sign <= 1) && (signpos == 0) && (length <= 8) && (other == 0))
                    {
                        // numeric integer, examples "123", "-99", "+10" etc.
                        this.CountInteger++;
                    }
                    else if ((digits > 0) && ((point == 1) || (comma == 1)) && (sign <= 1) && (length <= 12) && (datesep <= 2) && (other == 0)) // datesep <= 2 for example "-12.34" a dot and a minus
                    {
                        // numeric integer, examples "12.3", "-99,9" etc.
                        this.CountDecimal++;
                        if (dec == '.') this.CountDecimalPoint++;
                        if (dec == ',') this.CountDecimalComma++;

                        // maximum decimal places, example "1234.567" = 4 digits and 3 decimals
                        int countdec = data.Length - data.LastIndexOf(dec) - 1;
                        int countdig = data.Length - countdec - 1;
                        if (countdec > this.DecimalDecMax) this.DecimalDecMax = countdec;
                        if (countdig > this.DecimalDigMax) this.DecimalDigMax = countdig;
                    }
                    else
                    {
                        // any other is general text/varchar/string
                        this.CountString++;
                    };
                }
            }
        }

        public CsvColumn InferDatatype()
        {
            // determine most likely datatype based on data
            string mask = "";
            CsvColumn result = new CsvColumn(Index)
            {
                Name = Name,
                DataType = ColumnType.String,
                MaxWidth = 0,
                Mask = "",
                DecimalSymbol = '.',
                Decimals = 0
            };

            // check if whole number integers (no decimals)
            if ((this.CountInteger > this.CountString) && (this.CountInteger > this.CountDecimal) && (this.CountInteger > this.CountDateTime))
            {
                result.DataType = ColumnType.Integer;
            }
            // check decimals/numeric
            else if ((this.CountDecimal > this.CountString) && (this.CountDecimal > this.CountInteger) && (this.CountDecimal > this.CountDateTime))
            {
                result.DataType = ColumnType.Decimal;
                char dec = (this.CountDecimalPoint > this.CountDecimalComma ? '.' : ',');

                // mask, example "9999.99"
                mask = string.Format("{0}{1}{2}", mask.PadLeft(this.DecimalDigMax, '9'), dec, mask.PadLeft(this.DecimalDecMax, '9'));

                result.Decimals = this.DecimalDecMax;

                // Note: when dataset contains values "12.345" and "1234.5" then maxlength=6
                // However then DecimalDigMax=4 and DecimalDigMax=3 so mask is "9999.999" and maxlength should be 8 (not 6)
                if (mask.Length < this.MaxWidth)
                {
                    this.MaxWidth = mask.Length;
                };

                // keep global decimal point character
                result.DecimalSymbol = dec;
            }
            // check date or datetime
            else if ((this.CountDateTime > this.CountString) && (this.CountDateTime > this.CountInteger) && (this.CountDateTime > this.CountDecimal))
            {
                result.DataType = ColumnType.DateTime;
                // dateformat order, assume normal format (TODO: get system default here, how?)
                var part1 = "dd";
                var part2 = "MM";
                var part3 = "yyyy";
                // if the first number is higher than 12 and second number is max 12, then most probable day-month format
                if ((this.DateMax1 > 12) && (this.DateMax1 <= 31) && (this.DateMax2 >= 1) && (this.DateMax2 <= 12))
                {
                    part1 = "dd";
                    part2 = "MM";
                }
                // if the first number is max 12 and second number is higher than 12, then most probable month-day format
                if ((this.DateMax1 >= 1) && (this.DateMax1 <= 12) && (this.DateMax2 > 12) && (this.DateMax2 <= 31))
                {
                    part1 = "MM";
                    part2 = "dd";
                }
                // if the first digit higher than 1000, then probably year
                if (this.DateMax1 > 1000)
                {
                    part1 = "yyyy";
                    part2 = "MM";
                    part3 = "dd";
                }
                // if first separator is ':' it's probably a time value example "01:23:45.678"
                if (this.DateSep == ':')
                {
                    part1 = "HH";
                    part2 = "mm";
                    part3 = "ss";
                }

                // build mask
                mask = string.Format("{0}{1}{2}{3}{4}", part1, this.DateSep, part2, this.DateSep, part3);

                // single digit year, example "31-12-99"
                if ((this.MinWidth == this.MaxWidth) && (this.MinWidth == 8))
                {
                    mask = mask.Replace("yyyy", "yy");
                }

                // also includes time
                if (this.MaxWidth >= 13) mask += " HH:mm"; // example "01-01-2019 12:00"
                if (this.MaxWidth > 16) mask += ":ss";    // example "1-1-2019 2:00:00"
                if (this.MaxWidth > 19) mask += ".fff";   // example "01-01-2019 12:00:00.000"

                // build mask, fixed length date "dd-mm-yyyy" or not fixed length "d-m-yyyy" without prefix zeroes
                if (this.MinWidth < this.MaxWidth)
                {
                    mask = mask.Replace("dd", "d").Replace("MM", "M").Replace("HH", "H");
                }
            };

            // keep global datetime format
            result.Mask = mask;

            result.MaxWidth = this.MaxWidth;

            // add column 
            return result;
        }
        public static String ColAsString(CsvColumn col)
        {

            String str = "";

            if (col.DataType == CSVLint.ColumnType.String)   str += "String";
            if (col.DataType == CSVLint.ColumnType.Integer)  str += "Integer";
            if (col.DataType == CSVLint.ColumnType.DateTime) str += "DateTime";
            if (col.DataType == CSVLint.ColumnType.Decimal)  str += "Decimal";
            if (col.DataType == CSVLint.ColumnType.Unknown)  str += "Unknown";

            str = str + "\r\nwidth=" + col.MaxWidth + "\r\n";
            str = str + "DecimalSymbol=" + col.DecimalSymbol + "\r\n";
            str = str + "Decimals=" + col.Decimals + "\r\n";

            return str;
        }
    }
}
