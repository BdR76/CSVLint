﻿using CSVLint;
using Kbg.NppPluginNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class DetectColumnsForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        private readonly ToolTip helperTip = new ToolTip();
        public DetectColumnsForm()
        {
            InitializeComponent();

            // tooltip initialization
            helperTip.SetToolTip(btnFixedWidthPos, "Column end positions based on current column widths");
        }

        public char Separator { get; set; }
        public string ManWidths { get; set; }
        public bool HeaderNames { get; set; }
        public int SkipLines { get; set; }
        public char CommentChar { get; set; }

        public void InitialiseSetting()
        {
            // load user preferences
            cmbColumnSeparator.Text = Main.Settings.DetectColumnSep;
            txtFixedWidthPos.Text = Main.Settings.DetectColumnWidths;
            chkHeaderNames.Checked = Main.Settings.DetectColumnHeader;
            chkSkipLines.Checked = Main.Settings.DetectSkipLines;
            numSkipLines.Value = Main.Settings.DetectSkipLinesCount;
            chkCommentChar.Checked = Main.Settings.DetectCommentChar;
            txtCommentChar.Text = Main.Settings.CommentCharacter.ToString();

            // initialise form, enable/disable controls
            OnChkbx_CheckedChanged(chkSkipLines, null);
            OnChkbx_CheckedChanged(chkCommentChar, null);
        }
        private void DetectColumnsForm_Load(object sender, EventArgs e)
        {
            InitialiseSetting();
        }

        private string GetProcessedColWidths()
        {
            // Quality-of-life extras, pre-process the list of col positions:
            // 1) allow both comma and space separated
            // 2) expect absolute positions, but also allow column widths
            // 3) remove 0 as start position, because it is always implicitly expected
            var ret = "";

            // 1) Replace = allow comma separated "10, 12, 15, 20" and space separated "10 12 15 20" and semicolon separated
            var strvalues = txtFixedWidthPos.Text.Replace(' ', ',').Replace(';', ',').Replace(",,", ",");

            // 2) widths should contain column end positions, check if user entered column widths instead
            // example expected pos [8, 14, 15, 22, 25] -> but user entered [8, 6, 1, 7, 3]
            // This is recognisable, due to larger values coming before smaller values

            // filter out invalid integers https://stackoverflow.com/a/2959329/1745616
            List<int> ints = (from field in strvalues.Split(',').Where((x) => { int dummy; return Int32.TryParse(x, out dummy); }) select Int32.Parse(field)).ToList();

            // This is recognisable, due to larger values coming before smaller values
            var cl = 0; //count larger before smaller
            for (int i = 0; i < ints.Count - 1; i++)
            {
                for (int j = i + 1; j < ints.Count; j++)
                {
                    if (ints[i] >= ints[j]) cl++;
                }
            }

            // if more than 0 larger values before smaller values
            if (cl > 0)
            {
                // assume user entered column widths instead of positions, change all width to positions
                var endpos = 0;
                for (int i = 0; i < ints.Count; i++)
                {
                    endpos += ints[i];
                    ints[i] = endpos;
                }
            }

            // 3) remove 0 as start position, because it is always implicitly expected
            if (ints.Count > 0) {
                if (ints[0] == 0) ints.RemoveAt(0);
                for (int i = 0; i < ints.Count; i++)
                {
                    ret = ret + (i > 0 ? "," : "") + ints[i].ToString();
                }
            }

            return ret;
        }

        private void DetectColumnsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass new values to previous form
            Separator = (cmbColumnSeparator.Text.Length > 0 ? cmbColumnSeparator.Text[0] : '\0');
            ManWidths = GetProcessedColWidths();
            HeaderNames = chkHeaderNames.Checked;
            SkipLines = (chkSkipLines.Checked ? Convert.ToInt32(numSkipLines.Value) : 0);
            CommentChar = (chkCommentChar.Checked && txtCommentChar.Text.Length > 0 ? txtCommentChar.Text[0] : '\0');

            // exception
            if (cmbColumnSeparator.Text == "{Tab}")         Separator = '\t';
            if (cmbColumnSeparator.Text == "{Fixed width}") Separator = '\0';
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.DetectColumnSep = cmbColumnSeparator.Text;
            Main.Settings.DetectColumnWidths = txtFixedWidthPos.Text.Replace(' ', ',').Replace(",,", ","); // Replace = allow both comma separated "10, 12, 15, 20" and space separated "10 12 15 20"
            Main.Settings.DetectColumnHeader = chkHeaderNames.Checked;

            Main.Settings.DetectSkipLines = chkSkipLines.Checked;
            Main.Settings.DetectSkipLinesCount = Convert.ToInt32(numSkipLines.Value);
            Main.Settings.DetectCommentChar = chkCommentChar.Checked;
            Main.Settings.CommentCharacter = (chkCommentChar.Checked && txtCommentChar.Text.Length > 0 ? txtCommentChar.Text[0] : Main.Settings.CommentCharacter); // only first character

            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void cmbColumnSeparator_SelectedIndexChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = ((sender as ComboBox).SelectedIndex == 4);
            ToggleControlBasedOnControl(sender as ComboBox, chk);
        }

        private void OnChkbx_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as CheckBox).Checked;
            ToggleControlBasedOnControl(sender as CheckBox, chk);
        }

        private void btnFixedWidthPos_Click(object sender, EventArgs e)
        {
            // show dialog
            DialogResult dialogResult = MessageBox.Show("Paste column end positions based on the current column widths?", "Paste end positions", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                // paste column positions
                if (!Main.TryGetCurrentCsvDef(out CsvDefinition csvdef))
                    return;
                txtFixedWidthPos.Text = csvdef.GetColumnWidths(true);
            }
        }
    }
}
