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

namespace CSVLint
{
    class CsvGenerateCode
    {

        /// <summary>
        /// determine short variable names based on column names
        /// </summary>
        /// <param name="data"> csv data </param>
        public void DetermineVariableNames(string data)
        {
            // shorten variable names based on column names
        }

        /// <summary>
        /// determine short variable names based on column names
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
        /// generate Python code based on columns (most asked on stackoverflow)
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePython(string data)
        {
            return "Python";
        }

        /// <summary>
        /// generate Python Panda code based on columns (most asked on stackoverflow)
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePythonPanda(string data)
        {
            // fixed
            // import pandas
            // # Using Pandas with a column specification
            // col_specification = [(0, 20), (21, 30), (31, 50), (51, 100)]
            // data = pandas.read_fwf(path, colspecs=col_specification)

            // separator
            //import pandas
            //df = pandas.read_csv('hrdata.csv', 
            //            index_col='Employee', 
            //            parse_dates=['Hired'],
            //            sep=';',
            //            header=0, 
            //            names=['Employee', 'Hired', 'Salary', 'Sick Days'])
            //df.to_csv('hrdata_modified.csv')

            return "Python Panda";
        }

        /// <summary>
        /// generate SPSS code based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GenerateSPSS(string data)
        {
            return "SPSS";
        }

        /// <summary>
        /// generate SQL based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GenerateSQL(string data)
        {
            return "SQL";
        }

        /// <summary>
        /// generate schema.ini based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GenerateSchemaIni(string data)
        {
            return "schema.ini";
        }

        /// <summary>
        /// generate R-scripting code based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GenerateRScript(string data)
        {
            return "R script";
        }

        /// <summary>
        /// generate PowerShell script based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePowerShell(string data)
        {
            return "PowerShell";
        }

        /// <summary>
        /// generate PHP code based on columns
        /// </summary>
        /// <param name="data"> csv data </param>
        public string GeneratePHP(string data)
        {
            return "PHP";
        }
    }
}
