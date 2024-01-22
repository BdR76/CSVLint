using CSVLint;
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
    public partial class SortForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {

        public SortForm()
        {
            InitializeComponent();
        }
        public int SortColumn { get; set; }

        public bool SortAscending { get; set; }
        public bool SortValue { get; set; }

        public void InitialiseSetting(CsvDefinition csvdef)
        {
            // add all columns to list
            var idx = 0;
            for (var i = 0; i < csvdef.Fields.Count; i++)
            {
                var str = csvdef.Fields[i].Name;
                cmbSelectColumn.Items.Add(str);
                if (str == Main.Settings.SortColName) idx = cmbSelectColumn.Items.Count - 1;
            }

            // default select "(select column)" item
            cmbSelectColumn.SelectedIndex = idx;

            // which option selected
            rdbtnAscending.Checked = (Main.Settings.SortAscending);
            rdbtnDescending.Checked = (!Main.Settings.SortAscending);

            rdbtnValue.Checked = (Main.Settings.SortValue);
            rdbtnLength.Checked = (!Main.Settings.SortValue);
        }

        private void EvaluateOkButton()
        {
            // can not press OK when nothing selected
            btnOk.Enabled = (cmbSelectColumn.SelectedIndex > 0);
        }

        private void cmbSelectColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvaluateOkButton();
        }

        private void SortForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // which column index
            SortColumn = cmbSelectColumn.SelectedIndex - 1; // zero based, first item is a dummy

            SortAscending = rdbtnAscending.Checked;
            SortValue = rdbtnValue.Checked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.SortColName = cmbSelectColumn.Items[cmbSelectColumn.SelectedIndex].ToString(); ;

            Main.Settings.SortAscending = rdbtnAscending.Checked;
            Main.Settings.SortValue = rdbtnValue.Checked;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
