using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using CSVLint;
using CsvQuery.PluginInfrastructure;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    public partial class CsvLintWindow : Form
    {
        public CsvLintWindow()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // interface to Notepad++
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;

            // get text
            var textLength = scintillaGateway.GetTextLength();
            //string lines = scintillaGateway.GetText(Math.Min(1000000, textLength)); // TODO sample size as settings
            string lines = scintillaGateway.GetText(textLength);

            // analyze and determine csv definition
            CsvDefinition csvdef = CsvAnalyze.InferFromData(lines);

            // display csv definition
            txtSchemaIni.Text = csvdef.getIniLines();
            txtOutput.Clear();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            // interface to Notepad++
            ScintillaGateway scintillaGateway = PluginBase.CurrentScintillaGateway;

            // get sample of text
            var textLength = scintillaGateway.GetTextLength();
            //string lines = scintillaGateway.GetTextRange(Math.Min(100000, textLength));
            string sample = scintillaGateway.GetText(Math.Min(100000, textLength));

            // get csv definition from ini lines
            string inilines = txtSchemaIni.Text;

            // get key values from  ini lines
            var enstr = inilines.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(part => part.Split('='));

            // check for duplicate keys before turning into dictionary
            var dup = enstr.GroupBy(x => x[0])
                          .Where(g => g.Count() > 1)
                          .Select(y => y.Key)
                          .ToList();
            if (dup.Count > 0)
            {
                string errmsg = string.Format("Duplicate key(s) found ({0})", string.Join(",", dup));
                //throw new System.ArgumentException(err, "Error");
                //throw new ArgumentOutOfRangeException("error 123", err);
                MessageBox.Show(errmsg, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // create dictionary
                Dictionary<String, String> keys = enstr.ToDictionary(split => split[0], split => split[1]);

                // create dictionary
                CsvDefinition csvdef = new CsvDefinition(keys);

                // validate data
                CsvValidate csvval = new CsvValidate();

                var sr = ScintillaStreams.StreamAllText();

                //csvval.ValidateData(sample, csvdef);
                csvval.ValidateData(sr, csvdef);

                // display output message or errors
                string msg = csvval.report();
                txtOutput.Text = msg;
            }
        }
    }
}
