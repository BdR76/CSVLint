
namespace CSVLintNppPlugin.Forms
{
    partial class ColumnSplitForm
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
            this.lblSelectColumn = new System.Windows.Forms.Label();
            this.cmbSelectColumn = new System.Windows.Forms.ComboBox();
            this.rdbtnSplitValid = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitCharacter = new System.Windows.Forms.RadioButton();
            this.rdbtnSplitSubstring = new System.Windows.Forms.RadioButton();
            this.chkDeleteOriginal = new System.Windows.Forms.CheckBox();
            this.txtSplitCharacter = new System.Windows.Forms.TextBox();
            this.txtSplitSubstring = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(95, 20);
            this.lblTitle.Text = "Split column";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(152, 231);
            this.btnOk.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(271, 231);
            this.btnCancel.TabIndex = 9;
            // 
            // lblSelectColumn
            // 
            this.lblSelectColumn.AutoSize = true;
            this.lblSelectColumn.Location = new System.Drawing.Point(12, 52);
            this.lblSelectColumn.Name = "lblSelectColumn";
            this.lblSelectColumn.Size = new System.Drawing.Size(107, 13);
            this.lblSelectColumn.TabIndex = 0;
            this.lblSelectColumn.Text = "Select column to split";
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
            this.cmbSelectColumn.TabIndex = 1;
            this.cmbSelectColumn.Tag = "1";
            this.cmbSelectColumn.SelectedIndexChanged += new System.EventHandler(this.cmbSelectColumn_SelectedIndexChanged);
            // 
            // rdbtnSplitValid
            // 
            this.rdbtnSplitValid.AutoSize = true;
            this.rdbtnSplitValid.Checked = true;
            this.rdbtnSplitValid.Location = new System.Drawing.Point(16, 84);
            this.rdbtnSplitValid.Name = "rdbtnSplitValid";
            this.rdbtnSplitValid.Size = new System.Drawing.Size(158, 17);
            this.rdbtnSplitValid.TabIndex = 2;
            this.rdbtnSplitValid.TabStop = true;
            this.rdbtnSplitValid.Tag = "1";
            this.rdbtnSplitValid.Text = "Split valid and invalid values";
            this.rdbtnSplitValid.UseVisualStyleBackColor = true;
            // 
            // rdbtnSplitCharacter
            // 
            this.rdbtnSplitCharacter.AutoSize = true;
            this.rdbtnSplitCharacter.Location = new System.Drawing.Point(16, 116);
            this.rdbtnSplitCharacter.Name = "rdbtnSplitCharacter";
            this.rdbtnSplitCharacter.Size = new System.Drawing.Size(108, 17);
            this.rdbtnSplitCharacter.TabIndex = 3;
            this.rdbtnSplitCharacter.Tag = "2";
            this.rdbtnSplitCharacter.Text = "Split on character";
            this.rdbtnSplitCharacter.UseVisualStyleBackColor = true;
            this.rdbtnSplitCharacter.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // rdbtnSplitSubstring
            // 
            this.rdbtnSplitSubstring.AutoSize = true;
            this.rdbtnSplitSubstring.Location = new System.Drawing.Point(16, 148);
            this.rdbtnSplitSubstring.Name = "rdbtnSplitSubstring";
            this.rdbtnSplitSubstring.Size = new System.Drawing.Size(99, 17);
            this.rdbtnSplitSubstring.TabIndex = 5;
            this.rdbtnSplitSubstring.Tag = "3";
            this.rdbtnSplitSubstring.Text = "Split on position";
            this.rdbtnSplitSubstring.UseVisualStyleBackColor = true;
            this.rdbtnSplitSubstring.CheckedChanged += new System.EventHandler(this.OnRadioBtn_CheckedChanged);
            // 
            // chkDeleteOriginal
            // 
            this.chkDeleteOriginal.AutoSize = true;
            this.chkDeleteOriginal.Location = new System.Drawing.Point(16, 196);
            this.chkDeleteOriginal.Name = "chkDeleteOriginal";
            this.chkDeleteOriginal.Size = new System.Drawing.Size(139, 17);
            this.chkDeleteOriginal.TabIndex = 7;
            this.chkDeleteOriginal.Tag = "4";
            this.chkDeleteOriginal.Text = "Remove original column";
            this.chkDeleteOriginal.UseVisualStyleBackColor = true;
            // 
            // txtSplitCharacter
            // 
            this.txtSplitCharacter.Enabled = false;
            this.txtSplitCharacter.Location = new System.Drawing.Point(191, 112);
            this.txtSplitCharacter.Name = "txtSplitCharacter";
            this.txtSplitCharacter.Size = new System.Drawing.Size(48, 20);
            this.txtSplitCharacter.TabIndex = 4;
            this.txtSplitCharacter.Tag = "2";
            this.txtSplitCharacter.Text = "/";
            // 
            // txtSplitSubstring
            // 
            this.txtSplitSubstring.Enabled = false;
            this.txtSplitSubstring.Location = new System.Drawing.Point(191, 144);
            this.txtSplitSubstring.Name = "txtSplitSubstring";
            this.txtSplitSubstring.Size = new System.Drawing.Size(48, 20);
            this.txtSplitSubstring.TabIndex = 6;
            this.txtSplitSubstring.Tag = "3";
            this.txtSplitSubstring.Text = "4";
            // 
            // ColumnSplitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(383, 273);
            this.Controls.Add(this.txtSplitSubstring);
            this.Controls.Add(this.txtSplitCharacter);
            this.Controls.Add(this.chkDeleteOriginal);
            this.Controls.Add(this.rdbtnSplitSubstring);
            this.Controls.Add(this.rdbtnSplitCharacter);
            this.Controls.Add(this.rdbtnSplitValid);
            this.Controls.Add(this.cmbSelectColumn);
            this.Controls.Add(this.lblSelectColumn);
            this.Name = "ColumnSplitForm";
            this.Text = "Split column";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColumnSplitForm_FormClosing);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblSelectColumn, 0);
            this.Controls.SetChildIndex(this.cmbSelectColumn, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitValid, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitCharacter, 0);
            this.Controls.SetChildIndex(this.rdbtnSplitSubstring, 0);
            this.Controls.SetChildIndex(this.chkDeleteOriginal, 0);
            this.Controls.SetChildIndex(this.txtSplitCharacter, 0);
            this.Controls.SetChildIndex(this.txtSplitSubstring, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectColumn;
        private System.Windows.Forms.ComboBox cmbSelectColumn;
        private System.Windows.Forms.RadioButton rdbtnSplitValid;
        private System.Windows.Forms.RadioButton rdbtnSplitCharacter;
        private System.Windows.Forms.RadioButton rdbtnSplitSubstring;
        private System.Windows.Forms.CheckBox chkDeleteOriginal;
        private System.Windows.Forms.TextBox txtSplitCharacter;
        private System.Windows.Forms.TextBox txtSplitSubstring;
    }
}
