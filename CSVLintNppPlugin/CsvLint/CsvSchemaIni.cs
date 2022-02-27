using System;
using System.Collections.Generic;
using System.IO;

namespace CSVLintNppPlugin.CsvLint
{
    class CsvSchemaIni
    {
        /// <summary>
        /// the name of the file to store the schema informations
        /// </summary>
        /// <remarks>all schema informations of edited files of
        /// any directory will be stored in the schema.ini of
        /// that directory</remarks>
        private const string INI_NAME = "schema.ini";

        /// <summary>
        /// Reads the CSV definition for the given file.
        /// </summary>
        /// <param name="filePath">the full name of the file to be edited</param>
        /// <returns>the contents of the schema.ini in the directory
        /// of the given filename</returns>
        public static Dictionary<string, string> ReadIniSection(string filePath)
        {
            // file name and path of the file to be edited
            string path = Path.GetDirectoryName(filePath);
            string file = Path.GetFileName(filePath);

            // schema.ini and section name
            string inifile = Path.Combine(path, INI_NAME);
            string section = string.Format("[{0}]", file.ToLower());

            // read entire ini file and look for section
            var inilines = new Dictionary<string, string>();

            // not a new file that hasn't been saved yet
            if ( (!string.IsNullOrEmpty(path)) && File.Exists(inifile) )
            {
                using (StreamReader reader = new StreamReader(inifile))
                {
                    string line;
                    bool bSec = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line)) continue;
                        // check section
                        if (line[0] == '[')
                        {
                            // check current section and inilines index
                            bSec = line.ToLower() == section;
                        }
                        else if (bSec)
                        {
                            var spl = line.Split('=');
                            var key = line;
                            var val = "";
                            if (spl.Length > 1)
                            {
                                key = spl[0];
                                val = spl[1];
                            }

                            inilines.Add(key, val);
                        }
                    }
                }
            }

            return inilines;
        }

        /// <summary>
        /// Writes the given CSV definition for the given file.
        /// </summary>
        /// <param name="filePath">the full name of the edited file</param>
        /// <param name="inikeys">the textual representation of the CSV definition</param>
        /// <param name="errmsg">an error message if an error occured else empty string</param>
        /// <returns>true when no error occured</returns>
        public static bool WriteIniSection(string filePath, string inikeys, out string errmsg)
        {
            errmsg = string.Empty;
            // file name and path of the edited file
            string path = Path.GetDirectoryName(filePath);
            string file = Path.GetFileName(filePath);

            // schema.ini and section name
            string inifile = Path.Combine(path, INI_NAME);
            string section = string.Format("[{0}]", file.ToLower());

            // read entire ini file and look for section
            var inilines = new List<string>();
            int idx = -1;

            // check if schema.ini exists
            if (File.Exists(inifile))
            {
                using (StreamReader reader = new StreamReader(inifile))
                {
                    string line;
                    bool bSec = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            // check section
                            if (line[0] == '[')
                            {
                                // check current section and inilines index
                                bSec = line.ToLower() == section;
                                if (bSec) idx = inilines.Count;
                            }
                        }

                        // any other lines; just copy verbatim
                        if (!bSec) inilines.Add(line);
                    }
                }
            }

            // append any new sections at end
            if (idx == -1)
            {
                if ((inilines.Count > 0) && (inilines[inilines.Count - 1] != "")) inilines.Add("");
                idx = inilines.Count;
            }

            // section header
            inilines.Insert(idx + 0, string.Format("[{0}]", file));
            inilines.Insert(idx + 1, inikeys);
            //inilines.Insert(idx + 2, ""); // add one more empty line to separate next section

            // overwrite schema.ini file; `using` will implicitly do .Close() .Dispose() at the end
            try
            {
                using (StreamWriter writer = new StreamWriter(inifile, false))
                {
                    foreach (var str in inilines)
                    {
                        writer.WriteLine(str);
                    }
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                // error
                errmsg = e.Message;
                return false;
            }

            // ok
            return true;
        }
    }
}
