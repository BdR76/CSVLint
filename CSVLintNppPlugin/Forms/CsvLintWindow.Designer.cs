namespace Kbg.NppPluginNET
{
    partial class CsvLintWindow
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnEnableDisable = new System.Windows.Forms.Button();
            this.chkAutoDetect = new System.Windows.Forms.CheckBox();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnReformat = new System.Windows.Forms.Button();
            this.btnDetectColumns = new System.Windows.Forms.Button();
            this.txtSchemaIni = new System.Windows.Forms.TextBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.tooltipCsvLint = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnEnableDisable);
            this.splitContainer1.Panel1.Controls.Add(this.chkAutoDetect);
            this.splitContainer1.Panel1.Controls.Add(this.btnSort);
            this.splitContainer1.Panel1.Controls.Add(this.btnSplit);
            this.splitContainer1.Panel1.Controls.Add(this.btnApply);
            this.splitContainer1.Panel1.Controls.Add(this.btnReformat);
            this.splitContainer1.Panel1.Controls.Add(this.btnDetectColumns);
            this.splitContainer1.Panel1.Controls.Add(this.txtSchemaIni);
            this.splitContainer1.Panel1MinSize = 448;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSettings);
            this.splitContainer1.Panel2.Controls.Add(this.btnValidate);
            this.splitContainer1.Panel2.Controls.Add(this.txtOutput);
            this.splitContainer1.Size = new System.Drawing.Size(1170, 352);
            this.splitContainer1.SplitterDistance = 448;
            this.splitContainer1.TabIndex = 7;
            // 
            // btnEnableDisable
            // 
            this.btnEnableDisable.Image = global::CSVLintNppPlugin.Properties.Resources.setcolor1;
            this.btnEnableDisable.Location = new System.Drawing.Point(171, 3);
            this.btnEnableDisable.Name = "btnEnableDisable";
            this.btnEnableDisable.Size = new System.Drawing.Size(32, 32);
            this.btnEnableDisable.TabIndex = 11;
            this.tooltipCsvLint.SetToolTip(this.btnEnableDisable, "Toggle syntax highlighting colors (Language -> CSVLint)");
            this.btnEnableDisable.UseVisualStyleBackColor = true;
            this.btnEnableDisable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // chkAutoDetect
            // 
            this.chkAutoDetect.AutoSize = true;
            this.chkAutoDetect.Location = new System.Drawing.Point(80, 5);
            this.chkAutoDetect.Name = "chkAutoDetect";
            this.chkAutoDetect.Size = new System.Drawing.Size(51, 30);
            this.chkAutoDetect.TabIndex = 10;
            this.chkAutoDetect.Text = "auto-\nmatic";
            this.chkAutoDetect.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAutoDetect.UseVisualStyleBackColor = true;
            this.chkAutoDetect.Click += new System.EventHandler(this.chkAutoDetect_Click);
            // 
            // btnSort
            // 
            this.btnSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSort.Location = new System.Drawing.Point(207, 3);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(72, 32);
            this.btnSort.TabIndex = 9;
            this.btnSort.Text = "Sort";
            this.tooltipCsvLint.SetToolTip(this.btnSort, "Sort all data based on a column");
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplit.Location = new System.Drawing.Point(285, 3);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(72, 32);
            this.btnSplit.TabIndex = 9;
            this.btnSplit.Text = "Add column";
            this.tooltipCsvLint.SetToolTip(this.btnSplit, "Add or split a column based on data from an existing column");
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnApply
            // 
            this.btnApply.Image = global::CSVLintNppPlugin.Properties.Resources.disksave;
            this.btnApply.Location = new System.Drawing.Point(133, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(32, 32);
            this.btnApply.TabIndex = 8;
            this.tooltipCsvLint.SetToolTip(this.btnApply, "Apply changes and save column metadata to schema.ini");
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnReformat
            // 
            this.btnReformat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReformat.Location = new System.Drawing.Point(363, 3);
            this.btnReformat.Name = "btnReformat";
            this.btnReformat.Size = new System.Drawing.Size(72, 32);
            this.btnReformat.TabIndex = 7;
            this.btnReformat.Text = "Reformat";
            this.tooltipCsvLint.SetToolTip(this.btnReformat, "Reformat the data file with various options");
            this.btnReformat.UseVisualStyleBackColor = true;
            this.btnReformat.Click += new System.EventHandler(this.OnBtnReformat_Click);
            // 
            // btnDetectColumns
            // 
            this.btnDetectColumns.Location = new System.Drawing.Point(3, 3);
            this.btnDetectColumns.Name = "btnDetectColumns";
            this.btnDetectColumns.Size = new System.Drawing.Size(72, 32);
            this.btnDetectColumns.TabIndex = 6;
            this.btnDetectColumns.Text = "Detect columns";
            this.tooltipCsvLint.SetToolTip(this.btnDetectColumns, "Automatically detect columns and datatypes\r\nfrom the currently selected file.");
            this.btnDetectColumns.UseVisualStyleBackColor = true;
            this.btnDetectColumns.Click += new System.EventHandler(this.OnBtnDetectColumns_Click);
            // 
            // txtSchemaIni
            // 
            this.txtSchemaIni.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSchemaIni.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSchemaIni.Location = new System.Drawing.Point(3, 39);
            this.txtSchemaIni.Multiline = true;
            this.txtSchemaIni.Name = "txtSchemaIni";
            this.txtSchemaIni.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSchemaIni.Size = new System.Drawing.Size(432, 300);
            this.txtSchemaIni.TabIndex = 5;
            this.txtSchemaIni.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSchemaIni_KeyDown);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Image = global::CSVLintNppPlugin.Properties.Resources.settings;
            this.btnSettings.Location = new System.Drawing.Point(675, 3);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(32, 32);
            this.btnSettings.TabIndex = 12;
            this.tooltipCsvLint.SetToolTip(this.btnSettings, "CSV Lint settings");
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(3, 3);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(120, 32);
            this.btnValidate.TabIndex = 7;
            this.btnValidate.Text = "Validate data";
            this.tooltipCsvLint.SetToolTip(this.btnValidate, "Validate currently selected file\r\nbased on metadata on the left.");
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.OnBtnValidate_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.BackColor = System.Drawing.SystemColors.Control;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(3, 41);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(703, 298);
            this.txtOutput.TabIndex = 4;
            this.txtOutput.DoubleClick += new System.EventHandler(this.OnTxtOutput_DoubleClick);
            // 
            // tooltipCsvLint
            // 
            this.tooltipCsvLint.ShowAlways = true;
            // 
            // CsvLintWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 353);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CsvLintWindow";
            this.Text = "CSV Lint";
            this.tooltipCsvLint.SetToolTip(this, "Sort all data on a column");
            this.VisibleChanged += new System.EventHandler(this.CsvLintWindow_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnDetectColumns;
        private System.Windows.Forms.TextBox txtSchemaIni;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.ToolTip tooltipCsvLint;
        private System.Windows.Forms.Button btnReformat;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.CheckBox chkAutoDetect;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.Button btnEnableDisable;
        private System.Windows.Forms.Button btnSettings;
    }
}