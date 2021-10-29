using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
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

        static string iniFilePath = null;
        static CsvLintWindow frmCsvLintDlg = null;
        static int idMyDlg = -1;
        static Bitmap tbBmp = CSVLintNppPlugin.Properties.Resources.csvlint;
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
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);

            // lexer xml file must exist
            CheckLexerXml(iniFilePath);

            // remember ini filename for later
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");

            // menu items
            //PluginBase.SetCommand(0, "MyMenuCommand", myMenuFunction, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(0, "CSV Lint window", myDockableDialog); idMyDlg = 0;
            PluginBase.SetCommand(1, "---", null);
            PluginBase.SetCommand(2, "Analyse data report", analyseDataReport);
            PluginBase.SetCommand(3, "Count unique values", CountUniqueValues);
            PluginBase.SetCommand(4, "Convert to SQL", convertToSQL);
            PluginBase.SetCommand(5, "---", null);
            PluginBase.SetCommand(6, "&Settings", Settings.ShowDialog);
            PluginBase.SetCommand(7, "About", doAboutForm);
        }

        internal static void CheckLexerXml(string iniFilePath)
        {
            var filename = Path.Combine(iniFilePath, "CSVLint.xml");
            // create language xml in plugin config directory if needed
            if (!File.Exists(filename))
            {
                if (!CreateLexerXML(filename)) {
                    var errmsg = string.Format("Unable to create {0}.xml in folder {1}", PluginName, iniFilePath);
                    MessageBox.Show(errmsg, "Error saving CSVLint.xml", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal static bool CreateLexerXML(string filename)
        {
            string[] tags = new string[] { "instre1", "instre2", "type1", "type2", "type3", "type4", "type5", "type6" };
            string[] colors = new string[] { "FFFFFF", "E0E0FF", "FFFF80", "FFE0FF", "80FF80", "FFB0FF", "FFC0C0", "32FFBE", "FFD040" };

            // Create an XmlWriterSettings object with the correct options.
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t"; //  "\t";
            settings.OmitXmlDeclaration = false;
            //settings.Encoding = System.Text.Encoding.UTF8; // NOTE: this results in UTF-8-BOM, Notepad++ can only read UTF-8 xml
            settings.Encoding = new UTF8Encoding(false); // The false means, do not emit the BOM.

            try
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(filename, settings))
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
                        writer.WriteAttributeString("ext", "");

                    for (int i = 0; i < colors.Length; i++)
                    {
                        writer.WriteStartElement("WordsStyle");
                        writer.WriteAttributeString("styleID", i.ToString());
                        writer.WriteAttributeString("name", (i == 0 ? "Default" : "ColumnColor" + i.ToString()));
                        writer.WriteAttributeString("fgColor", "000000");
                        writer.WriteAttributeString("bgColor", colors[i]);
                        writer.WriteAttributeString("fontName", "");
                        writer.WriteAttributeString("fontStyle", "0");
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                } // End Using writer 
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        public static void CSVChangeFileTab()
        {
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

        internal static void doAboutForm()
        {
            var about = new AboutForm();
            about.ShowDialog();
            about.Dispose();
        }

        internal static void convertToSQL()
        {
            // get dictionary
            CsvDefinition csvdef = GetCurrentCsvDef();

            CsvEdit.ConvertToSQL(csvdef);
        }

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
                bool sortValue = frmunq.sortValue;
                bool sortDesc = frmunq.sortDesc;

                // clear up
                frmunq.Dispose();

                // return true (OK) or false (Cancel)
                if (r == DialogResult.OK)
                {
                    // count unique values
                    CsvAnalyze.CountUniqueValues(csvdef, colidx, sortValue, sortDesc);
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