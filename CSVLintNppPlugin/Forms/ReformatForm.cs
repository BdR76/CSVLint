using Kbg.NppPluginNET;
using System;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class ReformatForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public ReformatForm()
        {
            InitializeComponent();
        }

        public string NewDataTime { get; set; }
        public string NewDecimal { get; set; }
        public string NewSeparator { get; set; }
        public bool UpdateSeparator { get; set; }
        public int ApplyQuotes { get; set; }
        
        public bool TrimAllValues { get; set; }
        public bool alignVertically { get; set; }

        public void InitialiseSetting(string dtFormat, string decSep, string colSep)
        {
            // load user preferences
            cmbDateTime.Text  = Main.Settings.ReformatDateFormat;
            cmbDecimal.Text   = Main.Settings.ReformatDecSep;
            cmbSeparator.Text = Main.Settings.ReformatColSep;

            cmbQuotes.SelectedIndex = (Main.Settings.ReformatQuotes >= 0 && Main.Settings.ReformatQuotes < cmbQuotes.Items.Count ? Main.Settings.ReformatQuotes : 0);

            // load user preferences
            chkDateTime.Checked    = (Main.Settings.ReformatOptions.IndexOf("1;") >= 0);
            chkDecimal.Checked     = (Main.Settings.ReformatOptions.IndexOf("2;") >= 0);
            chkSeparator.Checked   = (Main.Settings.ReformatOptions.IndexOf("3;") >= 0);
            chkApplyQuotes.Checked = (Main.Settings.ReformatOptions.IndexOf("4;") >= 0);
            chkTrimAll.Checked     = (Main.Settings.ReformatOptions.IndexOf("5;") >= 0);
            chkAlignVert.Checked   = (Main.Settings.ReformatOptions.IndexOf("6;") >= 0);

            // enable/disable all
            OnChkbx_CheckedChanged(chkDateTime, null);
            OnChkbx_CheckedChanged(chkDecimal, null);
            OnChkbx_CheckedChanged(chkSeparator, null);
            OnChkbx_CheckedChanged(chkApplyQuotes, null);
            OnChkbx_CheckedChanged(chkTrimAll, null);
            OnChkbx_CheckedChanged(chkAlignVert, null);
        }

        private void OnChkbx_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as CheckBox).Checked;
            ToggleControlBasedOnControl((sender as CheckBox), chk);

            // can not press OK when nothing selected
            btnOk.Enabled = (chkDateTime.Checked | chkDecimal.Checked | chkSeparator.Checked | chkApplyQuotes.Checked | chkTrimAll.Checked | chkAlignVert.Checked);
        }

        private void ReformatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            NewDataTime  = (chkDateTime.Checked  ? cmbDateTime.Text : "");
            NewDecimal   = (chkDecimal.Checked   ? cmbDecimal.Text : "");
            NewSeparator = (chkSeparator.Checked ? cmbSeparator.Text : "");
            UpdateSeparator = (chkSeparator.Checked);
            ApplyQuotes     = (chkApplyQuotes.Checked ? cmbQuotes.SelectedIndex : 0);
            TrimAllValues   = (chkTrimAll.Checked);
            alignVertically = (chkAlignVert.Checked);

            // exception
            if (NewSeparator == "{Tab}") NewSeparator = "\t";
            if (NewSeparator == "{Fixed width}") NewSeparator = "\0";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.ReformatDateFormat = cmbDateTime.Text;
            Main.Settings.ReformatDecSep     = cmbDecimal.Text;
            Main.Settings.ReformatColSep     = cmbSeparator.Text;
            Main.Settings.ReformatQuotes     = cmbQuotes.SelectedIndex;

            // load user preferences
            var opt = "";
            opt += (chkDateTime.Checked     ? "1;" : "") +
                    (chkDecimal.Checked     ? "2;" : "") +
                    (chkSeparator.Checked   ? "3;" : "") +
                    (chkApplyQuotes.Checked ? "4;" : "") +
                    (chkTrimAll.Checked     ? "5;" : "") +
                    (chkAlignVert.Checked   ? "6;" : "");
            Main.Settings.ReformatOptions = opt;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
