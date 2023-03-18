using CSVLint;
using Kbg.NppPluginNET;
using System;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class ColumnSplitForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public ColumnSplitForm()
        {
            InitializeComponent();
        }
        public int SplitCode { get; set; }
        public int SplitColumn { get; set; }
        public string SplitParam1 { get; set; }
        public string SplitParam2 { get; set; }
        public bool SplitRemove { get; set; }

        public void InitialiseSetting(CsvDefinition csvdef)
        {
            // add all columns to list
            var idx = 0;
            for (var i = 0; i < csvdef.Fields.Count; i++)
            {
                var str = csvdef.Fields[i].Name;
                cmbSelectColumn.Items.Add(str);
                if (str == Main.Settings.SplitColName) idx = cmbSelectColumn.Items.Count - 1;
            }

            // default select "(select column)" item
            cmbSelectColumn.SelectedIndex = idx;

            // which option selected
            rdbtnPadChar.Checked        = (Main.Settings.SplitOption <= 0); // if previously -1 (=nothing selected) then reset to default
            rdbtnSearchReplace.Checked  = (Main.Settings.SplitOption == 1);
            rdbtnSplitValid.Checked     = (Main.Settings.SplitOption == 2);
            rdbtnSplitCharacter.Checked = (Main.Settings.SplitOption == 3);
            rdbtnSplitSubstring.Checked = (Main.Settings.SplitOption == 4);

            // load user preferences
            txtPadChar.Text           = Main.Settings.EditColPad;
            numPadLength.Value        = Main.Settings.EditColPadLength;
            txtSearch.Text            = Main.Settings.EditColSearch;
            txtReplace.Text           = Main.Settings.EditColReplace;
            txtSplitCharacter.Text    = Main.Settings.SplitChar;
            numSplitSubstring.Value   = Main.Settings.SplitPos;

            chkDeleteOriginal.Checked = Main.Settings.SplitRemoveOrg;
        }

        private void cmbSelectColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvaluateOkButton();
        }

        private void OnRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as RadioButton).Checked;
            ToggleControlBasedOnControl((sender as RadioButton), chk);

            EvaluateOkButton();
        }
        private void EvaluateOkButton()
        {
            // can not press OK when nothing selected
            btnOk.Enabled =  (   (cmbSelectColumn.SelectedIndex > 0) // column must be selected and
                              && (   rdbtnPadChar.Checked            // either one of the radio options
                                  || rdbtnSearchReplace.Checked
                                  || rdbtnSplitValid.Checked
                                  || rdbtnSplitCharacter.Checked
                                  || rdbtnSplitSubstring.Checked
                                  || chkDeleteOriginal.Checked)     // or remove column must be checked (else there's nothing to do)
                                  );
        }

        private void ColumnSplitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass split parameters to previous form
            SplitCode = 0;
            if (rdbtnPadChar.Checked)        SplitCode = 1;
            if (rdbtnSearchReplace.Checked)  SplitCode = 2;
            if (rdbtnSplitValid.Checked)     SplitCode = 3;
            if (rdbtnSplitCharacter.Checked) SplitCode = 4;
            if (rdbtnSplitSubstring.Checked) SplitCode = 5;

            // which column index
            SplitColumn = cmbSelectColumn.SelectedIndex - 1; // zero based, first item is a dummy

            // extra parameters
            SplitParam1 = "";
            SplitParam2 = "";
            if (rdbtnPadChar.Checked)       { SplitParam1 = txtPadChar.Text; SplitParam2 = numPadLength.Value.ToString(); }
            if (rdbtnSearchReplace.Checked) { SplitParam1 = txtSearch.Text; SplitParam2 = txtReplace.Text; }
            if (rdbtnSplitCharacter.Checked)  SplitParam1 = txtSplitCharacter.Text;
            if (rdbtnSplitSubstring.Checked)  SplitParam2 = numSplitSubstring.Value.ToString();

            // remove original column
            SplitRemove = (chkDeleteOriginal.Checked);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.SplitColName = cmbSelectColumn.Items[cmbSelectColumn.SelectedIndex].ToString(); ;

            // pass split parameters to previous form
            var opt = -1;
            if (rdbtnPadChar.Checked)        opt = 0;
            if (rdbtnSearchReplace.Checked)  opt = 1;
            if (rdbtnSplitValid.Checked)     opt = 2;
            if (rdbtnSplitCharacter.Checked) opt = 3;
            if (rdbtnSplitSubstring.Checked) opt = 4;
            Main.Settings.SplitOption = opt;

            Main.Settings.EditColPad       = txtPadChar.Text;
            Main.Settings.EditColPadLength = Convert.ToInt32(numPadLength.Value);
            Main.Settings.EditColSearch    = txtSearch.Text;
            Main.Settings.EditColReplace   = txtReplace.Text;
            Main.Settings.SplitChar        = txtSplitCharacter.Text;
            Main.Settings.SplitPos         = Convert.ToInt32(numSplitSubstring.Value);

            Main.Settings.SplitRemoveOrg = chkDeleteOriginal.Checked;

            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void rdbtns_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                (sender as RadioButton).Checked = false;
                EvaluateOkButton();
            }
        }

        private void chkDeleteOriginal_CheckedChanged(object sender, EventArgs e)
        {
            EvaluateOkButton();
        }
    }
}
