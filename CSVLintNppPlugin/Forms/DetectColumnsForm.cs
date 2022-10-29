using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class DetectColumnsForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public DetectColumnsForm()
        {
            InitializeComponent();
        }

        public char Separator { get; set; }
        public string ManWidths { get; set; }
        public bool HeaderNames { get; set; }

        public void InitialiseSetting()
        {
            // load user preferences
            cmbColumnSeparator.Text = Main.Settings.DetectColumnSep;
            txtFixedWidthPos.Text = Main.Settings.DetectColumnWidths;
            chkHeaderNames.Checked = Main.Settings.DetectColumnHeader;
        }

        private void DetectColumnsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            Separator = (cmbColumnSeparator.Text.Length > 0 ? cmbColumnSeparator.Text[0] : '\0');
            ManWidths = txtFixedWidthPos.Text.Replace(' ', ',').Replace(",,", ","); // Replace = allow both comma separated "10, 12, 15, 20" and space separated "10 12 15 20"
            HeaderNames = chkHeaderNames.Checked;

            // exception
            if (cmbColumnSeparator.Text == "{Tab}")         Separator = '\t';
            if (cmbColumnSeparator.Text == "{Fixed width}") Separator = '\0';
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.DetectColumnSep = cmbColumnSeparator.Text;
            Main.Settings.DetectColumnWidths = txtFixedWidthPos.Text.Replace(' ', ',').Replace(",,", ","); // Replace = allow both comma separated "10, 12, 15, 20" and space separated "10 12 15 20"
            Main.Settings.DetectColumnHeader = chkHeaderNames.Checked;

            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void cmbColumnSeparator_SelectedIndexChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = ((sender as ComboBox).SelectedIndex == 4);
            ToggleControlBasedOnControl(sender as ComboBox, chk);
        }
    }
}
