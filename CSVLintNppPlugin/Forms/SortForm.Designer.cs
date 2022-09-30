
namespace CSVLintNppPlugin.Forms
{
    partial class SortForm
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
            this.cmbSelectColumn = new System.Windows.Forms.ComboBox();
            this.lblSelectColumn = new System.Windows.Forms.Label();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.rdbtnAscending = new System.Windows.Forms.RadioButton();
            this.rdbtnDescending = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(153, 151);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 151);
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(75, 20);
            this.lblTitle.Text = "Sort data";
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(356, 8);
            this.picHelpIcon.Tag = "sort-data";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(376, 2);
            // 
            // lblSelectColumn
            // 
            this.lblSelectColumn.AutoSize = true;
            this.lblSelectColumn.Location = new System.Drawing.Point(12, 58);
            this.lblSelectColumn.Name = "lblSelectColumn";
            this.lblSelectColumn.Size = new System.Drawing.Size(125, 13);
            this.lblSelectColumn.TabIndex = 10;
            this.lblSelectColumn.Text = "Based on original column";
            // 
            // cmbSelectColumn
            // 
            this.cmbSelectColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectColumn.FormattingEnabled = true;
            this.cmbSelectColumn.Items.AddRange(new object[] {
            "(select a column)"});
            this.cmbSelectColumn.Location = new System.Drawing.Point(192, 54);
            this.cmbSelectColumn.Name = "cmbSelectColumn";
            this.cmbSelectColumn.Size = new System.Drawing.Size(180, 21);
            this.cmbSelectColumn.TabIndex = 3;
            this.cmbSelectColumn.Tag = "";
            this.cmbSelectColumn.SelectedIndexChanged += new System.EventHandler(this.cmbSelectColumn_SelectedIndexChanged);
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(13, 95);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(127, 13);
            this.lblSortOrder.TabIndex = 12;
            this.lblSortOrder.Text = "Ascending or descending";
            // 
            // rdbtnAscending
            // 
            this.rdbtnAscending.AutoSize = true;
            this.rdbtnAscending.Location = new System.Drawing.Point(192, 93);
            this.rdbtnAscending.Name = "rdbtnAscending";
            this.rdbtnAscending.Size = new System.Drawing.Size(75, 17);
            this.rdbtnAscending.TabIndex = 4;
            this.rdbtnAscending.TabStop = true;
            this.rdbtnAscending.Text = "Ascending";
            this.rdbtnAscending.UseVisualStyleBackColor = true;
            // 
            // rdbtnDescending
            // 
            this.rdbtnDescending.AutoSize = true;
            this.rdbtnDescending.Location = new System.Drawing.Point(192, 116);
            this.rdbtnDescending.Name = "rdbtnDescending";
            this.rdbtnDescending.Size = new System.Drawing.Size(82, 17);
            this.rdbtnDescending.TabIndex = 5;
            this.rdbtnDescending.TabStop = true;
            this.rdbtnDescending.Text = "Descending";
            this.rdbtnDescending.UseVisualStyleBackColor = true;
            // 
            // SortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(384, 193);
            this.Controls.Add(this.rdbtnDescending);
            this.Controls.Add(this.rdbtnAscending);
            this.Controls.Add(this.lblSortOrder);
            this.Controls.Add(this.cmbSelectColumn);
            this.Controls.Add(this.lblSelectColumn);
            this.Name = "SortForm";
            this.Text = "Sort data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SortForm_FormClosing);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.lblSelectColumn, 0);
            this.Controls.SetChildIndex(this.cmbSelectColumn, 0);
            this.Controls.SetChildIndex(this.lblSortOrder, 0);
            this.Controls.SetChildIndex(this.rdbtnAscending, 0);
            this.Controls.SetChildIndex(this.rdbtnDescending, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectColumn;
        private System.Windows.Forms.ComboBox cmbSelectColumn;
        private System.Windows.Forms.Label lblSortOrder;
        private System.Windows.Forms.RadioButton rdbtnAscending;
        private System.Windows.Forms.RadioButton rdbtnDescending;
    }
}
