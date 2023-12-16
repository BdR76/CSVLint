
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
            this.cmbColumnSeparator = new System.Windows.Forms.ComboBox();
            this.lblFixedWidthPos = new System.Windows.Forms.Label();
            this.txtFixedWidthPos = new System.Windows.Forms.TextBox();
            this.btnFixedWidthPos = new System.Windows.Forms.Button();
            this.chkHeaderNames = new System.Windows.Forms.CheckBox();
            this.chkSkipLines = new System.Windows.Forms.CheckBox();
            this.numSkipLines = new System.Windows.Forms.NumericUpDown();
            this.chkCommentChar = new System.Windows.Forms.CheckBox();
            this.txtCommentChar = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipLines)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(192, 210);
            this.btnOk.TabIndex = 9;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(395, 8);
            this.picHelpIcon.Tag = "detect-columns";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(186, 20);
            this.lblTitle.Text = "Detect columns manually";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(415, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(311, 210);
            this.btnCancel.TabIndex = 10;
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
            this.cmbColumnSeparator.Location = new System.Drawing.Point(192, 45);
            this.cmbColumnSeparator.Name = "cmbColumnSeparator";
            this.cmbColumnSeparator.Size = new System.Drawing.Size(83, 21);
            this.cmbColumnSeparator.TabIndex = 1;
            this.cmbColumnSeparator.Tag = "1";
            this.cmbColumnSeparator.SelectedIndexChanged += new System.EventHandler(this.cmbColumnSeparator_SelectedIndexChanged);
            // 
            // lblFixedWidthPos
            // 
            this.lblFixedWidthPos.AutoSize = true;
            this.lblFixedWidthPos.Location = new System.Drawing.Point(12, 80);
            this.lblFixedWidthPos.Name = "lblFixedWidthPos";
            this.lblFixedWidthPos.Size = new System.Drawing.Size(153, 13);
            this.lblFixedWidthPos.TabIndex = 0;
            this.lblFixedWidthPos.Tag = "1";
            this.lblFixedWidthPos.Text = "Column end positions (optional)";
            // 
            // txtFixedWidthPos
            // 
            this.txtFixedWidthPos.Enabled = false;
            this.txtFixedWidthPos.Location = new System.Drawing.Point(192, 77);
            this.txtFixedWidthPos.Name = "txtFixedWidthPos";
            this.txtFixedWidthPos.Size = new System.Drawing.Size(186, 20);
            this.txtFixedWidthPos.TabIndex = 2;
            this.txtFixedWidthPos.Tag = "1";
            // 
            // btnFixedWidthPos
            // 
            this.btnFixedWidthPos.Location = new System.Drawing.Point(384, 77);
            this.btnFixedWidthPos.Name = "btnFixedWidthPos";
            this.btnFixedWidthPos.Size = new System.Drawing.Size(30, 23);
            this.btnFixedWidthPos.TabIndex = 3;
            this.btnFixedWidthPos.Tag = "1";
            this.btnFixedWidthPos.Text = "..";
            this.btnFixedWidthPos.UseVisualStyleBackColor = true;
            this.btnFixedWidthPos.Click += new System.EventHandler(this.btnFixedWidthPos_Click);
            // 
            // chkHeaderNames
            // 
            this.chkHeaderNames.AutoSize = true;
            this.chkHeaderNames.Location = new System.Drawing.Point(12, 109);
            this.chkHeaderNames.Name = "chkHeaderNames";
            this.chkHeaderNames.Size = new System.Drawing.Size(250, 17);
            this.chkHeaderNames.TabIndex = 4;
            this.chkHeaderNames.Tag = "2";
            this.chkHeaderNames.Text = "Header names, first line contains column names";
            this.chkHeaderNames.UseVisualStyleBackColor = true;
            // 
            // chkSkipLines
            // 
            this.chkSkipLines.AutoSize = true;
            this.chkSkipLines.Location = new System.Drawing.Point(12, 141);
            this.chkSkipLines.Name = "chkSkipLines";
            this.chkSkipLines.Size = new System.Drawing.Size(90, 17);
            this.chkSkipLines.TabIndex = 5;
            this.chkSkipLines.Tag = "3";
            this.chkSkipLines.Text = "Skip first lines";
            this.chkSkipLines.UseVisualStyleBackColor = true;
            this.chkSkipLines.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // numSkipLines
            // 
            this.numSkipLines.Location = new System.Drawing.Point(192, 139);
            this.numSkipLines.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numSkipLines.Name = "numSkipLines";
            this.numSkipLines.Size = new System.Drawing.Size(83, 20);
            this.numSkipLines.TabIndex = 6;
            this.numSkipLines.Tag = "3";
            // 
            // chkCommentChar
            // 
            this.chkCommentChar.AutoSize = true;
            this.chkCommentChar.Location = new System.Drawing.Point(12, 173);
            this.chkCommentChar.Name = "chkCommentChar";
            this.chkCommentChar.Size = new System.Drawing.Size(139, 17);
            this.chkCommentChar.TabIndex = 7;
            this.chkCommentChar.Tag = "4";
            this.chkCommentChar.Text = "Comment lines start with";
            this.chkCommentChar.UseVisualStyleBackColor = true;
            this.chkCommentChar.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // txtCommentChar
            // 
            this.txtCommentChar.Location = new System.Drawing.Point(192, 171);
            this.txtCommentChar.Name = "txtCommentChar";
            this.txtCommentChar.Size = new System.Drawing.Size(83, 20);
            this.txtCommentChar.TabIndex = 8;
            this.txtCommentChar.Tag = "4";
            this.txtCommentChar.Text = " ";
            // 
            // DetectColumnsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(423, 252);
            this.Controls.Add(this.numSkipLines);
            this.Controls.Add(this.chkSkipLines);
            this.Controls.Add(this.chkCommentChar);
            this.Controls.Add(this.chkHeaderNames);
            this.Controls.Add(this.txtCommentChar);
            this.Controls.Add(this.btnFixedWidthPos);
            this.Controls.Add(this.txtFixedWidthPos);
            this.Controls.Add(this.lblFixedWidthPos);
            this.Controls.Add(this.cmbColumnSeparator);
            this.Controls.Add(this.lblColumnSeparator);
            this.Name = "DetectColumnsForm";
            this.Text = "Detect columns manually";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DetectColumnsForm_FormClosing);
            this.Load += new System.EventHandler(this.DetectColumnsForm_Load);
            this.Controls.SetChildIndex(this.lblColumnSeparator, 0);
            this.Controls.SetChildIndex(this.cmbColumnSeparator, 0);
            this.Controls.SetChildIndex(this.lblFixedWidthPos, 0);
            this.Controls.SetChildIndex(this.txtFixedWidthPos, 0);
            this.Controls.SetChildIndex(this.btnFixedWidthPos, 0);
            this.Controls.SetChildIndex(this.txtCommentChar, 0);
            this.Controls.SetChildIndex(this.chkHeaderNames, 0);
            this.Controls.SetChildIndex(this.chkCommentChar, 0);
            this.Controls.SetChildIndex(this.chkSkipLines, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.numSkipLines, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblColumnSeparator;
        private System.Windows.Forms.ComboBox cmbColumnSeparator;
        private System.Windows.Forms.Label lblFixedWidthPos;
        private System.Windows.Forms.TextBox txtFixedWidthPos;
        private System.Windows.Forms.Button btnFixedWidthPos;
        private System.Windows.Forms.CheckBox chkHeaderNames;
        private System.Windows.Forms.CheckBox chkSkipLines;
        private System.Windows.Forms.NumericUpDown numSkipLines;
        private System.Windows.Forms.CheckBox chkCommentChar;
        private System.Windows.Forms.TextBox txtCommentChar;
    }
}
