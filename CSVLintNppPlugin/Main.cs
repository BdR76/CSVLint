using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CSVLint;
using CSVLintNppPlugin.CsvLint;
using CSVLintNppPlugin.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "CSV Lint";
        public static Settings Settings = new Settings();
        public static CultureInfo dummyCulture;

        static string userConfigPath = null;
        static bool checkdarkmode = false;
        static CsvLintWindow frmCsvLintDlg = null;
        static int idMyDlg = -1;

        // toolbar icons
        static Bitmap tbBmp = CSVLintNppPlugin.Properties.Resources.csvlint;
        static Icon tbIco = CSVLintNppPlugin.Properties.Resources.csvlint_black_32;
        static Icon tbIcoDM = CSVLintNppPlugin.Properties.Resources.csvlint_white_32;


        static Bitmap tbBmp_tbTab = CSVLintNppPlugin.Properties.Resources.csvlint;
        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static Icon tbIcon = null;

        // list of files and csv definition for each
        static Dictionary<string, CsvDefinition> FileCsvDef = new Dictionary<string, CsvDefinition>();
        static CsvDefinition _CurrnetCsvDef;

        public static void OnNotification(ScNotification notification)
        {
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }

            // changing tabs
            if ((notification.Header.Code == (uint)NppMsg.NPPN_BUFFERACTIVATED) || (notification.Header.Code == (uint)NppMsg.NPPN_LANGCHANGED))
            {
                Main.CSVChangeFileTab();
            }

            // when closing a file
            if (notification.Header.Code == (uint)NppMsg.NPPN_FILEBEFORECLOSE)
            {
                Main.removeCSVdef();
            }
        }

        internal static void CommandMenuInit()
        {
            // config folder
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            userConfigPath = sbIniFilePath.ToString();
            if (!Directory.Exists(userConfigPath)) Directory.CreateDirectory(userConfigPath);

            // lexer xml file must exist
            TryCreateLexerXml(-1, false); // default: 0 = normal background, 2 = dark pastel

            // menu items
            //PluginBase.SetCommand(0, "MyMenuCommand", myMenuFunction, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(0, "CSV Lint window", myDockableDialog); idMyDlg = 0;
            PluginBase.SetCommand(1, "---", null);
            PluginBase.SetCommand(2, "Analyse data report", analyseDataReport);
            PluginBase.SetCommand(3, "Count unique values", CountUniqueValues);
            PluginBase.SetCommand(4, "Convert data to..", convertData);
            //PluginBase.SetCommand(5, "Generate metadata", convertData);
            PluginBase.SetCommand(5, "---", null);
            PluginBase.SetCommand(6, "&Settings", doSettings);
            PluginBase.SetCommand(7, "About / Help", doAboutForm);

            RefreshFromSettings();
        }

        internal static void RefreshFromSettings()
        {
            // the DateTime.TryParseExact requires a culture object, for much better performance DO NOT create on the fly for every call to EvaluateDateTime!
            var tmp = (CultureInfo)(CultureInfo.InvariantCulture.Clone());
            tmp.DateTimeFormat.Calendar.TwoDigitYearMax = Settings.intTwoDigitYearMax; // any cutoff you need
                                                                // incorrect: tmp.Calendar.TwoDigitYearMax = 2039
            dummyCulture = CultureInfo.ReadOnly(tmp);
        }

        internal static bool CheckConfigDarkMode()
        {
            string darkmodeenabled = "no";

            var xmlfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notepad++\\config.xml");
            try
            {
                XmlDocument config = new XmlDocument();
                config.Load(xmlfile);

                XmlNode darkmode = config.DocumentElement.SelectSingleNode("/NotepadPlus/GUIConfigs/GUIConfig[@name='DarkMode']");
                darkmodeenabled = ((darkmode != null) && (darkmode as XmlElement).HasAttribute("enable") ? darkmode.Attributes["enable"].Value : "no");
            }
            catch { };

            // return value
            return (darkmodeenabled == "yes");
        }

        internal static void TryCreateLexerXml(int presetidx, bool overwrite)
        {
            var filename = Path.Combine(userConfigPath, "CSVLint.xml");
            // create language xml in plugin config directory if needed
            if ((!File.Exists(filename)) || overwrite)
            {
                // when initially creating CSVLintxml for first time
                if (presetidx == -1)
                {
                    // initially give users random color preset, i.e. distribute random among all users, to see which they like best
                    var checkdarkmode = CheckConfigDarkMode();
                    var sec = DateTime.Now.Second % 2; // semi-random 0..1
                    presetidx = (checkdarkmode ? 2 : 0) + sec; // 0..1 for normal, 2..3 for dark mode
                }

                // create syntax color xml
                if (!CreateLexerXML(filename, presetidx))
                {
                    var errmsg = string.Format("Unable to create {0}.xml in folder {1}", PluginName, userConfigPath);
                    MessageBox.Show(errmsg, "Error saving CSVLint.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal static bool CreateLexerXML(string filename, int presetidx)
        {
            string[] presets = new string[] { "normal mode (background colors)", "normal mode (foreground colors)", "dark mode (pastel)", "dark mode (neon)" };
            string[] tags = new string[] { "instre1", "instre2", "type1", "type2", "type3", "type4", "type5", "type6" };

            string[] colors = new string[] {
                "000000", "FFFFFF", // normal colors
                "000000", "E0E0FF",
                "000000", "FFFF80",
                "000000", "FFE0FF",
                "000000", "80FF80",
                "000000", "FFB0FF",
                "000000", "FFC0C0",
                "000000", "32FFBE",
                "000000", "FFD040",
                "000000", "FFFFFF", // normal colors (foreground)
                "0000FF", "FFFFFF",
                "A0A000", "FFFFFF",
                "FF00FF", "FFFFFF",
                "00A000", "FFFFFF",
                "C000C0", "FFFFFF",
                "00C0A0", "FFFFFF",
                "C00000", "FFFFFF",
                "F07028", "FFFFFF",
                "DCDCCC", "3F3F3F", // dark mode (pastel)
                "9191D8", "3F3F3F",
                "BEC89E", "3F3F3F",
                "E0B8E0", "3F3F3F",
                "91C891", "3F3F3F",
                "C891C8", "3F3F3F",
                "D89191", "3F3F3F",
                "40C090", "3F3F3F",
                "C09040", "3F3F3F",
                "FFFFFF", "3F3F3F", // dark mode (neon)
                "D0D0FF", "000050",
                "FFFF80", "505000",
                "FFE0FF", "500050",
                "80FF80", "005000",
                "FFB0FF", "500050",
                "FFC0C0", "500000",
                "32FFBE", "005028",
                "FFD040", "502800"
            };

            // Create an XmlWriterSettings object with the correct options.
            System.Xml.XmlWriterSettings xmlsettings = new System.Xml.XmlWriterSettings();
            xmlsettings.Indent = true;
            xmlsettings.IndentChars = "\t"; //  "\t";
            xmlsettings.OmitXmlDeclaration = false;
            //settings.Encoding = System.Text.Encoding.UTF8; // NOTE: this results in UTF-8-BOM, Notepad++ can only read UTF-8 xml
            xmlsettings.Encoding = new UTF8Encoding(false); // The false means, do not emit the BOM.

            try
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(filename, xmlsettings))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("NotepadPlus");

                    // keywords
                    writer.WriteStartElement("Languages");
                    writer.WriteStartElement("Language");
                        writer.WriteAttributeString("name", "CSVLint");
                        writer.WriteAttributeString("ext", "csv");
                        writer.WriteAttributeString("commentLine", "#");
                        writer.WriteAttributeString("commentStart", "#[");
                        writer.WriteAttributeString("commentEnd", "]#");

                    for (int i = 0; i < tags.Length; i++)
                    {
                        writer.WriteStartElement("Keywords");
                        writer.WriteAttributeString("name", tags[i]);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    // colors
                    writer.WriteStartElement("LexerStyles");
                    writer.WriteStartElement("LexerType");
                        writer.WriteAttributeString("name", "CSVLint");
                        writer.WriteAttributeString("desc", "CSV Linter and validator");
                        writer.WriteAttributeString("excluded", "no");

                    var coloridx = 0;
                    for (int ps = 0; ps < presets.Length; ps++)
                    {
                        // comment preset name
                        writer.WriteComment(presets[ps]);

                        if (presetidx != ps) writer.WriteRaw("\r\n<!--");

                        for (int i = 0; i < 9; i++)
                        {
                            //writer.WriteStartElement("WordsStyle");
                            //writer.WriteAttributeString("styleID", i.ToString());
                                //writer.WriteAttributeString("name", (i == 0 ? "Default" : "ColumnColor" + i.ToString()));
                                //writer.WriteAttributeString("fgColor", "000000");
                                //writer.WriteAttributeString("bgColor", colors[i]);
                                //writer.WriteAttributeString("fontName", "");
                                //writer.WriteAttributeString("fontStyle", "0");
                            //writer.WriteEndElement();
                            var name = (i == 0 ? "Default" : "ColumnColor" + i.ToString());
                            var fgcolor = colors[coloridx];
                            var bgcolor = colors[coloridx + 1];
                            var bold = (ps == 0 || i == 0 ? "0" : "1");
                            var str = string.Format("\r\n\t\t\t<WordsStyle styleID=\"{0}\" name=\"{1}\" fgColor=\"{2}\" bgColor=\"{3}\" fontName=\"\" fontStyle=\"{4}\" />", i, name, fgcolor, bgcolor, bold);
                            writer.WriteRaw(str);

                            coloridx += 2;
                        }

                        if (presetidx != ps) writer.WriteRaw("\r\n-->");
                        writer.WriteRaw("\r\n\t\t");
                    };

                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                } // End Using writer 
            }
            catch
            {
                return false;
            }

            return true;
        }

        internal static void SetToolBarIcon()
        {
            // create struct
            toolbarIcons tbIcons = new toolbarIcons();

            // add bmp icon
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            tbIcons.hToolbarIcon = tbIco.Handle;            // icon with black lines
            tbIcons.hToolbarIconDarkMode = tbIcoDM.Handle;  // icon with light grey lines

            // convert to c++ pointer
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);

            // call Notepad++ api
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON_FORDARKMODE, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);

            // release pointer
            Marshal.FreeHGlobal(pTbIcons);
        }

        public static void CSVChangeFileTab()
        {
            // Notepad++ switch to a different file tab
            INotepadPPGateway notepad = new NotepadPPGateway();

            CsvDefinition csvdef;

            string filename = notepad.GetCurrentFilePath();

            // check if already in list
            if (!FileCsvDef.ContainsKey(filename))
            {
                // read schema.ini file
                var lines = CsvSchemaIni.ReadIniSection(filename);
                if (lines.Count > 0)
                {
                    // metadata from previously saved schema.ini
                    csvdef = new CsvDefinition(lines);
                }
                else
                {
                    // analyze and determine csv definition
                    csvdef = CsvAnalyze.InferFromData();
                }
                FileCsvDef.Add(filename, csvdef);
            }
            else
            {
                csvdef = FileCsvDef[filename];
            }

            // pass separator to lexer
            string sepchar = csvdef.Separator.ToString();
            string sepcol = (Settings.SeparatorColor ? "1" : "0");
            editor.SetProperty("separator", sepchar);
            editor.SetProperty("separatorcolor", sepcol);

            // if fixed width
            if ((csvdef.Separator == '\0') && (csvdef.FieldWidths != null))
            {
                // also pass column widths to lexer
                var strwidths = "";
                for (var i = 0; i < csvdef.FieldWidths.Count; i++)
                {
                    var w1 = csvdef.FieldWidths[i];
                    strwidths += (i > 0 ? "," : "") + csvdef.FieldWidths[i].ToString();
                }

                editor.SetProperty("fixedwidths", strwidths);
            }
            editor.SetIdleStyling(IdleStyling.ALL);

            // keep current csvdef
            _CurrnetCsvDef = FileCsvDef[filename];

            if (frmCsvLintDlg != null)
            {
                frmCsvLintDlg.SetCsvDefinition(csvdef);
            }
        }

        public static void updateCSVChanges(CsvDefinition csvdef, bool saveini)
        {
            // Notepad++ switc to a different file tab
            INotepadPPGateway notepad = new NotepadPPGateway();
            string filename = notepad.GetCurrentFilePath();

            // check if already in list
            if (!FileCsvDef.ContainsKey(filename))
            {
                // add csv definition
                FileCsvDef.Add(filename, csvdef);
            }
            else
            {
                // overwrite old csv definition with new
                FileCsvDef[filename] = csvdef;
            }

            // pass separator to lexer
            string sepchar = csvdef.Separator.ToString();
            string sepcol = (Settings.SeparatorColor ? "1" : "0");
            editor.SetProperty("separator", sepchar);
            editor.SetProperty("separatorcolor", sepcol);

            // if fixed width
            if ((csvdef.Separator == '\0') && (csvdef.FieldWidths != null))
            {
                var strwidths = "";
                for (var i = 0; i < csvdef.FieldWidths.Count; i++)
                {
                    var w1 = csvdef.FieldWidths[i];
                    strwidths += (i > 0 ? "," : "") + csvdef.FieldWidths[i].ToString();
                }

                editor.SetProperty("fixedwidths", strwidths);
            }
            editor.SetIdleStyling(IdleStyling.ALL);

            // also write to schema.ini file
            if (saveini)
            {
                if (!CsvSchemaIni.WriteIniSection(filename, csvdef.GetIniLines(), out string errmsg))
                {
                    MessageBox.Show(errmsg, "Error saving schema.ini", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public static CsvDefinition GetCurrentCsvDef()
        {
            // Notepad++ switc to a different file tab
            INotepadPPGateway notepad = new NotepadPPGateway();
            string filename = notepad.GetCurrentFilePath();

            // check if already in list
            if (FileCsvDef.ContainsKey(filename))
            {
                return FileCsvDef[filename];
            }

            return null;
        }

        public static void removeCSVdef()
        {
            // Notepad++ closes a file, also remove the definition from list
            INotepadPPGateway notepad = new NotepadPPGateway();
            string filename = notepad.GetCurrentFilePath();

            // check if in list
            if (FileCsvDef.ContainsKey(filename))
            {
                // remove csv definition
                FileCsvDef.Remove(filename);
            }
        }

        public static void GetCurrentFileLexerParameters(out char sep)
        {

            sep = ';';

            // Notepad++ switc to a different file tab
            INotepadPPGateway notepad = new NotepadPPGateway();

            CsvDefinition csvdef;

            string filename = notepad.GetCurrentFilePath();

            // check if already in list
            if (!FileCsvDef.ContainsKey(filename))
            {
                // read schema.ini file
                var lines = CsvSchemaIni.ReadIniSection(filename);
                if (lines.Count > 0)
                {
                    // metadata from previously saved schema.ini
                    csvdef = new CsvDefinition(lines);
                }
                else
                {
                    // analyze and determine csv definition
                    csvdef = CsvAnalyze.InferFromData();
                }

                FileCsvDef.Add(filename, csvdef);
            }
            else
            {
                csvdef = FileCsvDef[filename];
            }
            sep = csvdef.Separator;
        }

        internal static void PluginCleanUp()
        {
            // any clean up code here
        }
        internal static void doSettings()
        {
            Settings.ShowDialog();
            RefreshFromSettings();
        }

        internal static void doAboutForm()
        {
            var about = new AboutForm();
            about.ShowDialog();
            about.Dispose();
        }

        internal static void convertData()
        {
            // get dictionary
            CsvDefinition csvdef = GetCurrentCsvDef();

            // show split column dialog
            var frmparam = new DataConvertForm();
            frmparam.InitialiseSetting();

            DialogResult r = frmparam.ShowDialog();

            // clear up
            frmparam.Dispose();

            // return true (OK) or false (Cancel)
            if (r == DialogResult.OK)
            {
                switch (Main.Settings.DataConvertType)
                {
                    case 1: // XML
                        CsvEdit.ConvertToXML(csvdef);
                        break;
                    case 2: // JSON
                        CsvEdit.ConvertToJSON(csvdef);
                        break;
                    default: // case 0: SQL
                        CsvEdit.ConvertToSQL(csvdef);
                        break;
                }
            }
        }

        //internal static void generateMetaData()
        //{
        //    // get dictionary
        //    CsvDefinition csvdef = GetCurrentCsvDef();
        //
        //    // show metadata options
        //    var frmparam = new MetaDataGenerateForm();
        //    frmparam.InitialiseSetting();
        //
        //    DialogResult r = frmparam.ShowDialog();
        //
        //    // clear up
        //    frmparam.Dispose();
        //
        //    // return true (OK) or false (Cancel)
        //    if (r == DialogResult.OK)
        //    {
        //        switch (Main.Settings.MetadataGenerateType)
        //        {
        //            case 1: // schema JSON
        //                break;
        //            case 2: // Python script
        //                break;
        //            case 3: // R - script
        //                break;
        //            case 4: // SPSS syntax
        //                break;
        //            default: // case 0: schema ini
        //                break;
        //        }
        //    }
        //}

        internal static void analyseDataReport()
        {
            // get dictionary
            CsvDefinition csvdef = GetCurrentCsvDef();
            
            // check if valid dictionary
            if (csvdef.Fields.Count > 0)
            {
                // validate data
                CsvAnalyze.StatisticalReportData(csvdef);
            }
        }

        internal static void CountUniqueValues()
        {
            // get dictionary
            CsvDefinition csvdef = GetCurrentCsvDef();

            // check if valid dictionary
            if (csvdef.Fields.Count > 0)
            {
                // show unique values parameters form
                var frmunq = new UniqueValuesForm();
                frmunq.InitialiseSetting(csvdef);
                DialogResult r = frmunq.ShowDialog();

                // user clicked OK or Cancel
                List<int> colidx = new List<int>(frmunq.columnIndexes);
                bool sortBy = frmunq.sortBy;
                bool sortValue = frmunq.sortValue;
                bool sortDesc = frmunq.sortDesc;

                // clear up
                frmunq.Dispose();

                // return true (OK) or false (Cancel)
                if (r == DialogResult.OK)
                {
                    // count unique values
                    CsvAnalyze.CountUniqueValues(csvdef, colidx, sortBy, sortValue, sortDesc);
                }
            }
        }

        internal static void myDockableDialog()
        {
            if (frmCsvLintDlg == null)
            {
                // show dialog for first time
                frmCsvLintDlg = new CsvLintWindow();

                // icon
                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                // dockable window struct data
                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = frmCsvLintDlg.Handle;
                _nppTbData.pszName = "CSV Lint";
                _nppTbData.dlgID = idMyDlg;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_BOTTOM | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                // register as dockable window
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            }
            else
            {
                // toggle on/off
                if (!frmCsvLintDlg.Visible)
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, frmCsvLintDlg.Handle); // show
                else
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMHIDE, 0, frmCsvLintDlg.Handle); // hide
            }

            // immediately show currnet csv metadata when activated
            CSVChangeFileTab();
        }
        public static string GetVersion()
        {
            // version for example "1.3.0.0"
            String ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // if 4 version digits, remove last two ".0" if any, example  "1.3.0.0" ->  "1.3" or  "2.0.0.0" ->  "2.0"
            while ((ver.Length > 4) && (ver.Substring(ver.Length - 2, 2) == ".0"))
            {
                ver = ver.Substring(0, ver.Length - 2);
            }
            return ver;
        }
    }
}