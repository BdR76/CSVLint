using CSVLint;
using Kbg.NppPluginNET;
using System;

namespace CSVLintNppPlugin.Forms
{
    public partial class MetaDataGenerateForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public MetaDataGenerateForm()
        {
            InitializeComponent();
        }

        public void InitialiseSetting()
        {
            // save user preferences
            rdbtnPython.Checked     = (Main.Settings.MetadataType == 0); // Python
            rdbtnRScript.Checked    = (Main.Settings.MetadataType == 1); // R - script
            rdbtnSchemaIni.Checked  = (Main.Settings.MetadataType == 2); // schema ini
            rdbtnSchemaJSON.Checked = (Main.Settings.MetadataType == 3); // schema JSON
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = 0; // Python
            if (rdbtnPython.Checked)     idx = 1; // R - script
            if (rdbtnRScript.Checked)    idx = 2; // schema ini
            if (rdbtnSchemaJSON.Checked) idx = 3; // schema JSON

            Main.Settings.MetadataType = idx;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
