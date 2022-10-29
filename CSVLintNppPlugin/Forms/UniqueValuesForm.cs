using CSVLint;
using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class UniqueValuesForm : CsvEditFormBase
    {
        public UniqueValuesForm()
        {
            InitializeComponent();
        }

        public List<int> columnIndexes { get; set; }
        public bool sortBy { get; set; }
        public bool sortValue { get; set; }
        public bool sortDesc { get; set; }

        public void InitialiseSetting(CsvDefinition csvdef)
        {
            // add all columns
            listColumns.Items.Clear();
            for (var i = 0; i < csvdef.Fields.Count; i++)
            {
                listColumns.Items.Add(csvdef.Fields[i].Name);
            }

            // set default values
            chkSortBy.Checked = true;
            radioSortValue.Checked = true;
            radioSortAsc.Checked = true;

            // disable ok
            listColumns_SelectedIndexChanged(listColumns, null);

            // load user preferences
            chkSortBy.Checked      = Main.Settings.UniqueSortBy;
            radioSortValue.Checked = Main.Settings.UniqueSortValue;
            radioSortCount.Checked = !Main.Settings.UniqueSortValue;

            radioSortAsc.Checked = Main.Settings.UniqueSortAsc;
            radioSortDesc.Checked = !Main.Settings.UniqueSortAsc;

            // pre-select columns from previous
            var tmp = Main.Settings.UniqueColumns.Split('|');
            foreach (var colname in tmp)
            {
                if (listColumns.Items.IndexOf(colname) >= 0) listColumns.SelectedItems.Add(colname);
            }
        }

        private void listColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            // can not press OK when nothing selected
            btnOk.Enabled = listColumns.SelectedIndices.Count > 0;
        }
        private void chkSortBy_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as CheckBox).Checked;
            ToggleControlBasedOnControl(sender as CheckBox, chk);
        }

        private void UniqueValuesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            sortBy = chkSortBy.Checked;
            sortValue = radioSortValue.Checked;
            sortDesc = radioSortDesc.Checked;

            // indexes of selected columns
            columnIndexes = new List<int>();
            foreach (var item in listColumns.SelectedItems)
            {
                columnIndexes.Add(listColumns.Items.IndexOf(item)); // Add selected indexes to the List<int>
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.UniqueSortBy = chkSortBy.Checked;
            Main.Settings.UniqueSortValue = radioSortValue.Checked;
            Main.Settings.UniqueSortAsc = radioSortAsc.Checked;

            // pre-select columns from previous
            var cols = "";
            foreach (var col in listColumns.SelectedItems)
            {
                cols += col.ToString() + "|";
            }
            Main.Settings.UniqueColumns = cols;

            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void mnuitmSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listColumns.Items.Count; i++)
                listColumns.SetSelected(i, true);
        }

        private void mnuitmSelectNone_Click(object sender, EventArgs e)
        {
            listColumns.ClearSelected();
        }

        private void mnuitmSelectInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listColumns.Items.Count; i++)
                listColumns.SetSelected(i, !listColumns.GetSelected(i));
        }
    }
}
