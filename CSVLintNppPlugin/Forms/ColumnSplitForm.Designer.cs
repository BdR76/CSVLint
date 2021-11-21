
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
            this.rdbtnSplitValid = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitCharacter = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitSubstring = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitContains = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitDecode = new System.Windows.Forms.RadioButton();
            this.txtSplitCharacter = new System.Windows.Forms.TextBox();
            this.txtSplitContains = new System.Windows.Forms.TextBox();
            this.txtSplitDecode = new System.Windows.Forms.TextBox();
            this.txtSplitDecodeChar = new System.Windows.Forms.TextBox();
            this.chkDeleteOriginal = new System.Windows.Forms.CheckBox();
            this.numSplitSubstring = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numSplitSubstring)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(95, 20);
            this.lblTitle.Text = "Split column";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(152, 278);
            this.btnOk.TabIndex = 13;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(271, 278);
            this.btnCancel.TabIndex = 14;
            // 
            // lblSelectColumn
            // 
            this.lblSelectColumn.AutoSize = true;
            this.lblSelectColumn.Location = new System.Drawing.Point(12, 52);
            this.lblSelectColumn.Name = "lblSelectColumn";
            this.lblSelectColumn.Size = new System.Drawing.Size(107, 13);
            this.lblSelectColumn.TabIndex = 0;
            this.lblSelectColumn.Text = "Select column to split";
            // 
            // cmbSelectColumn
            // 
            this.cmbSelectColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectColumn.FormattingEnabled = true;
            this.cmbSelectColumn.Items.AddRange(new object[] {
            "(select a column)"});
            this.cmbSelectColumn.Location = new System.Drawing.Point(192, 48);
            this.cmbSelectColumn.Name = "cmbSelectColumn";
            this.cmbSelectColumn.Size = new System.Drawing.Size(180, 21);
            this.cmbSelectColumn.TabIndex = 1;
            this.cmbSelectColumn.Tag = "1";
            this.cmbSelectColumn.SelectedIndexChanged += new System.EventHandler(this.cmbSelectColumn_SelectedIndexChanged);
            // 
            // rdbtnSplitValid
            // 
            this.rdbtnSplitValid.AutoSize = true;
            this.rdbtnSplitValid.Checked = true;
            this.rdbtnSplitValid.Location = new System.Drawing.Point(16, 84);
            this.rdbtnSplitValid.Name = "rdbtnSplitValid";
            this.rdbtnSplitValid.Size = new System.Drawing.Size(158, 17);
            this.rdbtnSplitValid.TabIndex = 2;
            this.rdbtnSplitValid.TabStop = true;
            this.rdbtnSplitValid.Tag = "1";
            this.rdbtnSplitValid.Text = "Split valid and invalid values";
            this.rdbtnSplitValid.UseVisualStyleBackColor = true;
            // 
            // rdbtnSplitCharacter
            // 
            this.rdbtnSplitCharacter.AutoSize = true;
            this.rdbtnSplitCharacter.Location = new System.Drawing.Point(16, 116);
            this.rdbtnSplitCharacter.Name = "rdbtnSplitCharacter";
            this.rdbtnSplitCharacter.Size = new System.Drawing.Size(108, 17);
            this.rdbtnSplitCharacter.TabIndex = 3;
            this.rdbtnSplitCharacter.Tag = "2";
            this.rdbtnSplitCharacter.Text = "Split on character";
            this.rdbtnSplitCharacter.UseVisualStyleBackColor = true;
            this.rdbtnSplitCharacter.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnSplitSubstring
            // 
            this.rdbtnSplitSubstring.AutoSize = true;
            this.rdbtnSplitSubstring.Location = new System.Drawing.Point(16, 148);
            this.rdbtnSplitSubstring.Name = "rdbtnSplitSubstring";
            this.rdbtnSplitSubstring.Size = new System.Drawing.Size(99, 17);
            this.rdbtnSplitSubstring.TabIndex = 4;
            this.rdbtnSplitSubstring.Tag = "3";
            this.rdbtnSplitSubstring.Text = "Split on position";
            this.rdbtnSplitSubstring.UseVisualStyleBackColor = true;
            this.rdbtnSplitSubstring.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnSplitContains
            // 
            this.rdbtnSplitContains.AutoSize = true;
            this.rdbtnSplitContains.Location = new System.Drawing.Point(16, 180);
            this.rdbtnSplitContains.Name = "rdbtnSplitContains";
            this.rdbtnSplitContains.Size = new System.Drawing.Size(140, 17);
            this.rdbtnSplitContains.TabIndex = 5;
            this.rdbtnSplitContains.Tag = "4";
            this.rdbtnSplitContains.Text = "Move value if it contains";
            this.rdbtnSplitContains.UseVisualStyleBackColor = true;
            this.rdbtnSplitContains.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnSplitDecode
            // 
            this.rdbtnSplitDecode.AutoSize = true;
            this.rdbtnSplitDecode.Location = new System.Drawing.Point(16, 212);
            this.rdbtnSplitDecode.Name = "rdbtnSplitDecode";
            this.rdbtnSplitDecode.Size = new System.Drawing.Size(130, 17);
            this.rdbtnSplitDecode.TabIndex = 6;
            this.rdbtnSplitDecode.Tag = "5";
            this.rdbtnSplitDecode.Text = "Decode multiple value";
            this.rdbtnSplitDecode.UseVisualStyleBackColor = true;
            this.rdbtnSplitDecode.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // txtSplitCharacter
            // 
            this.txtSplitCharacter.Enabled = false;
            this.txtSplitCharacter.Location = new System.Drawing.Point(191, 112);
            this.txtSplitCharacter.Name = "txtSplitCharacter";
            this.txtSplitCharacter.Size = new System.Drawing.Size(48, 20);
            this.txtSplitCharacter.TabIndex = 7;
            this.txtSplitCharacter.Tag = "2";
            this.txtSplitCharacter.Text = "/";
            // 
            // txtSplitContains
            // 
            this.txtSplitContains.Enabled = false;
            this.txtSplitContains.Location = new System.Drawing.Point(191, 176);
            this.txtSplitContains.Name = "txtSplitContains";
            this.txtSplitContains.Size = new System.Drawing.Size(48, 20);
            this.txtSplitContains.TabIndex = 9;
            this.txtSplitContains.Tag = "4";
            this.txtSplitContains.Text = ".00";
            // 
            // txtSplitDecode
            // 
            this.txtSplitDecode.Enabled = false;
            this.txtSplitDecode.Location = new System.Drawing.Point(191, 208);
            this.txtSplitDecode.Name = "txtSplitDecode";
            this.txtSplitDecode.Size = new System.Drawing.Size(159, 20);
            this.txtSplitDecode.TabIndex = 10;
            this.txtSplitDecode.Tag = "5";
            this.txtSplitDecode.Text = "1;2;3;4;5";
            // 
            // txtSplitDecodeChar
            // 
            this.txtSplitDecodeChar.Enabled = false;
            this.txtSplitDecodeChar.Location = new System.Drawing.Point(356, 208);
            this.txtSplitDecodeChar.Name = "txtSplitDecodeChar";
            this.txtSplitDecodeChar.Size = new System.Drawing.Size(16, 20);
            this.txtSplitDecodeChar.TabIndex = 11;
            this.txtSplitDecodeChar.Tag = "5";
            this.txtSplitDecodeChar.Text = ";";
            // 
            // chkDeleteOriginal
            // 
            this.chkDeleteOriginal.AutoSize = true;
            this.chkDeleteOriginal.Location = new System.Drawing.Point(16, 244);
            this.chkDeleteOriginal.Name = "chkDeleteOriginal";
            this.chkDeleteOriginal.Size = new System.Drawing.Size(139, 17);
            this.chkDeleteOriginal.TabIndex = 12;
            this.chkDeleteOriginal.Tag = "6";
            this.chkDeleteOriginal.Text = "Remove original column";
            this.chkDeleteOriginal.UseVisualStyleBackColor = true;
            // 
            // numSplitSubstring
            // 
            this.numSplitSubstring.Enabled = false;
            this.numSplitSubstring.Location = new System.Drawing.Point(191, 144);
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
            this.numSplitSubstring.TabIndex = 8;
            this.numSplitSubstring.Tag = "3";
            // 
            // ColumnSplitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(383, 320);
            this.Controls.Add(this.numSplitSubstring);
            this.Controls.Add(this.chkDeleteOriginal);
            this.Controls.Add(this.txtSplitDecodeChar);
            this.Controls.Add(this.txtSplitDecode);
            this.Controls.Add(this.txtSplitContains);
            this.Controls.Add(this.txtSplitCharacter);
            this.Controls.Add(this.rdbtnSplitDecode);
            this.Controls.Add(this.rdbtnSplitContains);
            this.Controls.Add(this.rdbtnSplitSubstring);
            this.Controls.Add(this.rdbtnSplitCharacter);
            this.Controls.Add(this.rdbtnSplitValid);
            this.Controls.Add(this.cmbSelectColumn);
            this.Controls.Add(this.lblSelectColumn);
            this.Name = "ColumnSplitForm";
            this.Text = "Split column";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColumnSplitForm_FormClosing);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblSelectColumn, 0);
            this.Controls.SetChildIndex(this.cmbSelectColumn, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitValid, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitCharacter, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitSubstring, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitContains, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitDecode, 0);
            this.Controls.SetChildIndex(this.txtSplitCharacter, 0);
            this.Controls.SetChildIndex(this.txtSplitContains, 0);
            this.Controls.SetChildIndex(this.txtSplitDecode, 0);
            this.Controls.SetChildIndex(this.txtSplitDecodeChar, 0);
            this.Controls.SetChildIndex(this.chkDeleteOriginal, 0);
            this.Controls.SetChildIndex(this.numSplitSubstring, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numSplitSubstring)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectColumn;
        private System.Windows.Forms.ComboBox cmbSelectColumn;
        private System.Windows.Forms.RadioButton rdbtnSplitValid;
        private System.Windows.Forms.RadioButton rdbtnSplitCharacter;
        private System.Windows.Forms.RadioButton rdbtnSplitSubstring;
        private System.Windows.Forms.RadioButton rdbtnSplitContains;
        private System.Windows.Forms.RadioButton rdbtnSplitDecode;
        private System.Windows.Forms.TextBox txtSplitCharacter;
        private System.Windows.Forms.NumericUpDown numSplitSubstring;
        private System.Windows.Forms.TextBox txtSplitContains;
        private System.Windows.Forms.TextBox txtSplitDecode;
        private System.Windows.Forms.TextBox txtSplitDecodeChar;
        private System.Windows.Forms.CheckBox chkDeleteOriginal;
    }
}
