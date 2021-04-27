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

        public string newDataTime { get; set; }
        public string newDecimal { get; set; }
        public string newSeparator { get; set; }

        public void InitialiseSetting(string dtFormat, string decSep, string colSep)
        {
            // set default values
            cmbDateTime.Text = dtFormat;
            cmbDecimal.Text = decSep;
            cmbSeparator.Text = colSep;

            // disable all
            chkbx_CheckedChanged(chkDateTime, null);
            chkbx_CheckedChanged(chkDecimal, null);
            chkbx_CheckedChanged(chkSeparator, null);
        }

        private void chkbx_CheckedChanged(object sender, EventArgs e)
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
            newDataTime  = (chkDateTime.Checked  ? cmbDateTime.Text  : "");
            newDecimal   = (chkDecimal.Checked   ? cmbDecimal.Text   : "");
            newSeparator = (chkSeparator.Checked ? cmbSeparator.Text : "");
        }
    }
}
