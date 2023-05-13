
namespace CSVLintNppPlugin.Forms
{
    partial class UniqueValuesForm
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
            this.listColumns = new System.Windows.Forms.ListBox();
            this.ctxmnuColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuitmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuitmSelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuitmSelectInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.lblDescription = new System.Windows.Forms.Label();
            this.panelSortWhat = new System.Windows.Forms.Panel();
            this.chkSortBy = new System.Windows.Forms.CheckBox();
            this.radioSortCount = new System.Windows.Forms.RadioButton();
            this.radioSortValue = new System.Windows.Forms.RadioButton();
            this.panelSortHow = new System.Windows.Forms.Panel();
            this.labelSortHow = new System.Windows.Forms.Label();
            this.radioSortDesc = new System.Windows.Forms.RadioButton();
            this.radioSortAsc = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.ctxmnuColumns.SuspendLayout();
            this.panelSortWhat.SuspendLayout();
            this.panelSortHow.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(61, 345);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(261, 8);
            this.picHelpIcon.Tag = "count-unique-values";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(153, 20);
            this.lblTitle.Text = "Count unique values";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(285, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 345);
            // 
            // listColumns
            // 
            this.listColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listColumns.ContextMenuStrip = this.ctxmnuColumns;
            this.listColumns.FormattingEnabled = true;
            this.listColumns.Location = new System.Drawing.Point(16, 85);
            this.listColumns.Name = "listColumns";
            this.listColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listColumns.Size = new System.Drawing.Size(260, 199);
            this.listColumns.TabIndex = 3;
            this.listColumns.SelectedIndexChanged += new System.EventHandler(this.listColumns_SelectedIndexChanged);
            // 
            // ctxmnuColumns
            // 
            this.ctxmnuColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuitmSelectAll,
            this.mnuitmSelectNone,
            this.mnuitmSelectInvert});
            this.ctxmnuColumns.Name = "ctxmnuColumns";
            this.ctxmnuColumns.Size = new System.Drawing.Size(163, 70);
            // 
            // mnuitmSelectAll
            // 
            this.mnuitmSelectAll.Name = "mnuitmSelectAll";
            this.mnuitmSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuitmSelectAll.Size = new System.Drawing.Size(162, 22);
            this.mnuitmSelectAll.Text = "Select all";
            this.mnuitmSelectAll.Click += new System.EventHandler(this.mnuitmSelectAll_Click);
            // 
            // mnuitmSelectNone
            // 
            this.mnuitmSelectNone.Name = "mnuitmSelectNone";
            this.mnuitmSelectNone.Size = new System.Drawing.Size(162, 22);
            this.mnuitmSelectNone.Text = "Select none";
            this.mnuitmSelectNone.Click += new System.EventHandler(this.mnuitmSelectNone_Click);
            // 
            // mnuitmSelectInvert
            // 
            this.mnuitmSelectInvert.Name = "mnuitmSelectInvert";
            this.mnuitmSelectInvert.Size = new System.Drawing.Size(162, 22);
            this.mnuitmSelectInvert.Text = "Invert selection";
            this.mnuitmSelectInvert.Click += new System.EventHandler(this.mnuitmSelectInvert_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(12, 48);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(274, 34);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Select one or more columns and press OK to list and count all unique values, or c" +
    "ombinations of values.";
            // 
            // panelSortWhat
            // 
            this.panelSortWhat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSortWhat.Controls.Add(this.chkSortBy);
            this.panelSortWhat.Controls.Add(this.radioSortCount);
            this.panelSortWhat.Controls.Add(this.radioSortValue);
            this.panelSortWhat.Location = new System.Drawing.Point(16, 290);
            this.panelSortWhat.Name = "panelSortWhat";
            this.panelSortWhat.Size = new System.Drawing.Size(158, 48);
            this.panelSortWhat.TabIndex = 4;
            this.panelSortWhat.Tag = "";
            // 
            // chkSortBy
            // 
            this.chkSortBy.AutoSize = true;
            this.chkSortBy.Location = new System.Drawing.Point(3, 5);
            this.chkSortBy.Name = "chkSortBy";
            this.chkSortBy.Size = new System.Drawing.Size(59, 17);
            this.chkSortBy.TabIndex = 5;
            this.chkSortBy.Tag = "1";
            this.chkSortBy.Text = "Sort by";
            this.chkSortBy.UseVisualStyleBackColor = true;
            this.chkSortBy.CheckedChanged += new System.EventHandler(this.chkSortBy_CheckedChanged);
            // 
            // radioSortCount
            // 
            this.radioSortCount.AutoSize = true;
            this.radioSortCount.Location = new System.Drawing.Point(66, 26);
            this.radioSortCount.Name = "radioSortCount";
            this.radioSortCount.Size = new System.Drawing.Size(52, 17);
            this.radioSortCount.TabIndex = 7;
            this.radioSortCount.TabStop = true;
            this.radioSortCount.Tag = "1";
            this.radioSortCount.Text = "count";
            this.radioSortCount.UseVisualStyleBackColor = true;
            // 
            // radioSortValue
            // 
            this.radioSortValue.AutoSize = true;
            this.radioSortValue.Location = new System.Drawing.Point(66, 3);
            this.radioSortValue.Name = "radioSortValue";
            this.radioSortValue.Size = new System.Drawing.Size(56, 17);
            this.radioSortValue.TabIndex = 6;
            this.radioSortValue.TabStop = true;
            this.radioSortValue.Tag = "1";
            this.radioSortValue.Text = "values";
            this.radioSortValue.UseVisualStyleBackColor = true;
            // 
            // panelSortHow
            // 
            this.panelSortHow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSortHow.Controls.Add(this.labelSortHow);
            this.panelSortHow.Controls.Add(this.radioSortDesc);
            this.panelSortHow.Controls.Add(this.radioSortAsc);
            this.panelSortHow.Location = new System.Drawing.Point(144, 290);
            this.panelSortHow.Name = "panelSortHow";
            this.panelSortHow.Size = new System.Drawing.Size(132, 48);
            this.panelSortHow.TabIndex = 8;
            this.panelSortHow.Tag = "";
            // 
            // labelSortHow
            // 
            this.labelSortHow.AutoSize = true;
            this.labelSortHow.Location = new System.Drawing.Point(3, 5);
            this.labelSortHow.Name = "labelSortHow";
            this.labelSortHow.Size = new System.Drawing.Size(45, 13);
            this.labelSortHow.TabIndex = 10;
            this.labelSortHow.Tag = "1";
            this.labelSortHow.Text = "and sort";
            // 
            // radioSortDesc
            // 
            this.radioSortDesc.AutoSize = true;
            this.radioSortDesc.Location = new System.Drawing.Point(49, 26);
            this.radioSortDesc.Name = "radioSortDesc";
            this.radioSortDesc.Size = new System.Drawing.Size(80, 17);
            this.radioSortDesc.TabIndex = 10;
            this.radioSortDesc.TabStop = true;
            this.radioSortDesc.Tag = "1";
            this.radioSortDesc.Text = "descending";
            this.radioSortDesc.UseVisualStyleBackColor = true;
            // 
            // radioSortAsc
            // 
            this.radioSortAsc.AutoSize = true;
            this.radioSortAsc.Location = new System.Drawing.Point(49, 3);
            this.radioSortAsc.Name = "radioSortAsc";
            this.radioSortAsc.Size = new System.Drawing.Size(74, 17);
            this.radioSortAsc.TabIndex = 9;
            this.radioSortAsc.TabStop = true;
            this.radioSortAsc.Tag = "1";
            this.radioSortAsc.Text = "ascending";
            this.radioSortAsc.UseVisualStyleBackColor = true;
            // 
            // UniqueValuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(292, 387);
            this.Controls.Add(this.panelSortHow);
            this.Controls.Add(this.panelSortWhat);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.listColumns);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "UniqueValuesForm";
            this.Text = "Count unique values";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UniqueValuesForm_FormClosing);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.listColumns, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.panelSortWhat, 0);
            this.Controls.SetChildIndex(this.panelSortHow, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ctxmnuColumns.ResumeLayout(false);
            this.panelSortWhat.ResumeLayout(false);
            this.panelSortWhat.PerformLayout();
            this.panelSortHow.ResumeLayout(false);
            this.panelSortHow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listColumns;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel panelSortWhat;
        private System.Windows.Forms.RadioButton radioSortCount;
        private System.Windows.Forms.RadioButton radioSortValue;
        private System.Windows.Forms.Panel panelSortHow;
        private System.Windows.Forms.Label labelSortHow;
        private System.Windows.Forms.RadioButton radioSortDesc;
        private System.Windows.Forms.RadioButton radioSortAsc;
        private System.Windows.Forms.ContextMenuStrip ctxmnuColumns;
        private System.Windows.Forms.ToolStripMenuItem mnuitmSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuitmSelectNone;
        private System.Windows.Forms.ToolStripMenuItem mnuitmSelectInvert;
        private System.Windows.Forms.CheckBox chkSortBy;
    }
}
