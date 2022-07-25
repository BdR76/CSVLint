using CSVLint;
using CSVLintNppPlugin.Forms;
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class CsvLintWindow : Form
    {
        public CsvLintWindow()
        {
            InitializeComponent();
            chkAutoDetect.Checked = Main.Settings.AutoDetectColumns;
        }

        public void SetCsvDefinition(CsvDefinition csvdef)
        {
            // display csv definition
            txtSchemaIni.Text = csvdef.GetIniLines();
            btnApply.Enabled = false;
        }

        private void OnBtnDetectColumns_Click(object sender, EventArgs e)
        {
            bool usercancel = false;
            char sep = '\0';
            bool header = false;

            // manual override auto-detect
            if (!chkAutoDetect.Checked) {
                bool ok = GetDetectColumnsParameters(out sep, out header);
                usercancel = !ok;
            }

            if (!usercancel)
            {
                // clear any previous output
                txtOutput.Clear();
                txtOutput.Update();

                var dtStart = DateTime.Now;

                // analyze and determine csv definition
                CsvDefinition csvdef = CsvAnalyze.InferFromData(chkAutoDetect.Checked, sep, header);

                Main.UpdateCSVChanges(csvdef, false);

                var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

                // display csv definition
                txtSchemaIni.Text = csvdef.GetIniLines();
                btnApply.Enabled = true;

                txtOutput.Clear();
                txtOutput.Text = string.Format("Detecting columns from data is ready, time elapsed {0}", dtElapsed);
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

                var sr = ScintillaStreams.StreamAllText();

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
                int lineEnd = log.Text.IndexOf(':', position);
                if (lineEnd >= 0)
                {
                    // get part between "line" and ":"
                    position += 13; // "** error line " is 14 characters
                    string logline = log.Text.Substring(position, lineEnd - position);

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

        private bool GetDetectColumnsParameters(out char sep, out bool header)
        {
            // show manually detect columns parameters form
            var frmdetect = new DetectColumnsForm();
            frmdetect.InitialiseSetting();
            DialogResult r = frmdetect.ShowDialog();

            // user clicked OK or Cancel
            sep = frmdetect.Separator;
            header = frmdetect.HeaderNames;

            // clear up
            frmdetect.Dispose();

            // return true (OK) or false (Cancel)
            return r == DialogResult.OK;
        }

        private bool GetReformatParameters(out string dt, out string dec, out string sep, out bool updsep, out int updquote, out string replacecrlf, out bool trimall, out bool alignVert)
        {
            // show reformat form
            var frmedit = new ReformatForm();
            frmedit.InitialiseSetting();
            DialogResult r = frmedit.ShowDialog();

            // user clicked OK or Cancel
            dt = frmedit.NewDataTime;
            dec = frmedit.NewDecimal;
            sep = frmedit.NewSeparator;
            updsep = frmedit.UpdateSeparator;
            updquote = frmedit.ApplyQuotes;
            replacecrlf = frmedit.ReplaceCrLf;
            trimall = frmedit.TrimAllValues;
            alignVert = frmedit.alignVertically;

            // clear up
            frmedit.Dispose();

            // return true (OK) or false (Cancel)
            return r == DialogResult.OK;
        }

        private void OnBtnReformat_Click(object sender, EventArgs e)
        {
            bool ok = GetReformatParameters(out string editDataTime, out string editDecimal, out string editSeparator, out bool updateSeparator, out int updateQuotes, out string replaceCrLf, out bool trimAllValues, out bool alignVert);
            if (ok)
            {
                // clear any previous output
                txtOutput.Clear();
                txtOutput.Update();

                // get dictionary
                CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

                var dtStart = DateTime.Now;

                // analyze and determine csv definition
                CsvEdit.ReformatDataFile(csvdef, editDataTime, editDecimal, editSeparator, updateSeparator, updateQuotes, replaceCrLf, trimAllValues, alignVert);

                var dtElapsed = (DateTime.Now - dtStart).ToString(@"hh\:mm\:ss\.fff");

                string msg = "";

                // refresh datadefinition
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

                //if (updateQuotes != 0)
                //{
                    var quotetxt = "None / Minimal";
                    if (updateQuotes == 1) quotetxt = "Values with spaces";
                    if (updateQuotes == 2) quotetxt = "All string values";
                    if (updateQuotes == 3) quotetxt = "All non-numeric values";
                    if (updateQuotes == 4) quotetxt = "All values";

                    msg += string.Format("Reformat apply quotes: {0}\r\n", quotetxt);
                //};

                if (trimAllValues)
                {
                    msg += "Trim all values\r\n";
                }

                // display process message
                msg += string.Format("Reformat data is ready, time elapsed {0}\r\n", dtElapsed);
                txtOutput.Text = msg;

                // refresh datadefinition
                txtSchemaIni.Text = csvdef.GetIniLines();
                btnApply.Enabled = true;
                //btnApply.Enabled = true;
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

            // update the master list of csv definitions
            Main.UpdateCSVChanges(csvdef, true);

            // update screen, to smooth out any user input errors
            SetCsvDefinition(csvdef);
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            // get csv definition
            CsvDefinition csvdef = new CsvDefinition(txtSchemaIni.Text);

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
                if (cod == 1) msg = "invalid values";
                if (cod == 2) msg = "character " + par1;
                if (cod == 3) msg = "position " + par1;
                if (cod == 4) msg = "when contains " + par1;
                if (cod == 5) msg = "decode multiple value " + par1;
                msg = string.Format("Column \"{0}\" was split on \"{1}\"\r\n", csvdef.Fields[idx].Name, msg);

                // display process message
                msg += string.Format("Split column is ready, time elapsed {0}\r\n", dtElapsed);
                txtOutput.Text = msg;

                // refresh datadefinition
                OnBtnDetectColumns_Click(sender, e);
            }
        }

        private void chkAutoDetect_Click(object sender, EventArgs e)
        {
            Main.Settings.AutoDetectColumns = chkAutoDetect.Checked;
            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
