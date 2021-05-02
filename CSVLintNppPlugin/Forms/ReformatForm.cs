using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class ReformatForm : Form
    {
        public ReformatForm()
        {
            InitializeComponent();
        }

        public string NewDataTime { get; set; }
        public string NewDecimal { get; set; }
        public string NewSeparator { get; set; }
        public bool UpdateSeparator { get; set; }

        public void InitialiseSetting(string dtFormat, string decSep, string colSep)
        {
            // set default values
            cmbDateTime.Text = dtFormat;
            cmbDecimal.Text = decSep;
            cmbSeparator.Text = colSep;

            // disable all
            OnChkbx_CheckedChanged(chkDateTime, null);
            OnChkbx_CheckedChanged(chkDecimal, null);
            OnChkbx_CheckedChanged(chkSeparator, null);
        }

        private void OnChkbx_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            int idx = Int32.Parse((sender as CheckBox).Tag.ToString());
            bool chk = (sender as CheckBox).Checked;

            // enable/disable corresponding dropdownlist
            if (idx == 0) cmbDateTime.Enabled = chk;
            if (idx == 1) cmbDecimal.Enabled = chk;
            if (idx == 2) cmbSeparator.Enabled = chk;

            // can not press OK when nothing selected
            btnOk.Enabled = (chkDateTime.Checked | chkDecimal.Checked | chkSeparator.Checked);
        }

        private void ReformatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            NewDataTime  = (chkDateTime.Checked  ? cmbDateTime.Text : "");
            NewDecimal   = (chkDecimal.Checked   ? cmbDecimal.Text : "");
            NewSeparator = (chkSeparator.Checked ? cmbSeparator.Text : "");
            UpdateSeparator = (chkSeparator.Checked);

            // exception
            if (NewSeparator == "{Tab}") NewSeparator = "\t";
            if (NewSeparator == "{Fixed width}") NewSeparator = "\0";
        }
    }
}
