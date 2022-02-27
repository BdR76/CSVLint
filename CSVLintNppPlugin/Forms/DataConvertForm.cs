using Kbg.NppPluginNET;
using System;

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
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = (rdbtnSQL.Checked ? 0 : (rdbtnXML.Checked ? 1 : (rdbtnJSON.Checked ? 2 : 0)));
            Main.Settings.DataConvertType = idx;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
