
namespace CSVLintNppPlugin.Forms
{
    partial class DataConvertForm
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
            this.lblConvertTo = new System.Windows.Forms.Label();
            this.rdbtnSQL = new System.Windows.Forms.RadioButton();
            this.rdbtnXML = new System.Windows.Forms.RadioButton();
            this.rdbtnJSON = new System.Windows.Forms.RadioButton();
            this.rdbtnHTML = new System.Windows.Forms.RadioButton();
            this.cmbSQLtype = new System.Windows.Forms.ComboBox();
            this.lblSQLtype = new System.Windows.Forms.Label();
            this.lblBatchSize = new System.Windows.Forms.Label();
            this.numBatchSize = new System.Windows.Forms.NumericUpDown();
            this.txtTablename = new System.Windows.Forms.TextBox();
            this.lblTablename = new System.Windows.Forms.Label();
            this.cmbCSSstyle = new System.Windows.Forms.ComboBox();
            this.lblCSSstyle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBatchSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(225, 223);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(429, 8);
            this.picHelpIcon.Tag = "convert-data";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(100, 20);
            this.lblTitle.Text = "Convert data";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(449, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(344, 223);
            // 
            // lblConvertTo
            // 
            this.lblConvertTo.AutoSize = true;
            this.lblConvertTo.Location = new System.Drawing.Point(12, 48);
            this.lblConvertTo.Name = "lblConvertTo";
            this.lblConvertTo.Size = new System.Drawing.Size(80, 13);
            this.lblConvertTo.TabIndex = 3;
            this.lblConvertTo.Text = "Convert data to";
            // 
            // rdbtnSQL
            // 
            this.rdbtnSQL.AutoSize = true;
            this.rdbtnSQL.Location = new System.Drawing.Point(192, 46);
            this.rdbtnSQL.Name = "rdbtnSQL";
            this.rdbtnSQL.Size = new System.Drawing.Size(46, 17);
            this.rdbtnSQL.TabIndex = 4;
            this.rdbtnSQL.TabStop = true;
            this.rdbtnSQL.Tag = "1";
            this.rdbtnSQL.Text = "SQL";
            this.rdbtnSQL.UseVisualStyleBackColor = true;
            this.rdbtnSQL.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnXML
            // 
            this.rdbtnXML.AutoSize = true;
            this.rdbtnXML.Location = new System.Drawing.Point(256, 46);
            this.rdbtnXML.Name = "rdbtnXML";
            this.rdbtnXML.Size = new System.Drawing.Size(47, 17);
            this.rdbtnXML.TabIndex = 5;
            this.rdbtnXML.TabStop = true;
            this.rdbtnXML.Tag = "2";
            this.rdbtnXML.Text = "XML";
            this.rdbtnXML.UseVisualStyleBackColor = true;
            this.rdbtnXML.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnJSON
            // 
            this.rdbtnJSON.AutoSize = true;
            this.rdbtnJSON.Location = new System.Drawing.Point(320, 46);
            this.rdbtnJSON.Name = "rdbtnJSON";
            this.rdbtnJSON.Size = new System.Drawing.Size(53, 17);
            this.rdbtnJSON.TabIndex = 6;
            this.rdbtnJSON.TabStop = true;
            this.rdbtnJSON.Tag = "3";
            this.rdbtnJSON.Text = "JSON";
            this.rdbtnJSON.UseVisualStyleBackColor = true;
            this.rdbtnJSON.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnHTML
            // 
            this.rdbtnHTML.AutoSize = true;
            this.rdbtnHTML.Location = new System.Drawing.Point(384, 46);
            this.rdbtnHTML.Name = "rdbtnHTML";
            this.rdbtnHTML.Size = new System.Drawing.Size(55, 17);
            this.rdbtnHTML.TabIndex = 7;
            this.rdbtnHTML.TabStop = true;
            this.rdbtnHTML.Tag = "4";
            this.rdbtnHTML.Text = "HTML";
            this.rdbtnHTML.UseVisualStyleBackColor = true;
            this.rdbtnHTML.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // cmbSQLtype
            // 
            this.cmbSQLtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSQLtype.Enabled = false;
            this.cmbSQLtype.FormattingEnabled = true;
            this.cmbSQLtype.Items.AddRange(new object[] {
            "MySQL / MariaDB",
            "MS-SQL",
            "PostgreSQL"});
            this.cmbSQLtype.Location = new System.Drawing.Point(192, 109);
            this.cmbSQLtype.Name = "cmbSQLtype";
            this.cmbSQLtype.Size = new System.Drawing.Size(192, 21);
            this.cmbSQLtype.TabIndex = 9;
            this.cmbSQLtype.Tag = "1";
            // 
            // lblSQLtype
            // 
            this.lblSQLtype.AutoSize = true;
            this.lblSQLtype.Location = new System.Drawing.Point(12, 112);
            this.lblSQLtype.Name = "lblSQLtype";
            this.lblSQLtype.Size = new System.Drawing.Size(76, 13);
            this.lblSQLtype.TabIndex = 7;
            this.lblSQLtype.Tag = "";
            this.lblSQLtype.Text = "Database type";
            // 
            // lblBatchSize
            // 
            this.lblBatchSize.AutoSize = true;
            this.lblBatchSize.Location = new System.Drawing.Point(12, 144);
            this.lblBatchSize.Name = "lblBatchSize";
            this.lblBatchSize.Size = new System.Drawing.Size(56, 13);
            this.lblBatchSize.TabIndex = 9;
            this.lblBatchSize.Tag = "";
            this.lblBatchSize.Text = "Batch size";
            // 
            // numBatchSize
            // 
            this.numBatchSize.Enabled = false;
            this.numBatchSize.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numBatchSize.Location = new System.Drawing.Point(192, 141);
            this.numBatchSize.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numBatchSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBatchSize.Name = "numBatchSize";
            this.numBatchSize.Size = new System.Drawing.Size(111, 20);
            this.numBatchSize.TabIndex = 10;
            this.numBatchSize.Tag = "1,4";
            this.numBatchSize.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // txtTablename
            // 
            this.txtTablename.Enabled = false;
            this.txtTablename.Location = new System.Drawing.Point(192, 77);
            this.txtTablename.Name = "txtTablename";
            this.txtTablename.Size = new System.Drawing.Size(192, 20);
            this.txtTablename.TabIndex = 8;
            this.txtTablename.Tag = "1,2";
            // 
            // lblTablename
            // 
            this.lblTablename.AutoSize = true;
            this.lblTablename.Location = new System.Drawing.Point(12, 80);
            this.lblTablename.Name = "lblTablename";
            this.lblTablename.Size = new System.Drawing.Size(117, 13);
            this.lblTablename.TabIndex = 5;
            this.lblTablename.Tag = "";
            this.lblTablename.Text = "Table name / XML root";
            // 
            // cmbCSSstyle
            // 
            this.cmbCSSstyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCSSstyle.Enabled = false;
            this.cmbCSSstyle.FormattingEnabled = true;
            this.cmbCSSstyle.Items.AddRange(new object[] {
            "(None)",
            "Minimal UI",
            "Zebra rows",
            "Zebra columns",
            "CSV Lint colors"});
            this.cmbCSSstyle.Location = new System.Drawing.Point(192, 173);
            this.cmbCSSstyle.Name = "cmbCSSstyle";
            this.cmbCSSstyle.Size = new System.Drawing.Size(192, 21);
            this.cmbCSSstyle.TabIndex = 11;
            this.cmbCSSstyle.Tag = "4";
            // 
            // lblCSSstyle
            // 
            this.lblCSSstyle.AutoSize = true;
            this.lblCSSstyle.Location = new System.Drawing.Point(12, 176);
            this.lblCSSstyle.Name = "lblCSSstyle";
            this.lblCSSstyle.Size = new System.Drawing.Size(87, 13);
            this.lblCSSstyle.TabIndex = 11;
            this.lblCSSstyle.Tag = "";
            this.lblCSSstyle.Text = "HTML CSS Style";
            // 
            // DataConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(456, 265);
            this.Controls.Add(this.cmbCSSstyle);
            this.Controls.Add(this.lblCSSstyle);
            this.Controls.Add(this.rdbtnHTML);
            this.Controls.Add(this.txtTablename);
            this.Controls.Add(this.numBatchSize);
            this.Controls.Add(this.cmbSQLtype);
            this.Controls.Add(this.rdbtnJSON);
            this.Controls.Add(this.rdbtnXML);
            this.Controls.Add(this.rdbtnSQL);
            this.Controls.Add(this.lblBatchSize);
            this.Controls.Add(this.lblTablename);
            this.Controls.Add(this.lblSQLtype);
            this.Controls.Add(this.lblConvertTo);
            this.Name = "DataConvertForm";
            this.Text = "Convert data";
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblConvertTo, 0);
            this.Controls.SetChildIndex(this.lblSQLtype, 0);
            this.Controls.SetChildIndex(this.lblTablename, 0);
            this.Controls.SetChildIndex(this.lblBatchSize, 0);
            this.Controls.SetChildIndex(this.rdbtnSQL, 0);
            this.Controls.SetChildIndex(this.rdbtnXML, 0);
            this.Controls.SetChildIndex(this.rdbtnJSON, 0);
            this.Controls.SetChildIndex(this.cmbSQLtype, 0);
            this.Controls.SetChildIndex(this.numBatchSize, 0);
            this.Controls.SetChildIndex(this.txtTablename, 0);
            this.Controls.SetChildIndex(this.rdbtnHTML, 0);
            this.Controls.SetChildIndex(this.lblCSSstyle, 0);
            this.Controls.SetChildIndex(this.cmbCSSstyle, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBatchSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConvertTo;
        private System.Windows.Forms.RadioButton rdbtnSQL;
        private System.Windows.Forms.RadioButton rdbtnXML;
        private System.Windows.Forms.RadioButton rdbtnJSON;
        private System.Windows.Forms.RadioButton rdbtnHTML;
        private System.Windows.Forms.Label lblTablename;
        private System.Windows.Forms.TextBox txtTablename;
        private System.Windows.Forms.Label lblSQLtype;
        private System.Windows.Forms.ComboBox cmbSQLtype;
        private System.Windows.Forms.Label lblBatchSize;
        private System.Windows.Forms.NumericUpDown numBatchSize;
        private System.Windows.Forms.Label lblCSSstyle;
        private System.Windows.Forms.ComboBox cmbCSSstyle;
    }
}
