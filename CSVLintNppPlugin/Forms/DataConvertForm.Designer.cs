
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
            this.cmbSQLtype = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(142, 142);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(261, 142);
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(366, 2);
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(100, 20);
            this.lblTitle.Text = "Convert data";
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(346, 8);
            // 
            // lblConvertTo
            // 
            this.lblConvertTo.AutoSize = true;
            this.lblConvertTo.Location = new System.Drawing.Point(13, 46);
            this.lblConvertTo.Name = "lblConvertTo";
            this.lblConvertTo.Size = new System.Drawing.Size(56, 13);
            this.lblConvertTo.TabIndex = 3;
            this.lblConvertTo.Text = "Convert to";
            // 
            // rdbtnSQL
            // 
            this.rdbtnSQL.AutoSize = true;
            this.rdbtnSQL.Location = new System.Drawing.Point(76, 46);
            this.rdbtnSQL.Name = "rdbtnSQL";
            this.rdbtnSQL.Size = new System.Drawing.Size(74, 17);
            this.rdbtnSQL.TabIndex = 4;
            this.rdbtnSQL.TabStop = true;
            this.rdbtnSQL.Text = "SQL insert";
            this.rdbtnSQL.UseVisualStyleBackColor = true;
            // 
            // rdbtnXML
            // 
            this.rdbtnXML.AutoSize = true;
            this.rdbtnXML.Location = new System.Drawing.Point(76, 69);
            this.rdbtnXML.Name = "rdbtnXML";
            this.rdbtnXML.Size = new System.Drawing.Size(47, 17);
            this.rdbtnXML.TabIndex = 4;
            this.rdbtnXML.TabStop = true;
            this.rdbtnXML.Text = "XML";
            this.rdbtnXML.UseVisualStyleBackColor = true;
            // 
            // rdbtnJSON
            // 
            this.rdbtnJSON.AutoSize = true;
            this.rdbtnJSON.Location = new System.Drawing.Point(76, 92);
            this.rdbtnJSON.Name = "rdbtnJSON";
            this.rdbtnJSON.Size = new System.Drawing.Size(53, 17);
            this.rdbtnJSON.TabIndex = 4;
            this.rdbtnJSON.TabStop = true;
            this.rdbtnJSON.Text = "JSON";
            this.rdbtnJSON.UseVisualStyleBackColor = true;
            // 
            // cmbSQLtype
            // 
            this.cmbSQLtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSQLtype.FormattingEnabled = true;
            this.cmbSQLtype.Items.AddRange(new object[] {
            "MySQL",
            "MS-SQL",
            "PostgreSQL"});
            this.cmbSQLtype.Location = new System.Drawing.Point(237, 46);
            this.cmbSQLtype.Name = "cmbSQLtype";
            this.cmbSQLtype.Size = new System.Drawing.Size(121, 21);
            this.cmbSQLtype.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(157, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "database type";
            // 
            // DataConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(373, 184);
            this.Controls.Add(this.cmbSQLtype);
            this.Controls.Add(this.rdbtnJSON);
            this.Controls.Add(this.rdbtnXML);
            this.Controls.Add(this.rdbtnSQL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblConvertTo);
            this.Name = "DataConvertForm";
            this.Text = "Convert data";
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblConvertTo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.rdbtnSQL, 0);
            this.Controls.SetChildIndex(this.rdbtnXML, 0);
            this.Controls.SetChildIndex(this.rdbtnJSON, 0);
            this.Controls.SetChildIndex(this.cmbSQLtype, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConvertTo;
        private System.Windows.Forms.RadioButton rdbtnSQL;
        private System.Windows.Forms.RadioButton rdbtnXML;
        private System.Windows.Forms.RadioButton rdbtnJSON;
        private System.Windows.Forms.ComboBox cmbSQLtype;
        private System.Windows.Forms.Label label1;
    }
}
