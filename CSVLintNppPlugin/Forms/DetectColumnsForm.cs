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
        public bool HeaderNames { get; set; }

        public void InitialiseSetting()
        {
            // load user preferences
            cmbColumnSeparator.Text = Main.Settings.DetectColumnSep;
            chkHeaderNames.Checked = Main.Settings.DetectColumnHeader;
        }

        private void DetectColumnsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            Separator   = (cmbColumnSeparator.Text.Length > 0 ? cmbColumnSeparator.Text[0] : '\0');
            HeaderNames = chkHeaderNames.Checked;

            // exception
            if (cmbColumnSeparator.Text == "{Tab}")         Separator = '\t';
            if (cmbColumnSeparator.Text == "{Fixed width}") Separator = '\0';
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.DetectColumnSep = cmbColumnSeparator.Text;
            Main.Settings.DetectColumnHeader = chkHeaderNames.Checked;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
