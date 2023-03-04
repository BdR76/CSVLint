namespace CSVLintNppPlugin.Forms
{
    partial class ReformatForm
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
            this.chkSeparator = new System.Windows.Forms.CheckBox();
            this.cmbSeparator = new System.Windows.Forms.ComboBox();
            this.chkDateTime = new System.Windows.Forms.CheckBox();
            this.chkDecimal = new System.Windows.Forms.CheckBox();
            this.cmbDecimal = new System.Windows.Forms.ComboBox();
            this.cmbDateTime = new System.Windows.Forms.ComboBox();
            this.chkAlignVert = new System.Windows.Forms.CheckBox();
            this.chkReplaceCrLf = new System.Windows.Forms.CheckBox();
            this.txtReplaceCrLf = new System.Windows.Forms.TextBox();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.lblQuotes = new System.Windows.Forms.Label();
            this.lblTrimValues = new System.Windows.Forms.Label();
            this.chkTrimValues = new System.Windows.Forms.CheckBox();
            this.cmbQuotes = new System.Windows.Forms.ComboBox();
            this.lblReformat = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.grpGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(117, 338);
            this.btnOk.TabIndex = 12;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 338);
            this.btnCancel.TabIndex = 13;
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(112, 20);
            this.lblTitle.Text = "Reformat data";
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(316, 8);
            this.picHelpIcon.Tag = "reformat";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(340, 2);
            // 
            // chkSeparator
            // 
            this.chkSeparator.AutoSize = true;
            this.chkSeparator.Location = new System.Drawing.Point(12, 48);
            this.chkSeparator.Name = "chkSeparator";
            this.chkSeparator.Size = new System.Drawing.Size(108, 17);
            this.chkSeparator.TabIndex = 0;
            this.chkSeparator.Tag = "1";
            this.chkSeparator.Text = "Column separator";
            this.chkSeparator.UseVisualStyleBackColor = true;
            this.chkSeparator.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // cmbSeparator
            // 
            this.cmbSeparator.FormattingEnabled = true;
            this.cmbSeparator.Items.AddRange(new object[] {
            ",",
            "{Tab}",
            ";",
            "|",
            "{Fixed width}"});
            this.cmbSeparator.Location = new System.Drawing.Point(253, 48);
            this.cmbSeparator.Name = "cmbSeparator";
            this.cmbSeparator.Size = new System.Drawing.Size(83, 21);
            this.cmbSeparator.TabIndex = 1;
            this.cmbSeparator.Tag = "1";
            // 
            // chkDateTime
            // 
            this.chkDateTime.AutoSize = true;
            this.chkDateTime.Location = new System.Drawing.Point(12, 80);
            this.chkDateTime.Name = "chkDateTime";
            this.chkDateTime.Size = new System.Drawing.Size(100, 17);
            this.chkDateTime.TabIndex = 2;
            this.chkDateTime.Tag = "2";
            this.chkDateTime.Text = "Datetime format";
            this.chkDateTime.UseVisualStyleBackColor = true;
            this.chkDateTime.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // cmbDateTime
            // 
            this.cmbDateTime.FormattingEnabled = true;
            this.cmbDateTime.Items.AddRange(new object[] {
            "yyyy-MM-dd",
            "dd-MM-yyyy",
            "MM/dd/yyyy",
            "yyyy-MM-dd HH:mm:ss",
            "dd-MM-yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm:ss",
            "yyyy-M-d",
            "d-M-yyyy",
            "M/d/yyyy",
            "yyyy-M-d H:mm:ss.fff",
            "d-M-yyyy H:mm:ss.fff",
            "M/d/yyyy H:mm:ss.fff"});
            this.cmbDateTime.Location = new System.Drawing.Point(156, 80);
            this.cmbDateTime.Name = "cmbDateTime";
            this.cmbDateTime.Size = new System.Drawing.Size(180, 21);
            this.cmbDateTime.TabIndex = 3;
            this.cmbDateTime.Tag = "2";
            // 
            // chkDecimal
            // 
            this.chkDecimal.AutoSize = true;
            this.chkDecimal.Location = new System.Drawing.Point(12, 112);
            this.chkDecimal.Name = "chkDecimal";
            this.chkDecimal.Size = new System.Drawing.Size(111, 17);
            this.chkDecimal.TabIndex = 4;
            this.chkDecimal.Tag = "3";
            this.chkDecimal.Text = "Decimal separator";
            this.chkDecimal.UseVisualStyleBackColor = true;
            this.chkDecimal.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // cmbDecimal
            // 
            this.cmbDecimal.FormattingEnabled = true;
            this.cmbDecimal.Items.AddRange(new object[] {
            ".",
            ","});
            this.cmbDecimal.Location = new System.Drawing.Point(253, 112);
            this.cmbDecimal.Name = "cmbDecimal";
            this.cmbDecimal.Size = new System.Drawing.Size(83, 21);
            this.cmbDecimal.TabIndex = 5;
            this.cmbDecimal.Tag = "3";
            // 
            // chkReplaceCrLf
            // 
            this.chkReplaceCrLf.AutoSize = true;
            this.chkReplaceCrLf.Location = new System.Drawing.Point(12, 144);
            this.chkReplaceCrLf.Name = "chkReplaceCrLf";
            this.chkReplaceCrLf.Size = new System.Drawing.Size(174, 17);
            this.chkReplaceCrLf.TabIndex = 6;
            this.chkReplaceCrLf.Tag = "5";
            this.chkReplaceCrLf.Text = "Replace CrLf within values with";
            this.chkReplaceCrLf.UseVisualStyleBackColor = true;
            this.chkReplaceCrLf.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // txtReplaceCrLf
            // 
            this.txtReplaceCrLf.Location = new System.Drawing.Point(253, 144);
            this.txtReplaceCrLf.Name = "txtReplaceCrLf";
            this.txtReplaceCrLf.Size = new System.Drawing.Size(83, 20);
            this.txtReplaceCrLf.TabIndex = 7;
            this.txtReplaceCrLf.Tag = "5";
            // 
            // chkAlignVert
            // 
            this.chkAlignVert.AutoSize = true;
            this.chkAlignVert.Location = new System.Drawing.Point(12, 176);
            this.chkAlignVert.Name = "chkAlignVert";
            this.chkAlignVert.Size = new System.Drawing.Size(93, 17);
            this.chkAlignVert.TabIndex = 8;
            this.chkAlignVert.Tag = "7";
            this.chkAlignVert.Text = "Align vertically";
            this.chkAlignVert.UseVisualStyleBackColor = true;
            this.chkAlignVert.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.lblQuotes);
            this.grpGeneral.Controls.Add(this.lblTrimValues);
            this.grpGeneral.Controls.Add(this.chkTrimValues);
            this.grpGeneral.Controls.Add(this.cmbQuotes);
            this.grpGeneral.Location = new System.Drawing.Point(12, 209);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(328, 93);
            this.grpGeneral.TabIndex = 9;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General settings*";
            // 
            // lblTrimValues
            // 
            this.lblTrimValues.AutoSize = true;
            this.lblTrimValues.Location = new System.Drawing.Point(16, 32);
            this.lblTrimValues.Name = "lblTrimValues";
            this.lblTrimValues.Size = new System.Drawing.Size(74, 13);
            this.lblTrimValues.TabIndex = 18;
            this.lblTrimValues.Text = "Trim all values";
            // 
            // chkTrimValues
            // 
            this.chkTrimValues.AutoSize = true;
            this.chkTrimValues.Location = new System.Drawing.Point(148, 32);
            this.chkTrimValues.Name = "chkTrimValues";
            this.chkTrimValues.Size = new System.Drawing.Size(156, 17);
            this.chkTrimValues.TabIndex = 10;
            this.chkTrimValues.Tag = "6";
            this.chkTrimValues.Text = "Trim values (recommended)";
            this.chkTrimValues.UseVisualStyleBackColor = true;
            // 
            // lblQuotes
            // 
            this.lblQuotes.AutoSize = true;
            this.lblQuotes.Location = new System.Drawing.Point(16, 64);
            this.lblQuotes.Name = "lblQuotes";
            this.lblQuotes.Size = new System.Drawing.Size(84, 13);
            this.lblQuotes.TabIndex = 17;
            this.lblQuotes.Text = "Re-apply quotes";
            // 
            // cmbQuotes
            // 
            this.cmbQuotes.AutoCompleteCustomSource.AddRange(new string[] {
            "None / Minimal",
            "Space",
            "All string values",
            "All non-numeric values",
            "All values"});
            this.cmbQuotes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuotes.FormattingEnabled = true;
            this.cmbQuotes.Items.AddRange(new object[] {
            "None / Minimal",
            "Values with spaces",
            "All string values",
            "All non-numeric values",
            "All values"});
            this.cmbQuotes.Location = new System.Drawing.Point(144, 64);
            this.cmbQuotes.Name = "cmbQuotes";
            this.cmbQuotes.Size = new System.Drawing.Size(180, 21);
            this.cmbQuotes.TabIndex = 11;
            this.cmbQuotes.Tag = "4";
            // 
            // lblReformat
            // 
            this.lblReformat.AutoSize = true;
            this.lblReformat.Location = new System.Drawing.Point(12, 312);
            this.lblReformat.Name = "lblReformat";
            this.lblReformat.Size = new System.Drawing.Size(278, 13);
            this.lblReformat.TabIndex = 16;
            this.lblReformat.Text = "(note: always back-up your data files to prevent data loss)";
            // 
            // ReformatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 380);
            this.Controls.Add(this.lblReformat);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.chkAlignVert);
            this.Controls.Add(this.txtReplaceCrLf);
            this.Controls.Add(this.chkReplaceCrLf);
            this.Controls.Add(this.cmbDecimal);
            this.Controls.Add(this.chkDecimal);
            this.Controls.Add(this.cmbDateTime);
            this.Controls.Add(this.chkDateTime);
            this.Controls.Add(this.cmbSeparator);
            this.Controls.Add(this.chkSeparator);
            this.Name = "ReformatForm";
            this.Text = "Reformat data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReformatForm_FormClosing);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.chkSeparator, 0);
            this.Controls.SetChildIndex(this.cmbSeparator, 0);
            this.Controls.SetChildIndex(this.chkDateTime, 0);
            this.Controls.SetChildIndex(this.cmbDateTime, 0);
            this.Controls.SetChildIndex(this.chkDecimal, 0);
            this.Controls.SetChildIndex(this.cmbDecimal, 0);
            this.Controls.SetChildIndex(this.chkReplaceCrLf, 0);
            this.Controls.SetChildIndex(this.txtReplaceCrLf, 0);
            this.Controls.SetChildIndex(this.chkAlignVert, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.grpGeneral, 0);
            this.Controls.SetChildIndex(this.lblReformat, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkSeparator;
        private System.Windows.Forms.ComboBox cmbSeparator;
        private System.Windows.Forms.CheckBox chkDateTime;
        private System.Windows.Forms.ComboBox cmbDateTime;
        private System.Windows.Forms.CheckBox chkDecimal;
        private System.Windows.Forms.ComboBox cmbDecimal;
        private System.Windows.Forms.CheckBox chkReplaceCrLf;
        private System.Windows.Forms.TextBox txtReplaceCrLf;
        private System.Windows.Forms.CheckBox chkAlignVert;
        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.Label lblTrimValues;
        private System.Windows.Forms.CheckBox chkTrimValues;
        private System.Windows.Forms.Label lblQuotes;
        private System.Windows.Forms.ComboBox cmbQuotes;
        private System.Windows.Forms.Label lblReformat;
    }
}