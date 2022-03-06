
namespace CSVLintNppPlugin.Forms
{
    partial class MetaDataGenerateForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.rdbtnSchemaIni = new System.Windows.Forms.RadioButton();
            this.rdbtnSchemaJSON = new System.Windows.Forms.RadioButton();
            this.rdbtnRScript = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(93, 130);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 130);
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(209, 20);
            this.lblTitle.Text = "Generate metadata or script";
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Tag = "generate-metadata";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(316, 2);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Metadata type";
            // 
            // rdbtnSchemaIni
            // 
            this.rdbtnSchemaIni.AutoSize = true;
            this.rdbtnSchemaIni.Location = new System.Drawing.Point(104, 48);
            this.rdbtnSchemaIni.Name = "rdbtnSchemaIni";
            this.rdbtnSchemaIni.Size = new System.Drawing.Size(77, 17);
            this.rdbtnSchemaIni.TabIndex = 3;
            this.rdbtnSchemaIni.TabStop = true;
            this.rdbtnSchemaIni.Text = "Schema ini";
            this.rdbtnSchemaIni.UseVisualStyleBackColor = true;
            // 
            // rdbtnSchemaJSON
            // 
            this.rdbtnSchemaJSON.AutoSize = true;
            this.rdbtnSchemaJSON.Location = new System.Drawing.Point(104, 72);
            this.rdbtnSchemaJSON.Name = "rdbtnSchemaJSON";
            this.rdbtnSchemaJSON.Size = new System.Drawing.Size(95, 17);
            this.rdbtnSchemaJSON.TabIndex = 4;
            this.rdbtnSchemaJSON.TabStop = true;
            this.rdbtnSchemaJSON.Text = "Schema JSON";
            this.rdbtnSchemaJSON.UseVisualStyleBackColor = true;
            // 
            // rdbtnRScript
            // 
            this.rdbtnRScript.AutoSize = true;
            this.rdbtnRScript.Location = new System.Drawing.Point(104, 96);
            this.rdbtnRScript.Name = "rdbtnRScript";
            this.rdbtnRScript.Size = new System.Drawing.Size(61, 17);
            this.rdbtnRScript.TabIndex = 5;
            this.rdbtnRScript.TabStop = true;
            this.rdbtnRScript.Text = "R-script";
            this.rdbtnRScript.UseVisualStyleBackColor = true;
            // 
            // MetaDataGenerateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(324, 172);
            this.Controls.Add(this.rdbtnRScript);
            this.Controls.Add(this.rdbtnSchemaJSON);
            this.Controls.Add(this.rdbtnSchemaIni);
            this.Controls.Add(this.label1);
            this.Name = "MetaDataGenerateForm";
            this.Text = "Generate metadata";
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.rdbtnSchemaIni, 0);
            this.Controls.SetChildIndex(this.rdbtnSchemaJSON, 0);
            this.Controls.SetChildIndex(this.rdbtnRScript, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbtnSchemaIni;
        private System.Windows.Forms.RadioButton rdbtnSchemaJSON;
        private System.Windows.Forms.RadioButton rdbtnRScript;
    }
}
