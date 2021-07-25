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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkDateTime = new System.Windows.Forms.CheckBox();
            this.chkDecimal = new System.Windows.Forms.CheckBox();
            this.cmbDecimal = new System.Windows.Forms.ComboBox();
            this.cmbDateTime = new System.Windows.Forms.ComboBox();
            this.chkSeparator = new System.Windows.Forms.CheckBox();
            this.cmbSeparator = new System.Windows.Forms.ComboBox();
            this.lblReformat = new System.Windows.Forms.Label();
            this.chkTrimAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(13, 173);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 173);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkDateTime
            // 
            this.chkDateTime.AutoSize = true;
            this.chkDateTime.Location = new System.Drawing.Point(13, 41);
            this.chkDateTime.Name = "chkDateTime";
            this.chkDateTime.Size = new System.Drawing.Size(100, 17);
            this.chkDateTime.TabIndex = 0;
            this.chkDateTime.Tag = "0";
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
            this.chkDecimal.Tag = "1";
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
            this.cmbDecimal.Tag = "1";
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
            this.cmbDateTime.Tag = "0";
            // 
            // chkSeparator
            // 
            this.chkSeparator.AutoSize = true;
            this.chkSeparator.Location = new System.Drawing.Point(13, 105);
            this.chkSeparator.Name = "chkSeparator";
            this.chkSeparator.Size = new System.Drawing.Size(108, 17);
            this.chkSeparator.TabIndex = 4;
            this.chkSeparator.Tag = "2";
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
            this.cmbSeparator.Tag = "2";
            // 
            // lblReformat
            // 
            this.lblReformat.AutoSize = true;
            this.lblReformat.Location = new System.Drawing.Point(12, 12);
            this.lblReformat.Name = "lblReformat";
            this.lblReformat.Size = new System.Drawing.Size(324, 13);
            this.lblReformat.TabIndex = 8;
            this.lblReformat.Text = "Reformat (note: always back-up your data files to prevent data loss)";
            // 
            // chkTrimAll
            // 
            this.chkTrimAll.AutoSize = true;
            this.chkTrimAll.Location = new System.Drawing.Point(13, 137);
            this.chkTrimAll.Name = "chkTrimAll";
            this.chkTrimAll.Size = new System.Drawing.Size(93, 17);
            this.chkTrimAll.TabIndex = 6;
            this.chkTrimAll.Tag = "3";
            this.chkTrimAll.Text = "Trim all values";
            this.chkTrimAll.UseVisualStyleBackColor = true;
            this.chkTrimAll.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // ReformatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 216);
            this.Controls.Add(this.lblReformat);
            this.Controls.Add(this.cmbSeparator);
            this.Controls.Add(this.chkSeparator);
            this.Controls.Add(this.cmbDecimal);
            this.Controls.Add(this.chkDecimal);
            this.Controls.Add(this.cmbDateTime);
            this.Controls.Add(this.chkDateTime);
            this.Controls.Add(this.chkTrimAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ReformatForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reformat data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReformatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkDateTime;
        private System.Windows.Forms.CheckBox chkDecimal;
        private System.Windows.Forms.ComboBox cmbDecimal;
        private System.Windows.Forms.ComboBox cmbDateTime;
        private System.Windows.Forms.CheckBox chkSeparator;
        private System.Windows.Forms.ComboBox cmbSeparator;
        private System.Windows.Forms.Label lblReformat;
        private System.Windows.Forms.CheckBox chkTrimAll;
    }
}