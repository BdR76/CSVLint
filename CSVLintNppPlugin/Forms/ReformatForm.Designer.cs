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
            this.chkDateTime = new System.Windows.Forms.CheckBox();
            this.chkDecimal = new System.Windows.Forms.CheckBox();
            this.cmbDecimal = new System.Windows.Forms.ComboBox();
            this.cmbDateTime = new System.Windows.Forms.ComboBox();
            this.chkSeparator = new System.Windows.Forms.CheckBox();
            this.cmbSeparator = new System.Windows.Forms.ComboBox();
            this.lblReformat = new System.Windows.Forms.Label();
            this.chkTrimAll = new System.Windows.Forms.CheckBox();
            this.chkAlignVert = new System.Windows.Forms.CheckBox();
            this.chkApplyQuotes = new System.Windows.Forms.CheckBox();
            this.cmbQuotes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(117, 267);
            this.btnOk.TabIndex = 8;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 267);
            this.btnCancel.TabIndex = 9;
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(340, 2);
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
            // chkDateTime
            // 
            this.chkDateTime.AutoSize = true;
            this.chkDateTime.Location = new System.Drawing.Point(13, 48);
            this.chkDateTime.Name = "chkDateTime";
            this.chkDateTime.Size = new System.Drawing.Size(100, 17);
            this.chkDateTime.TabIndex = 0;
            this.chkDateTime.Tag = "1";
            this.chkDateTime.Text = "Datetime format";
            this.chkDateTime.UseVisualStyleBackColor = true;
            this.chkDateTime.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // chkDecimal
            // 
            this.chkDecimal.AutoSize = true;
            this.chkDecimal.Location = new System.Drawing.Point(13, 80);
            this.chkDecimal.Name = "chkDecimal";
            this.chkDecimal.Size = new System.Drawing.Size(111, 17);
            this.chkDecimal.TabIndex = 2;
            this.chkDecimal.Tag = "2";
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
            this.cmbDecimal.Location = new System.Drawing.Point(257, 80);
            this.cmbDecimal.Name = "cmbDecimal";
            this.cmbDecimal.Size = new System.Drawing.Size(83, 21);
            this.cmbDecimal.TabIndex = 3;
            this.cmbDecimal.Tag = "2";
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
            "yyyy-M-d H:mm:ss",
            "d-M-yyyy H:mm:ss",
            "M/d/yyyy H:mm:ss"});
            this.cmbDateTime.Location = new System.Drawing.Point(160, 48);
            this.cmbDateTime.Name = "cmbDateTime";
            this.cmbDateTime.Size = new System.Drawing.Size(180, 21);
            this.cmbDateTime.TabIndex = 1;
            this.cmbDateTime.Tag = "1";
            // 
            // chkSeparator
            // 
            this.chkSeparator.AutoSize = true;
            this.chkSeparator.Location = new System.Drawing.Point(13, 112);
            this.chkSeparator.Name = "chkSeparator";
            this.chkSeparator.Size = new System.Drawing.Size(108, 17);
            this.chkSeparator.TabIndex = 4;
            this.chkSeparator.Tag = "3";
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
            this.cmbSeparator.Location = new System.Drawing.Point(257, 112);
            this.cmbSeparator.Name = "cmbSeparator";
            this.cmbSeparator.Size = new System.Drawing.Size(83, 21);
            this.cmbSeparator.TabIndex = 5;
            this.cmbSeparator.Tag = "3";
            // 
            // lblReformat
            // 
            this.lblReformat.AutoSize = true;
            this.lblReformat.Location = new System.Drawing.Point(13, 240);
            this.lblReformat.Name = "lblReformat";
            this.lblReformat.Size = new System.Drawing.Size(278, 13);
            this.lblReformat.TabIndex = 8;
            this.lblReformat.Text = "(note: always back-up your data files to prevent data loss)";
            // 
            // chkTrimAll
            // 
            this.chkTrimAll.AutoSize = true;
            this.chkTrimAll.Location = new System.Drawing.Point(13, 176);
            this.chkTrimAll.Name = "chkTrimAll";
            this.chkTrimAll.Size = new System.Drawing.Size(93, 17);
            this.chkTrimAll.TabIndex = 6;
            this.chkTrimAll.Tag = "5";
            this.chkTrimAll.Text = "Trim all values";
            this.chkTrimAll.UseVisualStyleBackColor = true;
            this.chkTrimAll.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // chkAlignVert
            // 
            this.chkAlignVert.AutoSize = true;
            this.chkAlignVert.Location = new System.Drawing.Point(13, 208);
            this.chkAlignVert.Name = "chkAlignVert";
            this.chkAlignVert.Size = new System.Drawing.Size(187, 17);
            this.chkAlignVert.TabIndex = 7;
            this.chkAlignVert.Tag = "6";
            this.chkAlignVert.Text = "Align vertically (not recommended)";
            this.chkAlignVert.UseVisualStyleBackColor = true;
            this.chkAlignVert.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // chkApplyQuotes
            // 
            this.chkApplyQuotes.AutoSize = true;
            this.chkApplyQuotes.Location = new System.Drawing.Point(13, 144);
            this.chkApplyQuotes.Name = "chkApplyQuotes";
            this.chkApplyQuotes.Size = new System.Drawing.Size(103, 17);
            this.chkApplyQuotes.TabIndex = 6;
            this.chkApplyQuotes.Tag = "4";
            this.chkApplyQuotes.Text = "Re-apply quotes";
            this.chkApplyQuotes.UseVisualStyleBackColor = true;
            this.chkApplyQuotes.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
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
            this.cmbQuotes.Location = new System.Drawing.Point(160, 144);
            this.cmbQuotes.Name = "cmbQuotes";
            this.cmbQuotes.Size = new System.Drawing.Size(180, 21);
            this.cmbQuotes.TabIndex = 5;
            this.cmbQuotes.Tag = "4";
            // 
            // ReformatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 309);
            this.Controls.Add(this.lblReformat);
            this.Controls.Add(this.chkAlignVert);
            this.Controls.Add(this.chkApplyQuotes);
            this.Controls.Add(this.chkTrimAll);
            this.Controls.Add(this.cmbQuotes);
            this.Controls.Add(this.cmbSeparator);
            this.Controls.Add(this.chkSeparator);
            this.Controls.Add(this.cmbDecimal);
            this.Controls.Add(this.chkDecimal);
            this.Controls.Add(this.cmbDateTime);
            this.Controls.Add(this.chkDateTime);
            this.Name = "ReformatForm";
            this.Text = "Reformat data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReformatForm_FormClosing);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.chkDateTime, 0);
            this.Controls.SetChildIndex(this.cmbDateTime, 0);
            this.Controls.SetChildIndex(this.chkDecimal, 0);
            this.Controls.SetChildIndex(this.cmbDecimal, 0);
            this.Controls.SetChildIndex(this.chkSeparator, 0);
            this.Controls.SetChildIndex(this.cmbSeparator, 0);
            this.Controls.SetChildIndex(this.cmbQuotes, 0);
            this.Controls.SetChildIndex(this.chkTrimAll, 0);
            this.Controls.SetChildIndex(this.chkApplyQuotes, 0);
            this.Controls.SetChildIndex(this.chkAlignVert, 0);
            this.Controls.SetChildIndex(this.lblReformat, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkDateTime;
        private System.Windows.Forms.CheckBox chkDecimal;
        private System.Windows.Forms.ComboBox cmbDecimal;
        private System.Windows.Forms.ComboBox cmbDateTime;
        private System.Windows.Forms.CheckBox chkSeparator;
        private System.Windows.Forms.ComboBox cmbSeparator;
        private System.Windows.Forms.CheckBox chkTrimAll;
        private System.Windows.Forms.CheckBox chkAlignVert;
        private System.Windows.Forms.Label lblReformat;
        private System.Windows.Forms.CheckBox chkApplyQuotes;
        private System.Windows.Forms.ComboBox cmbQuotes;
    }
}