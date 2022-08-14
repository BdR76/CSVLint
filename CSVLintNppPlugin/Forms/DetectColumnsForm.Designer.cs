﻿
namespace CSVLintNppPlugin.Forms
{
    partial class DetectColumnsForm
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
            this.lblColumnSeparator = new System.Windows.Forms.Label();
            this.lblHeaderNames = new System.Windows.Forms.Label();
            this.chkHeaderNames = new System.Windows.Forms.CheckBox();
            this.cmbColumnSeparator = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(102, 119);
            this.btnOk.TabIndex = 3;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(221, 119);
            this.btnCancel.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(186, 20);
            this.lblTitle.Text = "Detect columns manually";
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(305, 8);
            this.picHelpIcon.Tag = "detect-columns";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(325, 2);
            // 
            // lblColumnSeparator
            // 
            this.lblColumnSeparator.AutoSize = true;
            this.lblColumnSeparator.Location = new System.Drawing.Point(12, 48);
            this.lblColumnSeparator.Name = "lblColumnSeparator";
            this.lblColumnSeparator.Size = new System.Drawing.Size(89, 13);
            this.lblColumnSeparator.TabIndex = 0;
            this.lblColumnSeparator.Text = "Column separator";
            // 
            // cmbColumnSeparator
            // 
            this.cmbColumnSeparator.FormattingEnabled = true;
            this.cmbColumnSeparator.Items.AddRange(new object[] {
            ",",
            "{Tab}",
            ";",
            "|",
            "{Fixed width}"});
            this.cmbColumnSeparator.Location = new System.Drawing.Point(142, 45);
            this.cmbColumnSeparator.Name = "cmbColumnSeparator";
            this.cmbColumnSeparator.Size = new System.Drawing.Size(83, 21);
            this.cmbColumnSeparator.TabIndex = 1;
            // 
            // lblHeaderNames
            // 
            this.lblHeaderNames.AutoSize = true;
            this.lblHeaderNames.Location = new System.Drawing.Point(12, 80);
            this.lblHeaderNames.Name = "lblHeaderNames";
            this.lblHeaderNames.Size = new System.Drawing.Size(76, 13);
            this.lblHeaderNames.TabIndex = 0;
            this.lblHeaderNames.Text = "Header names";
            // 
            // chkHeaderNames
            // 
            this.chkHeaderNames.AutoSize = true;
            this.chkHeaderNames.Location = new System.Drawing.Point(142, 79);
            this.chkHeaderNames.Name = "chkHeaderNames";
            this.chkHeaderNames.Size = new System.Drawing.Size(178, 17);
            this.chkHeaderNames.TabIndex = 2;
            this.chkHeaderNames.Text = "First line contains column names";
            this.chkHeaderNames.UseVisualStyleBackColor = true;
            // 
            // DetectColumnsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(333, 161);
            this.Controls.Add(this.cmbColumnSeparator);
            this.Controls.Add(this.chkHeaderNames);
            this.Controls.Add(this.lblHeaderNames);
            this.Controls.Add(this.lblColumnSeparator);
            this.Name = "DetectColumnsForm";
            this.Text = "Detect columns manually";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DetectColumnsForm_FormClosing);
            this.Controls.SetChildIndex(this.lblColumnSeparator, 0);
            this.Controls.SetChildIndex(this.lblHeaderNames, 0);
            this.Controls.SetChildIndex(this.chkHeaderNames, 0);
            this.Controls.SetChildIndex(this.cmbColumnSeparator, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblColumnSeparator;
        private System.Windows.Forms.ComboBox cmbColumnSeparator;
        private System.Windows.Forms.Label lblHeaderNames;
        private System.Windows.Forms.CheckBox chkHeaderNames;
    }
}