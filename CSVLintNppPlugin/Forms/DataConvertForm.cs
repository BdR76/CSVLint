using Kbg.NppPluginNET;
using System;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class DataConvertForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public DataConvertForm()
        {
            InitializeComponent();
        }

        public void InitialiseSetting()
        {
            // get user preferences
            rdbtnSQL.Checked  = (Main.Settings.DataConvertType == 0); // SQL
            rdbtnXML.Checked  = (Main.Settings.DataConvertType == 1); // XML
            rdbtnJSON.Checked = (Main.Settings.DataConvertType == 2); // JSON

            var idx = Main.Settings.DataConvertSQL;
            cmbSQLtype.SelectedIndex = (idx >= 0 && idx <= 2 ? idx : 0);

            //numSQLBatchSize.Value = Main.Settings.DataConvertBatch;
            var batch = Main.Settings.DataConvertBatch;
            batch = (batch < numSQLBatchSize.Minimum || batch > numSQLBatchSize.Maximum ? 1000 : batch);
            numSQLBatchSize.Value = batch;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = (rdbtnSQL.Checked ? 0 : (rdbtnXML.Checked ? 1 : (rdbtnJSON.Checked ? 2 : 0)));
            Main.Settings.DataConvertType = idx;
            Main.Settings.DataConvertSQL = cmbSQLtype.SelectedIndex;

            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void OnRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as RadioButton).Checked;
            ToggleControlBasedOnControl((sender as RadioButton), chk);
        }
    }
}
