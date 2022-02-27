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
    public partial class MetaDataGenerateForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        public MetaDataGenerateForm()
        {
            InitializeComponent();
        }

        public void InitialiseSetting(CsvDefinition csvdef)
        {
            // save user preferences
            rdbtnSchemaIni.Checked    = (Main.Settings.SplitOption == 0); // schema ini
            rdbtnSchemaJSON.Checked   = (Main.Settings.SplitOption == 1); // schema JSON
            rdbtnPythonScript.Checked = (Main.Settings.SplitOption == 2); // Python script
            rdbtnRScript.Checked      = (Main.Settings.SplitOption == 3); // R - script
            rdbtnSPSSScript.Checked   = (Main.Settings.SplitOption == 4); // SPSS syntax;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            int idx = 0; // schema ini
            if (rdbtnSchemaJSON.Checked)   idx = 1; // schema JSON
            if (rdbtnPythonScript.Checked) idx = 2; // Python script
            if (rdbtnRScript.Checked)      idx = 3; // R - script
            if (rdbtnSPSSScript.Checked)   idx = 4; // SPSS syntax;

            Main.Settings.MetadataType = idx;

            // save to file
            Main.Settings.SaveToIniFile();
        }
    }
}
