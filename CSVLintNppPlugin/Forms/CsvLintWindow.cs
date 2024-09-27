using CSVLint;
using CSVLintNppPlugin.Forms;
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class CsvLintWindow : DockingFormBase
    {
        public CsvLintWindow()
        {
            InitializeComponent();
            chkAutoDetect.Checked = Main.Settings.AutoDetectColumns;
        }

        public void SetCsvDefinition(CsvDefinition csvdef, bool applybtn)
        {
            // clear message to user when no columns found
            var msg = "";
            if (csvdef.FileIsTooBig)
            {
                msg += "; *********************************\r\n";
                msg += "; File is too large for CsvLint to analyze\r\n";
                msg += "; *********************************\r\n";
            }
            else if ((csvdef.Fields.Count == 1) && (csvdef.Fields[0].DataType == ColumnType.String) && (csvdef.Fields[0].MaxWidth >= 9999))
            {
                // give a clear message
                msg += "; *********************************\r\n";
                msg += "; Unable to detect column separator\r\n";
                msg += "; *********************************\r\n";
            }
            // display csv definition
            txtSchemaIni.Text = msg + csvdef.GetIniLines();
            btnApply.Enabled = applybtn;
        }

        private INotepadPPGateway notepad;
        private IntPtr theme_ptr;

        public void ToggleDarkLightTheme(bool isDark)
        {
            // get ptr to Notepad and darkmode theme
            notepad = new NotepadPPGateway();
            theme_ptr = notepad.GetDarkModeColors();

            // start recursive iteration with this form
            ApplyThemeColors(this, isDark);

            Marshal.FreeHGlobal(theme_ptr);
            notepad = null;
        }

        private void OnBtnDetectColumns_Click(object sender, EventArgs e)
        {
            bool okstart = true;
            char sep = '\0';
            string widths = "";
            bool header = false;
            int skip = 0;
            char comm = Main.Settings.CommentCharacter;

            // manual override auto-detect
            if (!chkAutoDetect.Checked) {
                okstart = GetDetectColumnsParameters(out sep, out widths, out header, out skip, out comm);
            }

            // start auto-detect or manual-detect
            if (okstart)
            {
                // clear any previous output
                if (sender != btnSplit) {
                    txtOutput.Clear();
                    txtOutput.Update();
                }

                var dtStart = DateTime.Now;

                // analyze and determine csv definition
                CsvDefinition csvdef = CsvAnalyze.InferFromData(chkAutoDetect.Checked, sep, widths, header, skip, comm, true);

                Main.UpdateCSVChanges(csvdef, false);

                var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

                // display csv definition
                SetCsvDefinition(csvdef, true);

                // exception don't overwrite Split log messages
                if (sender != btnSplit) {
                    txtOutput.Text = string.Format("Detecting columns from data is ready, time elapsed {0}", dtElapsed);
                }
            }
        }

        private void OnBtnValidate_Click(object sender, EventArgs e)
        {
            // clear any previous output
            txtOutput.Clear();
            txtOutput.Update();

            // get dictionary
            CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

            // check if valid dictionary
            if (csvdef.Fields.Count > 0)
            {
                // validate data
                CsvValidate csvval = new CsvValidate();

                if (!ScintillaStreams.TryStreamAllText(out var sr))
                    return;

                csvval.ValidateData(sr, csvdef);

                sr.Dispose();

                // display output message or errors
                string msg = csvval.Report();
                txtOutput.Text = msg;
            }
        }

        private void OnTxtOutput_DoubleClick(object sender, EventArgs e)
        {
            // double click on text box to jump to line in editor, 
            // Note: should fix this q&d solution, log and ui should be properly decoupled

            // variables
            int linenumber = 0;
            TextBox log = sender as TextBox;
            string errval = "";

            // determine current line number in textbox
            int lineNumber = log.GetLineFromCharIndex(log.SelectionStart);
            int position = log.GetFirstCharIndexFromLine(lineNumber);

            // error check
            if (position >= 0)
            {
                // log line always "** error line 123: error in.. etc.", check for ':'
                int lineStart = log.Text.IndexOf("** error line", position);
                int lineEnd = log.Text.IndexOf(':', position);
                if ( (lineStart >= 0) && (lineEnd >= 0) && (lineStart < lineEnd) )
                {
                    // get part between "line" and ":"
                    //position += 13; // "** error line " is 14 characters
                    string logline = log.Text.Substring(lineStart + 13, lineEnd - lineStart - 13);

                    int.TryParse(logline, out linenumber);

                    // find error value
                    position = log.Text.IndexOf(" value \"", lineEnd);
                    if (position >= 0)
                    {
                        position += 8; // " value \"" is 8 characters
                        lineEnd = log.Text.IndexOf('"', position);
                        if (lineEnd >= 0)
                        {
                            errval = log.Text.Substring(position, lineEnd - position);
                        }
                    }
                }
            }

            // editor jump to line
            if (linenumber > 0)
            {
                // interface to Notepad++
                ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;
                scintillaGateway.GotoLine(linenumber - 1); // zero-based index

                // set target to current line
                scintillaGateway.SetTargetStart(Math.Max(scintillaGateway.GetCurrentPos(), scintillaGateway.GetAnchor()));
                scintillaGateway.SetTargetEnd(scintillaGateway.GetLineEndPosition(linenumber - 1));

                // search in line and SetSelection to the error value
                int selpos = scintillaGateway.SearchInTarget(errval.Length, errval);
                if (selpos != -1) {
                    scintillaGateway.SetSelection(selpos, selpos + errval.Length);
                }
            }
        }

        private bool GetDetectColumnsParameters(out char sep, out string widths, out bool header, out int skip, out char comm)
        {
            // show manually detect columns parameters form
            var frmdetect = new DetectColumnsForm();
            DialogResult r = frmdetect.ShowDialog();

            // user clicked OK or Cancel
            sep = frmdetect.Separator;
            widths = frmdetect.ManWidths;
            header = frmdetect.HeaderNames;
            skip = frmdetect.SkipLines;
            comm = frmdetect.CommentChar;

            // clear up
            frmdetect.Dispose();

            // return true (OK) or false (Cancel)
            return r == DialogResult.OK;
        }

        private bool GetReformatParameters(out string sep, out bool updsep, out string dt, out string dec, out string replacecrlf, out bool alignVert)
        {
            // show reformat form
            var frmedit = new ReformatForm();
            DialogResult r = frmedit.ShowDialog();

            // user clicked OK or Cancel
            sep = frmedit.NewSeparator;
            updsep = frmedit.UpdateSeparator;
            dt = frmedit.NewDataTime;
            dec = frmedit.NewDecimal;
            replacecrlf = frmedit.ReplaceCrLf;
            alignVert = frmedit.alignVertically;

            // clear up
            frmedit.Dispose();

            // return true (OK) or false (Cancel)
            return r == DialogResult.OK;
        }

        private void OnBtnReformat_Click(object sender, EventArgs e)
        {
            bool ok = GetReformatParameters(out string editSeparator, out bool updateSeparator, out string editDataTime, out string editDecimal, out string replaceCrLf, out bool alignVert);
            if (ok)
            {
                // clear any previous output
                txtOutput.Clear();
                txtOutput.Update();

                // get dictionary
                CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

                // check if valid dictionary
                if (csvdef.Fields.Count > 0)
                {
                    var dtStart = DateTime.Now;

                    // analyze and determine csv definition
                    CsvEdit.ReformatDataFile(csvdef, editSeparator, updateSeparator, editDataTime, editDecimal, replaceCrLf, alignVert);

                    var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

                    // display the reformat parameters to user
                    string msg = "";

                    if (updateSeparator)
                    {
                        string oldsep = csvdef.Separator == '\0' ? "{Fixed width}" : (csvdef.Separator == '\t' ? "{Tab}" : csvdef.Separator.ToString());
                        string newsep = editSeparator == "\0" ? "{Fixed width}" : (editSeparator == "\t" ? "{Tab}" : editSeparator);

                        // fixed width doesn't contain header line (for example, columns can be width=1, no room in header line for column name longer than 1 character)
                        if (editSeparator == "\0")
                        {
                            csvdef.ColNameHeader = false;
                        }

                        msg += string.Format("Reformat column separator from {0} to {1}\r\n", oldsep, newsep);
                        csvdef.Separator = editSeparator[0];
                    };

                    if (editDataTime != "")
                    {
                        msg += string.Format("Reformat datetime format from \"{0}\" to \"{1}\"\r\n", csvdef.DateTimeFormat, editDataTime);
                        csvdef.DateTimeFormat = editDataTime;
                        foreach (var col in csvdef.Fields)
                        {
                            if (col.DataType == ColumnType.DateTime) col.UpdateDateTimeMask(editDataTime);
                        }
                    };

                    if (editDecimal != "")
                    {
                        msg += string.Format("Reformat decimal separator from {0} to {1}\r\n", csvdef.DecimalSymbol, editDecimal);
                        csvdef.DecimalSymbol = editDecimal[0];
                    };

                    if (Main.Settings.TrimValues || (Main.Settings.ReformatQuotes > 0))
                    {
                        msg += "General settings: ";

                        if (Main.Settings.TrimValues)
                        {
                            msg += "Trim all values";
                        }

                        if (Main.Settings.ReformatQuotes > 0)
                        {
                            var quotetxt = "None / Minimal";
                            if (Main.Settings.ReformatQuotes == 1) quotetxt = "Values with spaces";
                            if (Main.Settings.ReformatQuotes == 2) quotetxt = "All string values";
                            if (Main.Settings.ReformatQuotes == 3) quotetxt = "All non-numeric values";
                            if (Main.Settings.ReformatQuotes == 4) quotetxt = "All values";

                            if (Main.Settings.TrimValues) msg += ", ";
                            msg += string.Format("apply quotes: {0}", quotetxt);
                        };
                        msg += "\r\n";
                    }

                    // display process message
                    msg += string.Format("Reformat data is ready, time elapsed {0}\r\n", dtElapsed);
                    txtOutput.Text = msg;

                    // update the master list of csv definitions
                    Main.UpdateCSVChanges(csvdef, false);
                    // refresh datadefinition
                    SetCsvDefinition(csvdef, true);
                    //btnApply.Enabled = true;
                }
            }
        }

        private void txtSchemaIni_KeyDown(object sender, KeyEventArgs e)
        {
            var test = ((TextBox)sender).Modified;
            btnApply.Enabled = test;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // get dictionary
            txtSchemaIni.Modified = false;
            CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

            // check if valid dictionary
            if (csvdef.Fields.Count > 0)
            {
                // update the master list of csv definitions
                Main.UpdateCSVChanges(csvdef, true);

                // update screen, to smooth out any user input errors
                SetCsvDefinition(csvdef, false);
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            // get csv definition
            CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

            // check if valid dictionary
            if (csvdef.Fields.Count > 0)
            {
                // show split column dialog
                var frmsort = new SortForm();
                frmsort.InitialiseSetting(csvdef);
                DialogResult r = frmsort.ShowDialog();

                // user clicked OK or Cancel
                int idx = frmsort.SortColumn;
                bool asc = frmsort.SortAscending;
                bool val = frmsort.SortValue;

                // clear up
                frmsort.Dispose();

                // return true (OK) or false (Cancel)
                if (r == DialogResult.OK)
                {
                    // clear any previous output
                    txtOutput.Clear();
                    txtOutput.Update();

                    var dtStart = DateTime.Now;

                    // split column
                    CsvEdit.SortData(csvdef, idx, asc, val);

                    var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

                    // display process message
                    var colname = csvdef.Fields[idx].Name;
                    txtOutput.Text = string.Format("Sort data {0}scending on column '{1}' is ready, time elapsed {2}\r\n", (asc ? "a" : "de"), colname, dtElapsed); ;

                    // refresh datadefinition
                    //OnBtnDetectColumns_Click(sender, e);
                }
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            // get csv definition
            CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

            // check if valid dictionary
            if (csvdef.Fields.Count > 0)
            {
                // show split column dialog
                var frmsplit = new ColumnSplitForm();
                frmsplit.InitialiseSetting(csvdef);
                DialogResult r = frmsplit.ShowDialog();

                // user clicked OK or Cancel
                int cod = frmsplit.SplitCode;
                int idx = frmsplit.SplitColumn;
                string par1 = frmsplit.SplitParam1;
                string par2 = frmsplit.SplitParam2;
                bool rem = frmsplit.SplitRemove;

                // clear up
                frmsplit.Dispose();

                // return true (OK) or false (Cancel)
                if (r == DialogResult.OK)
                {
                    // clear any previous output
                    txtOutput.Clear();
                    txtOutput.Update();

                    var dtStart = DateTime.Now;

                    // split column
                    CsvEdit.ColumnSplit(csvdef, idx, cod, par1, par2, rem);

                    var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

                    // ready message
                    string msg = "";
                    if (cod == 1) msg = "was padded with \"" + par1 + "\"";
                    if (cod == 2) msg = "search \"" + par1 + "\" replace with \"" + par2 + "\"";
                    if (cod == 3) msg = "was split on valid and invalid values";
                    if (cod == 4) msg = "was split on character " + par1;
                    if (cod == 5) msg = "was split on position " + par1;
                    if (rem)      msg = msg + (msg == "" ? "" : "and original column ") + "was removed";
                    msg = string.Format("Column \"{0}\" {1}\r\n", csvdef.Fields[idx].Name, msg);

                    // display process message
                    msg += string.Format("Add new column is ready, time elapsed {0}\r\n", dtElapsed);
                    txtOutput.Text = msg;

                    // refresh datadefinition
                    OnBtnDetectColumns_Click(sender, e);
                }
            }
        }

        private void chkAutoDetect_Click(object sender, EventArgs e)
        {
            Main.Settings.AutoDetectColumns = chkAutoDetect.Checked;
            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void btnEnableDisable_Click(object sender, EventArgs e)
        {
            Main.EnableDisableLanguage();
        }

        private void CsvLintWindow_VisibleChanged(object sender, EventArgs e)
        {
            // CSV Lint window activated
            if (Visible)
            {
                // set font
                txtSchemaIni.Font = Main.Settings.FontDock;
                txtOutput.Font = Main.Settings.FontDock;
            }
        }

        // NOTE: this function is called recursively
        private void ApplyThemeColors(Control ctrl, bool isDark)
        {
            // darkmode or not
            DarkModeColors theme = (DarkModeColors)Marshal.PtrToStructure(theme_ptr, typeof(DarkModeColors)); // TODO: testing but THIS IS NOT CORRECT
            //if (isDark) theme = (DarkModeColors)Marshal.PtrToStructure(theme_ptr, typeof(DarkModeColors));

            // check for all control types
            if (ctrl.GetType() == typeof(Label))
            {
                ((Label)ctrl).BackColor = (!isDark ? Label.DefaultBackColor : NppDarkMode.BGRToColor(theme.PureBackground));
                ((Label)ctrl).ForeColor = (!isDark ? Label.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
            }

            // check for all control types
            if (ctrl.GetType() == typeof(LinkLabel))
            {
                ((LinkLabel)ctrl).LinkColor = (!isDark ? Color.Blue : NppDarkMode.BGRToColor(theme.LinkText));
                ((LinkLabel)ctrl).VisitedLinkColor = (!isDark ? Color.DarkMagenta : NppDarkMode.BGRToColor(theme.LinkText));
            }

            // ButtonBase == Button, RadioButton, CheckBox
            //if (ctrl is System.Windows.Forms.ButtonBase)
            if (ctrl is System.Windows.Forms.RadioButton || ctrl is System.Windows.Forms.CheckBox)
            {
                ((ButtonBase)ctrl).BackColor = (!isDark ? ButtonBase.DefaultBackColor : NppDarkMode.BGRToColor(theme.PureBackground));
                ((ButtonBase)ctrl).ForeColor = (!isDark ? ButtonBase.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
            }

            // edit box
            if (ctrl is System.Windows.Forms.TextBox)
            {
                ((TextBox)ctrl).BackColor = (!isDark ? TextBox.DefaultBackColor : NppDarkMode.BGRToColor(theme.SofterBackground));
                ((TextBox)ctrl).ForeColor = (!isDark ? TextBox.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
                // exception for schema.ini textbox
                if (ctrl.Name == "txtSchemaIni" && !isDark) ((TextBox)ctrl).BackColor = Color.FromKnownColor(KnownColor.Window);
            }

            // ListBox and ComboBox
            if (ctrl is System.Windows.Forms.ListControl)
            {
                ((ListControl)ctrl).BackColor = (!isDark ? Label.DefaultBackColor : NppDarkMode.BGRToColor(theme.SofterBackground));
                ((ListControl)ctrl).ForeColor = (!isDark ? Label.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
            }

            if (ctrl.GetType() == typeof(Button))
            {
                ((Button)ctrl).BackColor = (!isDark ? Color.FromKnownColor(KnownColor.ControlLight) : NppDarkMode.BGRToColor(theme.SofterBackground));
                ((Button)ctrl).ForeColor = (!isDark ? Color.FromKnownColor(KnownColor.ControlText) : NppDarkMode.BGRToColor(theme.Text));
                ((Button)ctrl).UseVisualStyleBackColor = !isDark;
            }

            // dialog form or panel/groupbox
            if (ctrl is System.Windows.Forms.Form || ctrl is System.Windows.Forms.GroupBox)
            {
                ((Control)ctrl).BackColor = (!isDark ? Control.DefaultBackColor : NppDarkMode.BGRToColor(theme.PureBackground));
                ((Control)ctrl).ForeColor = (!isDark ? Control.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
            }

            // recursively call for child controls
            if (ctrl.HasChildren)
            {
                foreach (Control c in ctrl.Controls)
                {
                    ApplyThemeColors(c, isDark);
                }
            }
        }

    }
}
