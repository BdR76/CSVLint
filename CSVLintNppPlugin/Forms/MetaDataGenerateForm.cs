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
            rdbtnSchemaIni.Checked  = (Main.Settings.MetadataType == 0); // schema ini
            rdbtnSchemaJSON.Checked = (Main.Settings.MetadataType == 1); // schema JSON
            rdbtnPython.Checked     = (Main.Settings.MetadataType == 2); // Python
            rdbtnRScript.Checked    = (Main.Settings.MetadataType == 3); // R - script
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = 0; // schema ini
            if (rdbtnSchemaJSON.Checked) idx = 1; // schema JSON
            if (rdbtnPython.Checked)     idx = 2; // Python
            if (rdbtnRScript.Checked)    idx = 3; // R - script

            Main.Settings.MetadataType = idx;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
