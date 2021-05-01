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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLint
{
    class CsvEdit
    {

        /// <summary>
        ///     reformat file for date, decimal and separator
        /// </summary>
        /// <param name="data"> csv data </param>
        public static void ReformatDataFile(CsvDefinition csvdef, string reformatDatTime, string reformatDecimal, string reformatSeparator, bool updateSeparator)
        {
            // TODO: nullable parameters

            // handle to editor
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;

            // use stringreader to go line by line
            var s = ScintillaStreams.StreamAllText();

            //var s = new StringReader(data);
            int linenr = 0;
            String line = "";
            String datanew = "";
            char newSep = (updateSeparator ? reformatSeparator[0] : csvdef.Separator);

            // process each line
            while ((line = s.ReadLine()) != null)
            {
                linenr++;
                // splite line into columns
                String[] data = csvdef.ParseData(line);

                // reformat data line to new line
                for (int c = 0; c < csvdef.Fields.Count; c++)
                {
                    // next value
                    String val = data[c];

                    // datetime reformat
                    if ( (csvdef.Fields[c].DataType == ColumnType.DateTime) && (reformatDatTime != "") )
                    {
                        // convert
                        try
                        {
                            val = DateTime.ParseExact(val, csvdef.Fields[c].Mask, CultureInfo.InvariantCulture).ToString(reformatDatTime, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                        }
                    };

                    // decimals reformat
                    if ((csvdef.Fields[c].DataType == ColumnType.Decimal) && (reformatDecimal != ""))
                    {
                        val = val.Replace(csvdef.DecimalSymbol, reformatDecimal[0]);
                    };

                    // construct new output data line
                    if (newSep == '\0')
                    {
                        // fixed width
                        int wid = csvdef.Fields[c].MaxWidth;
                        if ((csvdef.Fields[c].DataType == ColumnType.Integer) || (csvdef.Fields[c].DataType == ColumnType.Decimal))
                        {
                            datanew += val.PadLeft(wid, ' ');
                        }
                        else
                        {
                            datanew += val.PadRight(wid, ' ');
                        };
                        
                    }
                    else
                    {
                        // character separated
                        datanew += val + (c < csvdef.Fields.Count-1 ? newSep.ToString() : "");
                    }
                };

                // add line break
                datanew += '\n';
            };

            // update text in editor
            scintillaGateway.SetText(datanew);
        }

        /// <summary>
        ///     update all date columns to new date format
        /// </summary>
        /// <param name="data"> csv data </param>
        public void UpdateAllDateFormat(string data)
        {
            // read all text and replace
            var sr = ScintillaStreams.StreamAllText();
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;
        }

        /// <summary>
        ///     update all decimal columns to new decimal format
        /// </summary>
        /// <param name="data"> csv data </param>
        public void UpdateAllDecimal(string data)
        {
            // TODO implement
        }

        /// <summary>
        ///     update a single column to new data type
        /// </summary>
        /// <param name="data"> csv data </param>
        public void UpdateColumn(string data)
        {
            // TODO implement
        }

        /// <summary>
        ///     split invalid values of column into a new column
        /// </summary>
        /// <param name="data"> csv data </param>
        public void SplitColumn(string data)
        {
            // TODO implement
        }
    }
}
