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
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(112, 20);
            this.lblTitle.Text = "Reformat data";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(128, 230);
            this.btnOk.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(247, 230);
            this.btnCancel.TabIndex = 9;
            // 
            // chkDateTime
            // 
            this.chkDateTime.AutoSize = true;
            this.chkDateTime.Location = new System.Drawing.Point(13, 41);
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
            this.chkDecimal.Location = new System.Drawing.Point(13, 73);
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
            this.cmbDecimal.Location = new System.Drawing.Point(257, 73);
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
            this.cmbDateTime.Location = new System.Drawing.Point(160, 41);
            this.cmbDateTime.Name = "cmbDateTime";
            this.cmbDateTime.Size = new System.Drawing.Size(180, 21);
            this.cmbDateTime.TabIndex = 1;
            this.cmbDateTime.Tag = "1";
            // 
            // chkSeparator
            // 
            this.chkSeparator.AutoSize = true;
            this.chkSeparator.Location = new System.Drawing.Point(13, 105);
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
            this.cmbSeparator.Location = new System.Drawing.Point(257, 105);
            this.cmbSeparator.Name = "cmbSeparator";
            this.cmbSeparator.Size = new System.Drawing.Size(83, 21);
            this.cmbSeparator.TabIndex = 5;
            this.cmbSeparator.Tag = "3";
            // 
            // lblReformat
            // 
            this.lblReformat.AutoSize = true;
            this.lblReformat.Location = new System.Drawing.Point(13, 201);
            this.lblReformat.Name = "lblReformat";
            this.lblReformat.Size = new System.Drawing.Size(278, 13);
            this.lblReformat.TabIndex = 8;
            this.lblReformat.Text = "(note: always back-up your data files to prevent data loss)";
            // 
            // chkTrimAll
            // 
            this.chkTrimAll.AutoSize = true;
            this.chkTrimAll.Location = new System.Drawing.Point(13, 137);
            this.chkTrimAll.Name = "chkTrimAll";
            this.chkTrimAll.Size = new System.Drawing.Size(93, 17);
            this.chkTrimAll.TabIndex = 6;
            this.chkTrimAll.Tag = "4";
            this.chkTrimAll.Text = "Trim all values";
            this.chkTrimAll.UseVisualStyleBackColor = true;
            this.chkTrimAll.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // chkAlignVert
            // 
            this.chkAlignVert.AutoSize = true;
            this.chkAlignVert.Location = new System.Drawing.Point(12, 169);
            this.chkAlignVert.Name = "chkAlignVert";
            this.chkAlignVert.Size = new System.Drawing.Size(187, 17);
            this.chkAlignVert.TabIndex = 7;
            this.chkAlignVert.Tag = "5";
            this.chkAlignVert.Text = "Align vertically (not recommended)";
            this.chkAlignVert.UseVisualStyleBackColor = true;
            this.chkAlignVert.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // ReformatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 272);
            this.Controls.Add(this.lblReformat);
            this.Controls.Add(this.chkAlignVert);
            this.Controls.Add(this.chkTrimAll);
            this.Controls.Add(this.cmbSeparator);
            this.Controls.Add(this.chkSeparator);
            this.Controls.Add(this.cmbDecimal);
            this.Controls.Add(this.chkDecimal);
            this.Controls.Add(this.cmbDateTime);
            this.Controls.Add(this.chkDateTime);
            this.Name = "ReformatForm";
            this.Text = "Reformat data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReformatForm_FormClosing);
            this.Controls.SetChildIndex(this.chkDateTime, 0);
            this.Controls.SetChildIndex(this.cmbDateTime, 0);
            this.Controls.SetChildIndex(this.chkDecimal, 0);
            this.Controls.SetChildIndex(this.cmbDecimal, 0);
            this.Controls.SetChildIndex(this.chkSeparator, 0);
            this.Controls.SetChildIndex(this.cmbSeparator, 0);
            this.Controls.SetChildIndex(this.chkTrimAll, 0);
            this.Controls.SetChildIndex(this.chkAlignVert, 0);
            this.Controls.SetChildIndex(this.lblReformat, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
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
    }
}