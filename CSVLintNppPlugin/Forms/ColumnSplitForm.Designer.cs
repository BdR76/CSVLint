
namespace CSVLintNppPlugin.Forms
{
    partial class ColumnSplitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSelectColumn = new System.Windows.Forms.Label();
            this.cmbSelectColumn = new System.Windows.Forms.ComboBox();
            this.rdbtnPadChar = new System.Windows.Forms.RadioButton();
            this.rdbtnSearchReplace = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitValid = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitCharacter = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitSubstring = new System.Windows.Forms.RadioButton();
            this.txtPadChar = new System.Windows.Forms.TextBox();
            this.lblPadLength = new System.Windows.Forms.Label();
            this.numPadLength = new System.Windows.Forms.NumericUpDown();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchReplace = new System.Windows.Forms.Label();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.txtSplitCharacter = new System.Windows.Forms.TextBox();
            this.lblSplitCharacter = new System.Windows.Forms.Label();
            this.numSplitNth = new System.Windows.Forms.NumericUpDown();
            this.numSplitSubstring = new System.Windows.Forms.NumericUpDown();
            this.chkDeleteOriginal = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPadLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSplitNth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSplitSubstring)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(169, 278);
            this.btnOk.TabIndex = 15;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(365, 9);
            this.picHelpIcon.Tag = "add-new-columns";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(144, 20);
            this.lblTitle.Text = "Add new column(s)";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(393, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(288, 278);
            this.btnCancel.TabIndex = 16;
            // 
            // lblSelectColumn
            // 
            this.lblSelectColumn.AutoSize = true;
            this.lblSelectColumn.Location = new System.Drawing.Point(12, 54);
            this.lblSelectColumn.Name = "lblSelectColumn";
            this.lblSelectColumn.Size = new System.Drawing.Size(125, 13);
            this.lblSelectColumn.TabIndex = 0;
            this.lblSelectColumn.Text = "Based on original column";
            // 
            // cmbSelectColumn
            // 
            this.cmbSelectColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSelectColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectColumn.FormattingEnabled = true;
            this.cmbSelectColumn.Items.AddRange(new object[] {
            "(select a column)"});
            this.cmbSelectColumn.Location = new System.Drawing.Point(192, 50);
            this.cmbSelectColumn.Name = "cmbSelectColumn";
            this.cmbSelectColumn.Size = new System.Drawing.Size(192, 21);
            this.cmbSelectColumn.TabIndex = 1;
            this.cmbSelectColumn.Tag = "";
            this.cmbSelectColumn.SelectedIndexChanged += new System.EventHandler(this.cmbSelectColumn_SelectedIndexChanged);
            // 
            // rdbtnPadChar
            // 
            this.rdbtnPadChar.AutoSize = true;
            this.rdbtnPadChar.Location = new System.Drawing.Point(16, 84);
            this.rdbtnPadChar.Name = "rdbtnPadChar";
            this.rdbtnPadChar.Size = new System.Drawing.Size(94, 17);
            this.rdbtnPadChar.TabIndex = 2;
            this.rdbtnPadChar.Tag = "1";
            this.rdbtnPadChar.Text = "Pad with string";
            this.rdbtnPadChar.UseVisualStyleBackColor = true;
            this.rdbtnPadChar.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            this.rdbtnPadChar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // rdbtnSearchReplace
            // 
            this.rdbtnSearchReplace.AutoSize = true;
            this.rdbtnSearchReplace.Location = new System.Drawing.Point(16, 116);
            this.rdbtnSearchReplace.Name = "rdbtnSearchReplace";
            this.rdbtnSearchReplace.Size = new System.Drawing.Size(118, 17);
            this.rdbtnSearchReplace.TabIndex = 3;
            this.rdbtnSearchReplace.Tag = "2";
            this.rdbtnSearchReplace.Text = "Search and replace";
            this.rdbtnSearchReplace.UseVisualStyleBackColor = true;
            this.rdbtnSearchReplace.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            this.rdbtnSearchReplace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // rdbtnSplitValid
            // 
            this.rdbtnSplitValid.AutoSize = true;
            this.rdbtnSplitValid.Location = new System.Drawing.Point(16, 148);
            this.rdbtnSplitValid.Name = "rdbtnSplitValid";
            this.rdbtnSplitValid.Size = new System.Drawing.Size(158, 17);
            this.rdbtnSplitValid.TabIndex = 4;
            this.rdbtnSplitValid.Tag = "3";
            this.rdbtnSplitValid.Text = "Split valid and invalid values";
            this.rdbtnSplitValid.UseVisualStyleBackColor = true;
            this.rdbtnSplitValid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // rdbtnSplitCharacter
            // 
            this.rdbtnSplitCharacter.AutoSize = true;
            this.rdbtnSplitCharacter.Location = new System.Drawing.Point(16, 180);
            this.rdbtnSplitCharacter.Name = "rdbtnSplitCharacter";
            this.rdbtnSplitCharacter.Size = new System.Drawing.Size(108, 17);
            this.rdbtnSplitCharacter.TabIndex = 5;
            this.rdbtnSplitCharacter.Tag = "4";
            this.rdbtnSplitCharacter.Text = "Split on character";
            this.rdbtnSplitCharacter.UseVisualStyleBackColor = true;
            this.rdbtnSplitCharacter.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            this.rdbtnSplitCharacter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // rdbtnSplitSubstring
            // 
            this.rdbtnSplitSubstring.AutoSize = true;
            this.rdbtnSplitSubstring.Location = new System.Drawing.Point(16, 212);
            this.rdbtnSplitSubstring.Name = "rdbtnSplitSubstring";
            this.rdbtnSplitSubstring.Size = new System.Drawing.Size(99, 17);
            this.rdbtnSplitSubstring.TabIndex = 6;
            this.rdbtnSplitSubstring.Tag = "5";
            this.rdbtnSplitSubstring.Text = "Split on position";
            this.rdbtnSplitSubstring.UseVisualStyleBackColor = true;
            this.rdbtnSplitSubstring.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            this.rdbtnSplitSubstring.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // txtPadChar
            // 
            this.txtPadChar.Enabled = false;
            this.txtPadChar.Location = new System.Drawing.Point(192, 83);
            this.txtPadChar.Name = "txtPadChar";
            this.txtPadChar.Size = new System.Drawing.Size(48, 20);
            this.txtPadChar.TabIndex = 7;
            this.txtPadChar.Tag = "1";
            this.txtPadChar.Text = "0";
            // 
            // lblPadLength
            // 
            this.lblPadLength.AutoSize = true;
            this.lblPadLength.Location = new System.Drawing.Point(256, 86);
            this.lblPadLength.Name = "lblPadLength";
            this.lblPadLength.Size = new System.Drawing.Size(59, 13);
            this.lblPadLength.TabIndex = 15;
            this.lblPadLength.Text = "total length";
            // 
            // numPadLength
            // 
            this.numPadLength.Enabled = false;
            this.numPadLength.Location = new System.Drawing.Point(336, 84);
            this.numPadLength.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numPadLength.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numPadLength.Name = "numPadLength";
            this.numPadLength.Size = new System.Drawing.Size(48, 20);
            this.numPadLength.TabIndex = 8;
            this.numPadLength.Tag = "1";
            // 
            // txtSearch
            // 
            this.txtSearch.Enabled = false;
            this.txtSearch.Location = new System.Drawing.Point(192, 115);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(48, 20);
            this.txtSearch.TabIndex = 9;
            this.txtSearch.Tag = "2";
            this.txtSearch.Text = "abc";
            // 
            // lblSearchReplace
            // 
            this.lblSearchReplace.AutoSize = true;
            this.lblSearchReplace.Location = new System.Drawing.Point(272, 118);
            this.lblSearchReplace.Name = "lblSearchReplace";
            this.lblSearchReplace.Size = new System.Drawing.Size(26, 13);
            this.lblSearchReplace.TabIndex = 16;
            this.lblSearchReplace.Text = "with";
            // 
            // txtReplace
            // 
            this.txtReplace.Enabled = false;
            this.txtReplace.Location = new System.Drawing.Point(336, 115);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(48, 20);
            this.txtReplace.TabIndex = 10;
            this.txtReplace.Tag = "2";
            this.txtReplace.Text = "xyz";
            // 
            // txtSplitCharacter
            // 
            this.txtSplitCharacter.Enabled = false;
            this.txtSplitCharacter.Location = new System.Drawing.Point(192, 179);
            this.txtSplitCharacter.Name = "txtSplitCharacter";
            this.txtSplitCharacter.Size = new System.Drawing.Size(48, 20);
            this.txtSplitCharacter.TabIndex = 11;
            this.txtSplitCharacter.Tag = "4";
            this.txtSplitCharacter.Text = "/";
            // 
            // lblSplitCharacter
            // 
            this.lblSplitCharacter.AutoSize = true;
            this.lblSplitCharacter.Location = new System.Drawing.Point(246, 183);
            this.lblSplitCharacter.Name = "lblSplitCharacter";
            this.lblSplitCharacter.Size = new System.Drawing.Size(81, 13);
            this.lblSplitCharacter.TabIndex = 15;
            this.lblSplitCharacter.Text = "Nth occurrence";
            // 
            // numSplitNth
            // 
            this.numSplitNth.Enabled = false;
            this.numSplitNth.Location = new System.Drawing.Point(336, 180);
            this.numSplitNth.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numSplitNth.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numSplitNth.Name = "numSplitNth";
            this.numSplitNth.Size = new System.Drawing.Size(48, 20);
            this.numSplitNth.TabIndex = 12;
            this.numSplitNth.Tag = "4";
            this.numSplitNth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSplitSubstring
            // 
            this.numSplitSubstring.Enabled = false;
            this.numSplitSubstring.Location = new System.Drawing.Point(192, 212);
            this.numSplitSubstring.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numSplitSubstring.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.numSplitSubstring.Name = "numSplitSubstring";
            this.numSplitSubstring.Size = new System.Drawing.Size(48, 20);
            this.numSplitSubstring.TabIndex = 13;
            this.numSplitSubstring.Tag = "5";
            // 
            // chkDeleteOriginal
            // 
            this.chkDeleteOriginal.AutoSize = true;
            this.chkDeleteOriginal.Location = new System.Drawing.Point(16, 244);
            this.chkDeleteOriginal.Name = "chkDeleteOriginal";
            this.chkDeleteOriginal.Size = new System.Drawing.Size(139, 17);
            this.chkDeleteOriginal.TabIndex = 14;
            this.chkDeleteOriginal.Tag = "6";
            this.chkDeleteOriginal.Text = "Remove original column";
            this.chkDeleteOriginal.UseVisualStyleBackColor = true;
            this.chkDeleteOriginal.CheckedChanged += new System.EventHandler(this.chkDeleteOriginal_CheckedChanged);
            // 
            // ColumnSplitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(400, 320);
            this.Controls.Add(this.lblSearchReplace);
            this.Controls.Add(this.lblSplitCharacter);
            this.Controls.Add(this.lblPadLength);
            this.Controls.Add(this.numSplitNth);
            this.Controls.Add(this.numPadLength);
            this.Controls.Add(this.numSplitSubstring);
            this.Controls.Add(this.chkDeleteOriginal);
            this.Controls.Add(this.txtPadChar);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtSplitCharacter);
            this.Controls.Add(this.rdbtnSplitSubstring);
            this.Controls.Add(this.rdbtnSplitCharacter);
            this.Controls.Add(this.rdbtnSplitValid);
            this.Controls.Add(this.rdbtnSearchReplace);
            this.Controls.Add(this.rdbtnPadChar);
            this.Controls.Add(this.cmbSelectColumn);
            this.Controls.Add(this.lblSelectColumn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(960, 359);
            this.MinimumSize = new System.Drawing.Size(416, 359);
            this.Name = "ColumnSplitForm";
            this.Tag = "split-column";
            this.Text = "Add new column(s)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColumnSplitForm_FormClosing);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblSelectColumn, 0);
            this.Controls.SetChildIndex(this.cmbSelectColumn, 0);
            this.Controls.SetChildIndex(this.rdbtnPadChar, 0);
            this.Controls.SetChildIndex(this.rdbtnSearchReplace, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitValid, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitCharacter, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitSubstring, 0);
            this.Controls.SetChildIndex(this.txtSplitCharacter, 0);
            this.Controls.SetChildIndex(this.txtSearch, 0);
            this.Controls.SetChildIndex(this.txtReplace, 0);
            this.Controls.SetChildIndex(this.txtPadChar, 0);
            this.Controls.SetChildIndex(this.chkDeleteOriginal, 0);
            this.Controls.SetChildIndex(this.numSplitSubstring, 0);
            this.Controls.SetChildIndex(this.numPadLength, 0);
            this.Controls.SetChildIndex(this.numSplitNth, 0);
            this.Controls.SetChildIndex(this.lblPadLength, 0);
            this.Controls.SetChildIndex(this.lblSplitCharacter, 0);
            this.Controls.SetChildIndex(this.lblSearchReplace, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPadLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSplitNth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSplitSubstring)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectColumn;
        private System.Windows.Forms.ComboBox cmbSelectColumn;
        private System.Windows.Forms.RadioButton rdbtnPadChar;
        private System.Windows.Forms.RadioButton rdbtnSearchReplace;
        private System.Windows.Forms.RadioButton rdbtnSplitValid;
        private System.Windows.Forms.RadioButton rdbtnSplitCharacter;
        private System.Windows.Forms.RadioButton rdbtnSplitSubstring;
        private System.Windows.Forms.TextBox txtPadChar;
        private System.Windows.Forms.Label lblPadLength;
        private System.Windows.Forms.NumericUpDown numPadLength;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearchReplace;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.TextBox txtSplitCharacter;
        private System.Windows.Forms.Label lblSplitCharacter;
        private System.Windows.Forms.NumericUpDown numSplitNth;
        private System.Windows.Forms.NumericUpDown numSplitSubstring;
        private System.Windows.Forms.CheckBox chkDeleteOriginal;
    }
}
