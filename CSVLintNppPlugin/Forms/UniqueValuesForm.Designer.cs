
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.panelSortWhat = new System.Windows.Forms.Panel();
            this.lblSortWhat = new System.Windows.Forms.Label();
            this.radioSortCount = new System.Windows.Forms.RadioButton();
            this.radioSortValue = new System.Windows.Forms.RadioButton();
            this.panelSortHow = new System.Windows.Forms.Panel();
            this.labelSortHow = new System.Windows.Forms.Label();
            this.radioSortDesc = new System.Windows.Forms.RadioButton();
            this.radioSortAsc = new System.Windows.Forms.RadioButton();
            this.ctxmnuColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuitmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuitmSelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuitmSelectInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSortWhat.SuspendLayout();
            this.panelSortHow.SuspendLayout();
            this.ctxmnuColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(153, 20);
            this.lblTitle.Text = "Count unique values";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(61, 354);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 354);
            // 
            // listColumns
            // 
            this.listColumns.ContextMenuStrip = this.ctxmnuColumns;
            this.listColumns.FormattingEnabled = true;
            this.listColumns.Location = new System.Drawing.Point(16, 77);
            this.listColumns.Name = "listColumns";
            this.listColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listColumns.Size = new System.Drawing.Size(260, 199);
            this.listColumns.TabIndex = 3;
            this.listColumns.SelectedIndexChanged += new System.EventHandler(this.listColumns_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(12, 40);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(274, 34);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Select one or more columns and press OK to list and count all unique values, or c" +
    "ombinations of values.";
            // 
            // panelSortWhat
            // 
            this.panelSortWhat.Controls.Add(this.lblSortWhat);
            this.panelSortWhat.Controls.Add(this.radioSortCount);
            this.panelSortWhat.Controls.Add(this.radioSortValue);
            this.panelSortWhat.Location = new System.Drawing.Point(16, 282);
            this.panelSortWhat.Name = "panelSortWhat";
            this.panelSortWhat.Size = new System.Drawing.Size(109, 66);
            this.panelSortWhat.TabIndex = 6;
            // 
            // lblSortWhat
            // 
            this.lblSortWhat.AutoSize = true;
            this.lblSortWhat.Location = new System.Drawing.Point(3, 5);
            this.lblSortWhat.Name = "lblSortWhat";
            this.lblSortWhat.Size = new System.Drawing.Size(40, 13);
            this.lblSortWhat.TabIndex = 10;
            this.lblSortWhat.Text = "Sort by";
            // 
            // radioSortCount
            // 
            this.radioSortCount.AutoSize = true;
            this.radioSortCount.Location = new System.Drawing.Point(48, 26);
            this.radioSortCount.Name = "radioSortCount";
            this.radioSortCount.Size = new System.Drawing.Size(52, 17);
            this.radioSortCount.TabIndex = 9;
            this.radioSortCount.TabStop = true;
            this.radioSortCount.Text = "count";
            this.radioSortCount.UseVisualStyleBackColor = true;
            // 
            // radioSortValue
            // 
            this.radioSortValue.AutoSize = true;
            this.radioSortValue.Location = new System.Drawing.Point(48, 3);
            this.radioSortValue.Name = "radioSortValue";
            this.radioSortValue.Size = new System.Drawing.Size(56, 17);
            this.radioSortValue.TabIndex = 8;
            this.radioSortValue.TabStop = true;
            this.radioSortValue.Text = "values";
            this.radioSortValue.UseVisualStyleBackColor = true;
            // 
            // panelSortHow
            // 
            this.panelSortHow.Controls.Add(this.labelSortHow);
            this.panelSortHow.Controls.Add(this.radioSortDesc);
            this.panelSortHow.Controls.Add(this.radioSortAsc);
            this.panelSortHow.Location = new System.Drawing.Point(131, 282);
            this.panelSortHow.Name = "panelSortHow";
            this.panelSortHow.Size = new System.Drawing.Size(145, 66);
            this.panelSortHow.TabIndex = 7;
            // 
            // labelSortHow
            // 
            this.labelSortHow.AutoSize = true;
            this.labelSortHow.Location = new System.Drawing.Point(3, 5);
            this.labelSortHow.Name = "labelSortHow";
            this.labelSortHow.Size = new System.Drawing.Size(45, 13);
            this.labelSortHow.TabIndex = 10;
            this.labelSortHow.Text = "and sort";
            // 
            // radioSortDesc
            // 
            this.radioSortDesc.AutoSize = true;
            this.radioSortDesc.Location = new System.Drawing.Point(49, 26);
            this.radioSortDesc.Name = "radioSortDesc";
            this.radioSortDesc.Size = new System.Drawing.Size(80, 17);
            this.radioSortDesc.TabIndex = 9;
            this.radioSortDesc.TabStop = true;
            this.radioSortDesc.Text = "descending";
            this.radioSortDesc.UseVisualStyleBackColor = true;
            // 
            // radioSortAsc
            // 
            this.radioSortAsc.AutoSize = true;
            this.radioSortAsc.Location = new System.Drawing.Point(49, 3);
            this.radioSortAsc.Name = "radioSortAsc";
            this.radioSortAsc.Size = new System.Drawing.Size(74, 17);
            this.radioSortAsc.TabIndex = 8;
            this.radioSortAsc.TabStop = true;
            this.radioSortAsc.Text = "ascending";
            this.radioSortAsc.UseVisualStyleBackColor = true;
            // 
            // ctxmnuColumns
            // 
            this.ctxmnuColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuitmSelectAll,
            this.mnuitmSelectNone,
            this.mnuitmSelectInvert});
            this.ctxmnuColumns.Name = "ctxmnuColumns";
            this.ctxmnuColumns.Size = new System.Drawing.Size(181, 92);
            // 
            // mnuitmSelectAll
            // 
            this.mnuitmSelectAll.Name = "mnuitmSelectAll";
            this.mnuitmSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuitmSelectAll.Size = new System.Drawing.Size(180, 22);
            this.mnuitmSelectAll.Text = "Select all";
            this.mnuitmSelectAll.Click += new System.EventHandler(this.mnuitmSelectAll_Click);
            // 
            // mnuitmSelectNone
            // 
            this.mnuitmSelectNone.Name = "mnuitmSelectNone";
            this.mnuitmSelectNone.Size = new System.Drawing.Size(180, 22);
            this.mnuitmSelectNone.Text = "Select none";
            this.mnuitmSelectNone.Click += new System.EventHandler(this.mnuitmSelectNone_Click);
            // 
            // mnuitmSelectInvert
            // 
            this.mnuitmSelectInvert.Name = "mnuitmSelectInvert";
            this.mnuitmSelectInvert.Size = new System.Drawing.Size(180, 22);
            this.mnuitmSelectInvert.Text = "Invert selection";
            this.mnuitmSelectInvert.Click += new System.EventHandler(this.mnuitmSelectInvert_Click);
            // 
            // UniqueValuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(292, 396);
            this.Controls.Add(this.panelSortHow);
            this.Controls.Add(this.panelSortWhat);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.listColumns);
            this.Name = "UniqueValuesForm";
            this.Text = "Count unique values";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UniqueValuesForm_FormClosing);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.listColumns, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.panelSortWhat, 0);
            this.Controls.SetChildIndex(this.panelSortHow, 0);
            this.panelSortWhat.ResumeLayout(false);
            this.panelSortWhat.PerformLayout();
            this.panelSortHow.ResumeLayout(false);
            this.panelSortHow.PerformLayout();
            this.ctxmnuColumns.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listColumns;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel panelSortWhat;
        private System.Windows.Forms.Label lblSortWhat;
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
    }
}
