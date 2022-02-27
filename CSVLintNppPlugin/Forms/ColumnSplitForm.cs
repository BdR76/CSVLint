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
            rdbtnSplitValid.Checked = (Main.Settings.SplitOption == 0);
            rdbtnSplitCharacter.Checked = (Main.Settings.SplitOption == 1);
            rdbtnSplitSubstring.Checked = (Main.Settings.SplitOption == 2);
            rdbtnSplitContains.Checked = (Main.Settings.SplitOption == 3);
            rdbtnSplitDecode.Checked = (Main.Settings.SplitOption == 4);

            // load user preferences
            txtSplitCharacter.Text = Main.Settings.SplitChar;

            numSplitSubstring.Value = Main.Settings.SplitPos;
            txtSplitContains.Text = Main.Settings.SplitContain;
            txtSplitDecode.Text = Main.Settings.SplitDecode;
            txtSplitDecodeChar.Text = Main.Settings.SplitDecodeChar;

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
            btnOk.Enabled = (cmbSelectColumn.SelectedIndex > 0);
        }

        private void ColumnSplitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass split parameters to previous form
            SplitCode = 0;
            if (rdbtnSplitValid.Checked)     SplitCode = 1;
            if (rdbtnSplitCharacter.Checked) SplitCode = 2;
            if (rdbtnSplitSubstring.Checked) SplitCode = 3;
            if (rdbtnSplitContains.Checked)  SplitCode = 4;
            if (rdbtnSplitDecode.Checked)    SplitCode = 5;

            // which column index
            SplitColumn = cmbSelectColumn.SelectedIndex - 1; // zero based, frist item is a dummy

            // extra parameters
            SplitParam1 = "";
            SplitParam2 = "";
            if (rdbtnSplitCharacter.Checked) SplitParam1 = txtSplitCharacter.Text;
            if (rdbtnSplitSubstring.Checked) SplitParam1 = numSplitSubstring.Value.ToString();
            if (rdbtnSplitContains.Checked)  SplitParam1 = txtSplitContains.Text;
            if (rdbtnSplitDecode.Checked)  { SplitParam1 = txtSplitDecode.Text; SplitParam2 = txtSplitDecodeChar.Text; }

            // remove original column
            SplitRemove = (chkDeleteOriginal.Checked);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.SplitColName = cmbSelectColumn.Items[cmbSelectColumn.SelectedIndex].ToString(); ;

            // pass split parameters to previous form
            var opt = 0;
            //if (rdbtnSplitValid.Checked)     opt = 0;
            if (rdbtnSplitCharacter.Checked) opt = 1;
            if (rdbtnSplitSubstring.Checked) opt = 2;
            if (rdbtnSplitContains.Checked)  opt = 3;
            if (rdbtnSplitDecode.Checked)    opt = 4;
            Main.Settings.SplitOption = opt;

            Main.Settings.SplitChar = txtSplitCharacter.Text;
            Main.Settings.SplitPos = Convert.ToInt32(numSplitSubstring.Value);
            Main.Settings.SplitContain = txtSplitContains.Text;
            Main.Settings.SplitDecode = txtSplitDecode.Text;
            Main.Settings.SplitDecodeChar = txtSplitDecodeChar.Text;

            Main.Settings.SplitRemoveOrg = chkDeleteOriginal.Checked;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
