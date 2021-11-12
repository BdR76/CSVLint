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
    public partial class UniqueValuesForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public UniqueValuesForm()
        {
            InitializeComponent();
        }

        public List<int> columnIndexes { get; set; }
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
            radioSortValue.Checked = true;
            radioSortAsc.Checked = true;

            // disable ok
            listColumns_SelectedIndexChanged(listColumns, null);

            // load user preferences
            radioSortValue.Checked = (Main.Settings.UniqueSortValue);
            radioSortCount.Checked = (!Main.Settings.UniqueSortValue);

            radioSortAsc.Checked = (Main.Settings.UniqueSortAsc);
            radioSortDesc.Checked = (!Main.Settings.UniqueSortAsc);

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
            btnOk.Enabled = (listColumns.SelectedIndices.Count > 0);
        }

        private void UniqueValuesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            sortValue = (radioSortValue.Checked);
            sortDesc = (radioSortDesc.Checked);

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
    }
}
