using Kbg.NppPluginNET;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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
            rdbtnHTML.Checked = (Main.Settings.DataConvertType == 3); // HTML

            txtTablename.Text = Main.Settings.DataConvertName;

            var idx = Main.Settings.DataConvertSQL;
            cmbSQLtype.SelectedIndex = (idx >= 0 && idx <= 2 ? idx : 0);

            //numBatchSize.Value = Main.Settings.DataConvertBatch;
            var batch = Main.Settings.DataConvertBatch;
            batch = (batch < numBatchSize.Minimum || batch > numBatchSize.Maximum ? 1000 : batch);
            numBatchSize.Value = batch;

            idx = Main.Settings.DataConvertStyle;
            cmbCSSstyle.SelectedIndex = (idx >= 0 && idx <= 4 ? idx : 0);

            // trigger enable other based on which is selected (Note: first parameter rdbtnSQL could be any of the 4 radiobuttons in this case)
            ToggleChildControlsWithTag(rdbtnSQL, this, (Main.Settings.DataConvertType+1), true);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = (rdbtnSQL.Checked ? 0 : (rdbtnXML.Checked ? 1 : (rdbtnJSON.Checked ? 2 : (rdbtnHTML.Checked ? 3 : 0))));
            Main.Settings.DataConvertType = idx;
            Main.Settings.DataConvertName = txtTablename.Text;
            Main.Settings.DataConvertSQL = cmbSQLtype.SelectedIndex;
            Main.Settings.DataConvertBatch = Convert.ToInt32(numBatchSize.Value);
            Main.Settings.DataConvertStyle = cmbCSSstyle.SelectedIndex;

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
