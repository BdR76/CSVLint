using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// 

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

        // full stats
        public int stat_minint = 0;
        public int stat_maxint = 0;
        public double stat_mindbl = 0.0;
        public double stat_maxdbl = 0.0;
        public DateTime stat_mindat;
        public DateTime stat_maxdat;
        public string stat_minint_org = "";
        public string stat_maxint_org = "";
        public string stat_mindbl_org = "";
        public string stat_maxdbl_org = "";

        public string stat_mindat_org = "";
        public string stat_maxdat_org = "";

        // date format is unknown when no data read yet
        public int stat_dat_dmy = 0; // 0=unknown, 1=YMD, 2=DMY, 3=MDY, when beginning assume day-month order in dateformat is unknown until a value confirms either YMD or DMY or MDY
        public string stat_dat_fromat = ""; // most likely format
        public DateTime stat_mindat_mdy;
        public DateTime stat_maxdat_mdy;
        public string stat_mindat_mdy_org = "";
        public string stat_maxdat_mdy_org = "";
        public Dictionary<String, int> stat_uniquecount = new Dictionary<String, int>();

        public CsvAnalyzeColumn(int idx)
        {
            this.Index = idx;
        }

        public void InputData(String data, int fixedLength, bool fullstats)
        {
            // count how many values
            this.CountAll++;

            // adjust for quoted values, trim first because can be a space before the first quote, example .., "BMI",..
            var datatrim = data.Trim();
            if ((datatrim.Length > 0) && (datatrim[0] == Main.Settings.DefaultQuoteChar))
            {
                data = data.Trim();
                data = data.Trim(Main.Settings.DefaultQuoteChar);
            }

            // next value to evaluate
            if (Main.Settings.TrimValues) data = data.Trim();

            // assume first line only contains column header names
            if (this.CountAll == 1)
            {
                this.Name = data;
            }
            else
            {
                // for fixed length files, the MaxWidth should be length of not-trimmed data
                int length = (fixedLength > 0 ? fixedLength : data.Length);

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

                    // keep full statistics
                    if (fullstats) KeepUniqueValues(data);

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
                    int ddmax1 = -1;
                    int ddmax2 = -1;
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
                        else if ("\\/-:. ".IndexOf(ch) >= 0) // check date separators
                        {
                            other++;
                            datesep++;
                            if ((sep1 == '\0') && (datesep == 1))
                            {
                                // check if numeric up to the first separator
                                sep1 = ch;
                                string datedig1 = data.Substring(0, data.IndexOf(ch));
                                // keep max number example 31-12-2020 -> keep 31
                                bool isNumeric = int.TryParse(datedig1, out int n1);
                                if (isNumeric)
                                {
                                    if (ddmax1 < n1) ddmax1 = n1;
                                }

                                // check if cannot be a datetime
                                if ( (!isNumeric) || (datedig1.Length == 3) || (datedig1.Length > 4))
                                {
                                    ddmax1 = -1; // force the most likely datatype to not be non-datetime
                                }
                            }
                            else if ((sep2 == '\0') && (datesep == 2))
                                {
                                // check if numeric up to the first separator
                                sep2 = ch;
                                int pos1 = data.IndexOf(sep1) + 1;
                                string datedig2 = data.Substring(pos1, data.IndexOf(ch, pos1) - pos1);
                                // keep max number example 31-12-2020 -> keep 31
                                bool isNumeric = int.TryParse(datedig2, out int n2);
                                if (isNumeric)
                                {
                                    if (ddmax2 < n2) ddmax2 = n2;
                                }
                                // check if cannot be a datetime
                                if ((!isNumeric) || (datedig2.Length == 3) || (datedig2.Length > 4))
                                {
                                    ddmax2 = -1; // force the most likely datatype to not be non-datetime
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
                            other--; // in hindsight was incorrectly counted as 'other' because ''. -> correction
                        }
                        // plus and minus are signs for digits, check separately because minus ('-') is also counted as date separator, example "31-12-1999"
                        if (ch == '+')
                        {
                            sign++;
                            signpos = charidx;
                        }
                        // plus and minus are signs for digits, check separately because minus ('-') is also counted as date separator, example "31-12-1999"
                        if (ch == '-')
                        {
                            sign++;
                            signpos = charidx;
                            other--; // in hindsight was incorrectly counted as 'other' because '-' could also be integer -> correction
                        }
                    }

                    // exception for time values like "12:23" -> also fill ddmax2 "23"
                    if ( (datesep == 1) && (vallength >= 4) && (vallength <= 5) )
                    {
                        // check if numeric up to the first separator
                        int pos3 = data.IndexOf(sep1) + 1;
                        string datedig3 = data.Substring(pos3, data.Length - pos3);
                        // keep max number example 31-12-2020 -> keep 31
                        bool checkNumeric = int.TryParse(datedig3, out int n3);
                        if (checkNumeric)
                        {
                            if (ddmax2 < n3) ddmax2 = n3;
                        }
                    }

                    // determine most likely datatype based on characters in string

                    // date, examples "31-12-2019", "1/1/2019", "2019-12-31", "1-1-99" etc.
                    if ((length >= 8) && (length <= 10) && (datesep == 2) && (sep1 != ':') && (digits >= 4) && (digits <= 8) && (ddmax1 > 0) && ((ddmax1 <= 31) || (ddmax1 >= 1900)))
                    {
                        this.CountDateTime++;
                        if (this.DateSep == '\0') this.DateSep = sep1;
                        if (this.DateMax1 < ddmax1) this.DateMax1 = ddmax1;
                        if (this.DateMax2 < ddmax2) this.DateMax2 = ddmax2;

                        // keep full statistics
                        if (fullstats) KeepMinMaxDateTime(data, ddmax1, ddmax2, 1);
                    }
                    // or datetime, examples "31-12-2019 23:59:00", "1/1/2019 12:00", "2019-12-31 23:59:59.000", "1-1-99 9:00" etc.
                    else if ((length >= 13) && (length <= 23) && (datesep > 2) && (datesep <= 6) && (digits >= 7) && (digits <= 17) && (ddmax1 > 0) && ((ddmax1 <= 31) || (ddmax1 >= 1900)))
                    {
                        this.CountDateTime++;
                        if (this.DateSep == '\0') this.DateSep = sep1;
                        if (this.DateMax1 < ddmax1) this.DateMax1 = ddmax1;
                        if (this.DateMax2 < ddmax2) this.DateMax2 = ddmax2;

                        // keep full statistics
                        if (fullstats) KeepMinMaxDateTime(data, ddmax1, ddmax2, 3);
                    }
                    // or time, examples "9:00", "23:59:59", "23:59:59.000" etc.
                    else if ((length >= 4) && (length <= 12) && (sep1 == ':') && (datesep >= 1) && (datesep <= 3) && (digits >= 3) && (digits <= 9) && (ddmax1 > 0) && ((ddmax1 <= 23) && (ddmax2 <= 59)))
                    {
                        this.CountDateTime++;
                        if (this.DateSep == '\0') this.DateSep = sep1;
                        if (this.DateMax1 < ddmax1) this.DateMax1 = ddmax1;
                        if (this.DateMax2 < ddmax2) this.DateMax2 = ddmax2;

                        // keep full statistics
                        if (fullstats) KeepMinMaxDateTime(data, ddmax1, ddmax2, 2);
                    }
                    else if ((digits > 0) && (point != 1) && (comma != 1) && (sign <= 1) && (signpos == 0) && (other == 0) && (length <= Main.Settings.IntegerDigitsMax))
                    {
                        // numeric integer, examples "123", "-99", "+10" etc. but not "000123"
                        if ((data.Length > 1) && (data[0] == '0'))
                        {
                            this.CountString++;
                        }
                        else
                        {
                            this.CountInteger++;
                            // keep full statistics
                            if (fullstats) KeepMinMaxInteger(data);
                        }
                    }
                    else if ((digits > 0) && ((point == 1) || (comma == 1)) && (sign <= 1) && (other == 0) && (datesep <= 2) ) // datesep <= 2 for example "-12.34" a dot and a minus
                    {
                        // numeric integer, examples "12.3", "-99,9" etc.
                        this.CountDecimal++;
                        if (dec == '.') this.CountDecimalPoint++;
                        if (dec == ',') this.CountDecimalComma++;

                        // maximum decimal places, example "1234.567" = 4 digits and 3 decimals
                        int countdec = data.Length - data.LastIndexOf(dec) - 1;
                        int countdig = data.Length - countdec - 1;

                        if (countdec <= Main.Settings.DecimalDigitsMax)
                        {
                            if (countdec > this.DecimalDecMax) this.DecimalDecMax = countdec;
                            if (countdig > this.DecimalDigMax) this.DecimalDigMax = countdig;

                            // keep full statistics
                            if (fullstats) KeepMinMaxDecimal(data, dec);
                        }
                        else
                            this.CountString++;
                    }
                    else
                    {
                        // any other is general text/varchar/string
                        this.CountString++;
                    };
                }
            }
        }

        /// <summary>
        /// Keep more stats only when running full analyze data report, not when scanning meta data
        /// </summary>
        public void KeepMinMaxInteger(String value)
        {
            // try parse as integer
            if (int.TryParse(value, out int valint))
            {
                // keep the minimum values
                if ((valint < stat_minint) || (stat_minint_org == ""))
                {
                    stat_minint = valint;
                    stat_minint_org = value;
                }

                // keep the maximum values
                if ((valint > stat_maxint) || (stat_maxint_org == ""))
                {
                    stat_maxint = valint;
                    stat_maxint_org = value;
                }
            }
        }

        public void KeepMinMaxDecimal(String value, char dec)
        {
            // try parse as integer
            if (float.TryParse(value, out float valdbl))
            {
                // keep the minimum values
                if ((valdbl < stat_mindbl) || (stat_mindbl_org == ""))
                {
                    stat_mindbl = valdbl;
                    stat_mindbl_org = value;
                }

                // keep the maximum values
                if ((valdbl > stat_maxdbl) || (stat_maxdbl_org == ""))
                {
                    stat_maxdbl = valdbl;
                    stat_maxdbl_org = value;
                }
            }
        }

        public void KeepMinMaxDateTime(String value, int ddmax1, int ddmax2, int datatype)
        {
            // TODO: this is not optimal, could still miss some minimum/maximum dates when initially assuming incorrect format

            // try to determine datetime format
            if (stat_dat_dmy == 0)
            {
                bool newformat = (stat_dat_fromat == "");

                // date or datetime
                if ((datatype == 1) || (datatype == 3))
                {
                    // YMD = 1
                    if (ddmax1 > 1000)
                    {
                        stat_dat_dmy = 1;
                        stat_dat_fromat = string.Format("yyyy{0}M{0}d", this.DateSep == '\0' ? "" : this.DateSep.ToString());
                        newformat = true;
                    }

                    // DMY = 2
                    if ((ddmax1 > 12) && (ddmax1 <= 31))
                    {
                        stat_dat_dmy = 2;
                        stat_dat_fromat = string.Format("d{0}M{0}yyyy", this.DateSep == '\0' ? "" : this.DateSep.ToString());
                        newformat = true;
                    }

                    // MDY = 3
                    if ((ddmax2 > 12) && (ddmax2 <= 31))
                    {
                        stat_dat_dmy = 3;
                        stat_dat_fromat = string.Format("M{0}d{0}yyyy", this.DateSep == '\0' ? "" : this.DateSep.ToString());
                        newformat = true;
                    }

                    // if not yet clear
                    if (stat_dat_fromat == "")
                    {
                        if (ddmax1 > 31)
                        {
                            stat_dat_fromat = string.Format("yyyy{0}M{0}d", this.DateSep == '\0' ? "" : this.DateSep.ToString());
                        }
                        else
                        {
                            stat_dat_fromat = string.Format("d{0}M{0}yyyy", this.DateSep == '\0' ? "" : this.DateSep.ToString());
                        }
                    }
                }

                // add time part
                // date or datetime
                if ((datatype == 2) || (datatype == 3))
                {
                    if (newformat)
                    {
                        // space between
                        if (stat_dat_fromat != "") stat_dat_fromat += " ";

                        // count how many ':'
                        int count = 0;
                        foreach (var c in value) if (c == ':') count++;
                        if (count == 1) stat_dat_fromat += "H:mm";
                        if (count == 2) stat_dat_fromat += "H:mm:ss";
                    }
                }
            }

            // try parse as datetime
            if (DateTime.TryParseExact(value, stat_dat_fromat,
                                        Main.dummyCulture,
                                        DateTimeStyles.None,
                                        out DateTime valdat))
            {

                // keep the minimum values
                if ((valdat < stat_mindat) || (stat_mindat_org == ""))
                {
                    stat_mindat = valdat;
                    stat_mindat_org = value;
                }

                // keep the maximum values
                if ((valdat > stat_maxdat) || (stat_maxdat_org == ""))
                {
                    stat_maxdat = valdat;
                    stat_maxdat_org = value;
                }
            }
        }

        public void KeepUniqueValues(String value)
        {
            // when already found X unique values, no use in keep counting; probably not coded anyway
            if (stat_uniquecount.Count <= Main.Settings.UniqueValuesMax)
            {
                // count unique value(s)
                if (!stat_uniquecount.ContainsKey(value))
                    stat_uniquecount.Add(value, 1);
                else
                    stat_uniquecount[value]++;
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

            // if mixed integers and decimals, check percentage of decimals
            if ((this.CountInteger > 0) && (this.CountDecimal > 0))
            {
                // decimal values ratio to integer values
                var decratio = (1.0 * this.CountDecimal) / (this.CountInteger + this.CountDecimal);

                // if larger than 1 percent, example 10 decimal values and 990 integers
                if (decratio > 0.01)
                {
                    // consider it to be a decimal column
                    this.CountDecimal += this.CountInteger;
                    this.CountInteger = 0;
                }
				// if less than 1% then interpret column as integers and decimals are errors in data
            }

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

                // if just hour minutes not seconds, example "12:59"
                if ((this.DateSep == ':') && (this.MaxWidth <= 5))
                {
                    mask = mask.Replace(":ss", "");
                }

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
