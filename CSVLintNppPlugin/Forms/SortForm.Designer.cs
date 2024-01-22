
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
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.rdbtnDescending = new System.Windows.Forms.RadioButton();
            this.rdbtnAscending = new System.Windows.Forms.RadioButton();
            this.pnlSortOn = new System.Windows.Forms.Panel();
            this.lblSortOn = new System.Windows.Forms.Label();
            this.rdbtnLength = new System.Windows.Forms.RadioButton();
            this.rdbtnValue = new System.Windows.Forms.RadioButton();
            this.cmbSelectColumn = new System.Windows.Forms.ComboBox();
            this.lblSelectColumn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.pnlSortOn.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(153, 187);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(356, 8);
            this.picHelpIcon.Tag = "sort-data";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(75, 20);
            this.lblTitle.Text = "Sort data";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(376, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 187);
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(12, 128);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(53, 13);
            this.lblSortOrder.TabIndex = 12;
            this.lblSortOrder.Text = "Sort order";
            // 
            // rdbtnDescending
            // 
            this.rdbtnDescending.AutoSize = true;
            this.rdbtnDescending.Location = new System.Drawing.Point(192, 152);
            this.rdbtnDescending.Name = "rdbtnDescending";
            this.rdbtnDescending.Size = new System.Drawing.Size(82, 17);
            this.rdbtnDescending.TabIndex = 8;
            this.rdbtnDescending.TabStop = true;
            this.rdbtnDescending.Text = "Descending";
            this.rdbtnDescending.UseVisualStyleBackColor = true;
            // 
            // rdbtnAscending
            // 
            this.rdbtnAscending.AutoSize = true;
            this.rdbtnAscending.Location = new System.Drawing.Point(192, 128);
            this.rdbtnAscending.Name = "rdbtnAscending";
            this.rdbtnAscending.Size = new System.Drawing.Size(75, 17);
            this.rdbtnAscending.TabIndex = 7;
            this.rdbtnAscending.TabStop = true;
            this.rdbtnAscending.Text = "Ascending";
            this.rdbtnAscending.UseVisualStyleBackColor = true;
            // 
            // pnlSortOn
            // 
            this.pnlSortOn.Controls.Add(this.lblSortOn);
            this.pnlSortOn.Controls.Add(this.rdbtnLength);
            this.pnlSortOn.Controls.Add(this.rdbtnValue);
            this.pnlSortOn.Location = new System.Drawing.Point(0, 80);
            this.pnlSortOn.Name = "pnlSortOn";
            this.pnlSortOn.Size = new System.Drawing.Size(368, 46);
            this.pnlSortOn.TabIndex = 4;
            // 
            // lblSortOn
            // 
            this.lblSortOn.AutoSize = true;
            this.lblSortOn.Location = new System.Drawing.Point(12, 0);
            this.lblSortOn.Name = "lblSortOn";
            this.lblSortOn.Size = new System.Drawing.Size(41, 13);
            this.lblSortOn.TabIndex = 13;
            this.lblSortOn.Text = "Sort on";
            // 
            // rdbtnLength
            // 
            this.rdbtnLength.AutoSize = true;
            this.rdbtnLength.Location = new System.Drawing.Point(192, 24);
            this.rdbtnLength.Name = "rdbtnLength";
            this.rdbtnLength.Size = new System.Drawing.Size(99, 17);
            this.rdbtnLength.TabIndex = 6;
            this.rdbtnLength.TabStop = true;
            this.rdbtnLength.Text = "Length of value";
            this.rdbtnLength.UseVisualStyleBackColor = true;
            // 
            // rdbtnValue
            // 
            this.rdbtnValue.AutoSize = true;
            this.rdbtnValue.Location = new System.Drawing.Point(192, 0);
            this.rdbtnValue.Name = "rdbtnValue";
            this.rdbtnValue.Size = new System.Drawing.Size(52, 17);
            this.rdbtnValue.TabIndex = 5;
            this.rdbtnValue.TabStop = true;
            this.rdbtnValue.Text = "Value";
            this.rdbtnValue.UseVisualStyleBackColor = true;
            // 
            // cmbSelectColumn
            // 
            this.cmbSelectColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectColumn.FormattingEnabled = true;
            this.cmbSelectColumn.Items.AddRange(new object[] {
            "(select a column)"});
            this.cmbSelectColumn.Location = new System.Drawing.Point(192, 48);
            this.cmbSelectColumn.Name = "cmbSelectColumn";
            this.cmbSelectColumn.Size = new System.Drawing.Size(180, 21);
            this.cmbSelectColumn.TabIndex = 3;
            this.cmbSelectColumn.Tag = "";
            this.cmbSelectColumn.SelectedIndexChanged += new System.EventHandler(this.cmbSelectColumn_SelectedIndexChanged);
            // 
            // lblSelectColumn
            // 
            this.lblSelectColumn.AutoSize = true;
            this.lblSelectColumn.Location = new System.Drawing.Point(12, 48);
            this.lblSelectColumn.Name = "lblSelectColumn";
            this.lblSelectColumn.Size = new System.Drawing.Size(78, 13);
            this.lblSelectColumn.TabIndex = 10;
            this.lblSelectColumn.Text = "Sort on column";
            // 
            // SortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(384, 229);
            this.Controls.Add(this.pnlSortOn);
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
            this.Controls.SetChildIndex(this.pnlSortOn, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.pnlSortOn.ResumeLayout(false);
            this.pnlSortOn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectColumn;
        private System.Windows.Forms.ComboBox cmbSelectColumn;
        private System.Windows.Forms.Panel pnlSortOn;
        private System.Windows.Forms.Label lblSortOn;
        private System.Windows.Forms.RadioButton rdbtnLength;
        private System.Windows.Forms.RadioButton rdbtnValue;
        private System.Windows.Forms.Label lblSortOrder;
        private System.Windows.Forms.RadioButton rdbtnAscending;
        private System.Windows.Forms.RadioButton rdbtnDescending;
    }
}
