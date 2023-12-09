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
            rdbtnSchemaIni.Checked   = (Main.Settings.MetadataType == 0); // schema ini
            rdbtnSchemaJSON.Checked  = (Main.Settings.MetadataType == 1); // schema JSON
            rdbtnDatadictCSV.Checked = (Main.Settings.MetadataType == 2); // Datadictionary CSV
            rdbtnPython.Checked      = (Main.Settings.MetadataType == 3); // Python
            rdbtnRScript.Checked     = (Main.Settings.MetadataType == 4); // R-script
            rdbtnPowerShell.Checked  = (Main.Settings.MetadataType == 5); // PowerShell
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = 0; // schema ini
            if (rdbtnSchemaJSON.Checked)  idx = 1; // schema JSON
            if (rdbtnDatadictCSV.Checked) idx = 2; // Datadictionary CSV
            if (rdbtnPython.Checked)      idx = 3; // Python
            if (rdbtnRScript.Checked)     idx = 4; // R-script
            if (rdbtnPowerShell.Checked)  idx = 5; // PowerShell

            Main.Settings.MetadataType = idx;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
