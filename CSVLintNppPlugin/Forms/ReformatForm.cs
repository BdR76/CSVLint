using Kbg.NppPluginNET;
using System;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class ReformatForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        private readonly ToolTip helperTip = new ToolTip();

        public ReformatForm()
        {
            InitializeComponent();
        }
        public string NewSeparator { get; set; }
        public bool UpdateSeparator { get; set; }
        public string NewDataTime { get; set; }
        public string NewDecimal { get; set; }
        public string ReplaceCrLf { get; set; }
        public bool alignVertically { get; set; }

        public void InitialiseSetting()
        {
            // load user preferences
            cmbSeparator.Text   = Main.Settings.ReformatColSep;
            cmbDateTime.Text    = Main.Settings.ReformatDateFormat;
            cmbDecimal.Text     = Main.Settings.ReformatDecSep;
            txtReplaceCrLf.Text = Main.Settings.ReformatReplaceCrLf;

            // load user preferences
            chkSeparator.Checked   = Main.Settings.ReformatOptions.IndexOf("1;") >= 0;
            chkDateTime.Checked    = Main.Settings.ReformatOptions.IndexOf("2;") >= 0;
            chkDecimal.Checked     = Main.Settings.ReformatOptions.IndexOf("3;") >= 0;
            chkReplaceCrLf.Checked = Main.Settings.ReformatOptions.IndexOf("4;") >= 0;
            chkAlignVert.Checked   = Main.Settings.ReformatOptions.IndexOf("5;") >= 0;

            // enable/disable all
            OnChkbx_CheckedChanged(chkSeparator, null);
            OnChkbx_CheckedChanged(chkDateTime, null);
            OnChkbx_CheckedChanged(chkDecimal, null);
            OnChkbx_CheckedChanged(chkReplaceCrLf, null);
            OnChkbx_CheckedChanged(chkTrimValues, null);
            OnChkbx_CheckedChanged(chkAlignVert, null);

            // general settings
            chkTrimValues.Checked = Main.Settings.TrimValues;
            cmbQuotes.SelectedIndex = Main.Settings.ReformatQuotes >= 0 && Main.Settings.ReformatQuotes < cmbQuotes.Items.Count ? Main.Settings.ReformatQuotes : 0;

            // tooltip initialization
            helperTip.SetToolTip(grpGeneral, "*Also applied when detecting, sorting, adding and validating data.\nAlso see menu Plugins > CSV Lint > Settings.");
            helperTip.SetToolTip(chkTrimValues, "It is generally recommended to enable TrimValues, especially when\nreformatting datetime/decimal values or working with fixed width data.");
        }

        private void ReformatForm_Load(object sender, EventArgs e)
        {
            InitialiseSetting();
        }

        private void OnChkbx_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as CheckBox).Checked;
            ToggleControlBasedOnControl(sender as CheckBox, chk);

            // can always press OK, even when nothing selected to apply the general Trim and Quotes settings
            //btnOk.Enabled = chkSeparator.Checked | chkDateTime.Checked | chkDecimal.Checked | chkReplaceCrLf.Checked | chkTrimValues.Checked | chkAlignVert.Checked;
        }

        private void ReformatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            NewSeparator = chkSeparator.Checked ? cmbSeparator.Text : "";
            NewDataTime  = chkDateTime.Checked  ? cmbDateTime.Text : "";
            NewDecimal   = chkDecimal.Checked   ? cmbDecimal.Text : "";
            UpdateSeparator = chkSeparator.Checked;
            ReplaceCrLf     = chkReplaceCrLf.Checked ? txtReplaceCrLf.Text : "\r\n";
            alignVertically = chkAlignVert.Checked;

            // exception
            if (NewSeparator == "{Tab}") NewSeparator = "\t";
            if (NewSeparator == "{Fixed width}") NewSeparator = "\0";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.ReformatColSep      = cmbSeparator.Text;
            Main.Settings.ReformatDateFormat  = cmbDateTime.Text;
            Main.Settings.ReformatDecSep      = cmbDecimal.Text;
            Main.Settings.ReformatReplaceCrLf = txtReplaceCrLf.Text;

            // load user preferences
            string opt =
                (chkSeparator.Checked   ? "1;" : "") +
                (chkDateTime.Checked    ? "2;" : "") +
                (chkDecimal.Checked     ? "3;" : "") +
                (chkReplaceCrLf.Checked ? "4;" : "") +
                (chkAlignVert.Checked   ? "5;" : "");
            Main.Settings.ReformatOptions = opt;

            // general settings
            Main.Settings.TrimValues = chkTrimValues.Checked;
            Main.Settings.ReformatQuotes = cmbQuotes.SelectedIndex;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
