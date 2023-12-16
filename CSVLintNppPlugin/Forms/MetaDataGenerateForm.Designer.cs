
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
            this.lblMetadataType = new System.Windows.Forms.Label();
            this.rdbtnSchemaIni = new System.Windows.Forms.RadioButton();
            this.rdbtnSchemaJSON = new System.Windows.Forms.RadioButton();
            this.rdbtnDatadictCSV = new System.Windows.Forms.RadioButton();
            this.rdbtnPython = new System.Windows.Forms.RadioButton();
            this.rdbtnRScript = new System.Windows.Forms.RadioButton();
            this.rdbtnPowerShell = new System.Windows.Forms.RadioButton();
            this.lblScripts = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(93, 206);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Tag = "generate-metadata";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(209, 20);
            this.lblTitle.Text = "Generate metadata or script";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(316, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 206);
            // 
            // lblMetadataType
            // 
            this.lblMetadataType.AutoSize = true;
            this.lblMetadataType.Location = new System.Drawing.Point(13, 48);
            this.lblMetadataType.Name = "lblMetadataType";
            this.lblMetadataType.Size = new System.Drawing.Size(98, 13);
            this.lblMetadataType.TabIndex = 3;
            this.lblMetadataType.Text = "Generate metadata";
            // 
            // rdbtnSchemaIni
            // 
            this.rdbtnSchemaIni.AutoSize = true;
            this.rdbtnSchemaIni.Location = new System.Drawing.Point(120, 48);
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
            this.rdbtnSchemaJSON.Location = new System.Drawing.Point(120, 72);
            this.rdbtnSchemaJSON.Name = "rdbtnSchemaJSON";
            this.rdbtnSchemaJSON.Size = new System.Drawing.Size(144, 17);
            this.rdbtnSchemaJSON.TabIndex = 4;
            this.rdbtnSchemaJSON.TabStop = true;
            this.rdbtnSchemaJSON.Text = "W3C CSV schema JSON";
            this.rdbtnSchemaJSON.UseVisualStyleBackColor = true;
            // 
            // rdbtnDatadictCSV
            // 
            this.rdbtnDatadictCSV.AutoSize = true;
            this.rdbtnDatadictCSV.Location = new System.Drawing.Point(120, 96);
            this.rdbtnDatadictCSV.Name = "rdbtnDatadictCSV";
            this.rdbtnDatadictCSV.Size = new System.Drawing.Size(117, 17);
            this.rdbtnDatadictCSV.TabIndex = 5;
            this.rdbtnDatadictCSV.TabStop = true;
            this.rdbtnDatadictCSV.Text = "Datadictionary CSV";
            this.rdbtnDatadictCSV.UseVisualStyleBackColor = true;
            // 
            // rdbtnPython
            // 
            this.rdbtnPython.AutoSize = true;
            this.rdbtnPython.Location = new System.Drawing.Point(120, 120);
            this.rdbtnPython.Name = "rdbtnPython";
            this.rdbtnPython.Size = new System.Drawing.Size(86, 17);
            this.rdbtnPython.TabIndex = 6;
            this.rdbtnPython.TabStop = true;
            this.rdbtnPython.Text = "Python script";
            this.rdbtnPython.UseVisualStyleBackColor = true;
            // 
            // rdbtnRScript
            // 
            this.rdbtnRScript.AutoSize = true;
            this.rdbtnRScript.Location = new System.Drawing.Point(120, 144);
            this.rdbtnRScript.Name = "rdbtnRScript";
            this.rdbtnRScript.Size = new System.Drawing.Size(61, 17);
            this.rdbtnRScript.TabIndex = 7;
            this.rdbtnRScript.TabStop = true;
            this.rdbtnRScript.Text = "R-script";
            this.rdbtnRScript.UseVisualStyleBackColor = true;
            // 
            // rdbtnPowerShell
            // 
            this.rdbtnPowerShell.AutoSize = true;
            this.rdbtnPowerShell.Location = new System.Drawing.Point(120, 168);
            this.rdbtnPowerShell.Name = "rdbtnPowerShell";
            this.rdbtnPowerShell.Size = new System.Drawing.Size(78, 17);
            this.rdbtnPowerShell.TabIndex = 8;
            this.rdbtnPowerShell.TabStop = true;
            this.rdbtnPowerShell.Text = "PowerShell";
            this.rdbtnPowerShell.UseVisualStyleBackColor = true;
            // 
            // lblScripts
            // 
            this.lblScripts.AutoSize = true;
            this.lblScripts.Location = new System.Drawing.Point(13, 120);
            this.lblScripts.Name = "lblScripts";
            this.lblScripts.Size = new System.Drawing.Size(79, 13);
            this.lblScripts.TabIndex = 3;
            this.lblScripts.Text = "Generate script";
            // 
            // MetaDataGenerateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(324, 248);
            this.Controls.Add(this.rdbtnPowerShell);
            this.Controls.Add(this.rdbtnRScript);
            this.Controls.Add(this.rdbtnPython);
            this.Controls.Add(this.rdbtnDatadictCSV);
            this.Controls.Add(this.rdbtnSchemaJSON);
            this.Controls.Add(this.rdbtnSchemaIni);
            this.Controls.Add(this.lblScripts);
            this.Controls.Add(this.lblMetadataType);
            this.Name = "MetaDataGenerateForm";
            this.Text = "Generate metadata";
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblMetadataType, 0);
            this.Controls.SetChildIndex(this.lblScripts, 0);
            this.Controls.SetChildIndex(this.rdbtnSchemaIni, 0);
            this.Controls.SetChildIndex(this.rdbtnSchemaJSON, 0);
            this.Controls.SetChildIndex(this.rdbtnDatadictCSV, 0);
            this.Controls.SetChildIndex(this.rdbtnPython, 0);
            this.Controls.SetChildIndex(this.rdbtnRScript, 0);
            this.Controls.SetChildIndex(this.rdbtnPowerShell, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMetadataType;
        private System.Windows.Forms.RadioButton rdbtnSchemaIni;
        private System.Windows.Forms.RadioButton rdbtnSchemaJSON;
        private System.Windows.Forms.RadioButton rdbtnDatadictCSV;
        private System.Windows.Forms.RadioButton rdbtnPython;
        private System.Windows.Forms.RadioButton rdbtnRScript;
        private System.Windows.Forms.RadioButton rdbtnPowerShell;
        private System.Windows.Forms.Label lblScripts;
    }
}
