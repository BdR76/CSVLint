using CSVLint;
using Kbg.NppPluginNET;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class ColumnsSelectForm : CSVLintNppPlugin.Forms.CsvEditFormBase
    {
        private Label lblDescription;
        private Panel pnlDistinctOptions;
        private CheckBox chkNewFile;
        private CheckBox chkDistinctCount;
        private TableLayoutPanel tblColSelect;
        private Panel pnlColSelect;
        private Button btnAllSelect;
        private Button btnColRemove;
        private Button btnColSelect;
        private Panel pnlColMove;
        private Button btnColMoveDown;
        private Button btnColMoveUp;
        private Button btnAllRemove;
        private GroupBox gbxAvailableColumns;
        private ListBox listAvailableColumns;
        private GroupBox gbxSelectedColumns;
        private ListBox listSelectedColumns;
        private RadioButton rdbSortDesc;
        private RadioButton rdbSortAsc;
        private CsvDefinition _csvdef;

        public ColumnsSelectForm()
        {
            InitializeComponent();
        }
        public string SelectedColumns { get; set; }
        public bool SelectDistinct { get; set; }

        public void InitialiseSetting(CsvDefinition csvdef)
        {
            // save csvdefinition for refresh columns
            this._csvdef = csvdef;

            // get selected columns from previous session
            var selcols = Main.Settings.SelectCols.Split('|').ToList();

            // temporary list of all available columns names (in case the settings incorrectly contains duplicates)
            var allcols = csvdef.Fields.Select(f => f.Name).ToList();

            // empty list (create form so always empty)
            //listSelectedColumns.Items.Clear();

            // add selected items in listBox in same order as they appear in the settings, except when doesn't exists in csvdef.Fields[].Name
            foreach (string colname in selcols)
            {
                var idx = allcols.IndexOf(colname);
                if (idx >= 0)
                {
                    listSelectedColumns.Items.Add(colname);
                    allcols.RemoveAt(idx);
                }
            }

            // add all column names to list, except when selected
            RefreshAvailableColumns();

            // load user preferences
            chkNewFile.Checked = Main.Settings.SelectColsNewfile;
            chkDistinctCount.Checked = Main.Settings.SelectColsDistinct;
            rdbSortAsc.Checked = (Main.Settings.SelectColsSort == 1);  // 1 = ascending
            rdbSortDesc.Checked = (Main.Settings.SelectColsSort == 2); // 2 = descending

            OnChkbx_CheckedChanged(chkDistinctCount, null);
            EvaluateOkButton();
        }

        private void RefreshAvailableColumns()
        {
            // save currently selected item
            var itemselected = listAvailableColumns.SelectedItem;

            // temporary list of selected items (in case duplicates)
            var listcols = listSelectedColumns.Items.Cast<string>().ToList();

            // add all column names to list, except when selected
            listAvailableColumns.Items.Clear();
            for (var i = 0; i < this._csvdef.Fields.Count; i++)
            {
                var idx = listcols.IndexOf(this._csvdef.Fields[i].Name);
                if (idx < 0)
                {
                    listAvailableColumns.Items.Add(this._csvdef.Fields[i].Name);
                } else {
                    listcols.RemoveAt(idx);
                }
            }

            // restore previously selected item
            if (itemselected != null)
            {
                // if previously selected item is still in the list, select it
                if (listAvailableColumns.Items.Contains(itemselected))
                {
                    listAvailableColumns.SelectedItem = itemselected;
                }
            }

        }

        private void EvaluateOkButton()
        {
            // can not press OK when nothing selected
            btnOk.Enabled = (listSelectedColumns.Items.Count > 0);

            // enable disable add remove buttons
            btnColSelect.Enabled = (listAvailableColumns.SelectedItem != null && listAvailableColumns.Items.Count > 0);
            btnColRemove.Enabled = (listSelectedColumns.SelectedItem != null && listSelectedColumns.Items.Count > 0);

            btnAllSelect.Enabled = (listAvailableColumns.Items.Count > 0);
            btnAllRemove.Enabled = (listSelectedColumns.Items.Count > 0);

            // enable move up 
            btnColMoveUp.Enabled = (listSelectedColumns.SelectedItem != null && listSelectedColumns.SelectedIndex > 0);
            btnColMoveDown.Enabled = (listSelectedColumns.SelectedItem != null && listSelectedColumns.SelectedIndex < listSelectedColumns.Items.Count - 1);
        }

        private void ColumnsSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // pass split parameters to previous form
            SelectedColumns = string.Join("|", listSelectedColumns.Items.Cast<string>());
            SelectDistinct = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // save user preferences
            Main.Settings.SelectCols = string.Join("|", listSelectedColumns.Items.Cast<string>());
            Main.Settings.SelectColsNewfile = chkNewFile.Checked;
            Main.Settings.SelectColsDistinct = chkDistinctCount.Checked;
            Main.Settings.SelectColsSort = (rdbSortAsc.Checked ? 1 : (rdbSortDesc.Checked ? 2 : 0)); // 0 = no sort

            // save to file
            Main.Settings.SaveToIniFile();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColumnsSelectForm));
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlDistinctOptions = new System.Windows.Forms.Panel();
            this.rdbSortDesc = new System.Windows.Forms.RadioButton();
            this.rdbSortAsc = new System.Windows.Forms.RadioButton();
            this.chkDistinctCount = new System.Windows.Forms.CheckBox();
            this.chkNewFile = new System.Windows.Forms.CheckBox();
            this.tblColSelect = new System.Windows.Forms.TableLayoutPanel();
            this.gbxAvailableColumns = new System.Windows.Forms.GroupBox();
            this.listAvailableColumns = new System.Windows.Forms.ListBox();
            this.pnlColSelect = new System.Windows.Forms.Panel();
            this.btnAllRemove = new System.Windows.Forms.Button();
            this.btnAllSelect = new System.Windows.Forms.Button();
            this.btnColRemove = new System.Windows.Forms.Button();
            this.btnColSelect = new System.Windows.Forms.Button();
            this.pnlColMove = new System.Windows.Forms.Panel();
            this.btnColMoveDown = new System.Windows.Forms.Button();
            this.btnColMoveUp = new System.Windows.Forms.Button();
            this.gbxSelectedColumns = new System.Windows.Forms.GroupBox();
            this.listSelectedColumns = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            this.pnlDistinctOptions.SuspendLayout();
            this.tblColSelect.SuspendLayout();
            this.gbxAvailableColumns.SuspendLayout();
            this.pnlColSelect.SuspendLayout();
            this.pnlColMove.SuspendLayout();
            this.gbxSelectedColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(453, 435);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(656, 8);
            this.picHelpIcon.Tag = "select-columns";
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(117, 20);
            this.lblTitle.Text = "Select columns";
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(676, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(572, 435);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(12, 48);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(668, 33);
            this.lblDescription.TabIndex = 12;
            this.lblDescription.Text = resources.GetString("lblDescription.Text");
            // 
            // pnlDistinctOptions
            // 
            this.pnlDistinctOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlDistinctOptions.Controls.Add(this.rdbSortDesc);
            this.pnlDistinctOptions.Controls.Add(this.rdbSortAsc);
            this.pnlDistinctOptions.Controls.Add(this.chkDistinctCount);
            this.pnlDistinctOptions.Controls.Add(this.chkNewFile);
            this.pnlDistinctOptions.Location = new System.Drawing.Point(4, 420);
            this.pnlDistinctOptions.Name = "pnlDistinctOptions";
            this.pnlDistinctOptions.Size = new System.Drawing.Size(365, 45);
            this.pnlDistinctOptions.TabIndex = 13;
            // 
            // rdbSortDesc
            // 
            this.rdbSortDesc.AutoSize = true;
            this.rdbSortDesc.Location = new System.Drawing.Point(267, 24);
            this.rdbSortDesc.Name = "rdbSortDesc";
            this.rdbSortDesc.Size = new System.Drawing.Size(82, 17);
            this.rdbSortDesc.TabIndex = 17;
            this.rdbSortDesc.TabStop = true;
            this.rdbSortDesc.Tag = "1";
            this.rdbSortDesc.Text = "Descending";
            this.rdbSortDesc.UseVisualStyleBackColor = true;
            this.rdbSortDesc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // rdbSortAsc
            // 
            this.rdbSortAsc.AutoSize = true;
            this.rdbSortAsc.Location = new System.Drawing.Point(189, 24);
            this.rdbSortAsc.Name = "rdbSortAsc";
            this.rdbSortAsc.Size = new System.Drawing.Size(75, 17);
            this.rdbSortAsc.TabIndex = 16;
            this.rdbSortAsc.TabStop = true;
            this.rdbSortAsc.Tag = "1";
            this.rdbSortAsc.Text = "Ascending";
            this.rdbSortAsc.UseVisualStyleBackColor = true;
            this.rdbSortAsc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdbtns_MouseDown);
            // 
            // chkDistinctCount
            // 
            this.chkDistinctCount.AutoSize = true;
            this.chkDistinctCount.Location = new System.Drawing.Point(12, 25);
            this.chkDistinctCount.Name = "chkDistinctCount";
            this.chkDistinctCount.Size = new System.Drawing.Size(167, 17);
            this.chkDistinctCount.TabIndex = 15;
            this.chkDistinctCount.Tag = "1";
            this.chkDistinctCount.Text = "Select distinct values and sort";
            this.chkDistinctCount.UseVisualStyleBackColor = true;
            this.chkDistinctCount.CheckedChanged += new System.EventHandler(this.OnChkbx_CheckedChanged);
            // 
            // chkNewFile
            // 
            this.chkNewFile.AutoSize = true;
            this.chkNewFile.Location = new System.Drawing.Point(12, 3);
            this.chkNewFile.Name = "chkNewFile";
            this.chkNewFile.Size = new System.Drawing.Size(108, 17);
            this.chkNewFile.TabIndex = 14;
            this.chkNewFile.Text = "Result in new tab";
            this.chkNewFile.UseVisualStyleBackColor = true;
            // 
            // tblColSelect
            // 
            this.tblColSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblColSelect.ColumnCount = 4;
            this.tblColSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblColSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tblColSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblColSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tblColSelect.Controls.Add(this.gbxAvailableColumns, 0, 0);
            this.tblColSelect.Controls.Add(this.pnlColSelect, 1, 0);
            this.tblColSelect.Controls.Add(this.pnlColMove, 3, 0);
            this.tblColSelect.Controls.Add(this.gbxSelectedColumns, 2, 0);
            this.tblColSelect.Location = new System.Drawing.Point(4, 84);
            this.tblColSelect.Name = "tblColSelect";
            this.tblColSelect.RowCount = 1;
            this.tblColSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblColSelect.Size = new System.Drawing.Size(676, 332);
            this.tblColSelect.TabIndex = 3;
            // 
            // gbxAvailableColumns
            // 
            this.gbxAvailableColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxAvailableColumns.Controls.Add(this.listAvailableColumns);
            this.gbxAvailableColumns.Location = new System.Drawing.Point(3, 3);
            this.gbxAvailableColumns.Name = "gbxAvailableColumns";
            this.gbxAvailableColumns.Size = new System.Drawing.Size(221, 326);
            this.gbxAvailableColumns.TabIndex = 3;
            this.gbxAvailableColumns.TabStop = false;
            this.gbxAvailableColumns.Text = "Available columns";
            // 
            // listAvailableColumns
            // 
            this.listAvailableColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listAvailableColumns.FormattingEnabled = true;
            this.listAvailableColumns.Location = new System.Drawing.Point(4, 18);
            this.listAvailableColumns.Name = "listAvailableColumns";
            this.listAvailableColumns.Size = new System.Drawing.Size(213, 303);
            this.listAvailableColumns.TabIndex = 3;
            this.listAvailableColumns.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
            this.listAvailableColumns.DoubleClick += new System.EventHandler(this.listAvailableColumns_DoubleClick);
            this.listAvailableColumns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listAvailableColumns_KeyDown);
            // 
            // pnlColSelect
            // 
            this.pnlColSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlColSelect.Controls.Add(this.btnAllRemove);
            this.pnlColSelect.Controls.Add(this.btnAllSelect);
            this.pnlColSelect.Controls.Add(this.btnColRemove);
            this.pnlColSelect.Controls.Add(this.btnColSelect);
            this.pnlColSelect.Location = new System.Drawing.Point(230, 3);
            this.pnlColSelect.Name = "pnlColSelect";
            this.pnlColSelect.Size = new System.Drawing.Size(104, 326);
            this.pnlColSelect.TabIndex = 4;
            // 
            // btnAllRemove
            // 
            this.btnAllRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllRemove.Location = new System.Drawing.Point(0, 242);
            this.btnAllRemove.Name = "btnAllRemove";
            this.btnAllRemove.Size = new System.Drawing.Size(100, 30);
            this.btnAllRemove.TabIndex = 8;
            this.btnAllRemove.Text = "<< All";
            this.btnAllRemove.UseVisualStyleBackColor = true;
            this.btnAllRemove.Click += new System.EventHandler(this.btnAllRemove_Click);
            // 
            // btnAllSelect
            // 
            this.btnAllSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllSelect.Location = new System.Drawing.Point(0, 206);
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Size = new System.Drawing.Size(100, 30);
            this.btnAllSelect.TabIndex = 7;
            this.btnAllSelect.Text = "All >>";
            this.btnAllSelect.UseVisualStyleBackColor = true;
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // btnColRemove
            // 
            this.btnColRemove.Location = new System.Drawing.Point(0, 84);
            this.btnColRemove.Name = "btnColRemove";
            this.btnColRemove.Size = new System.Drawing.Size(100, 30);
            this.btnColRemove.TabIndex = 6;
            this.btnColRemove.Text = "<<";
            this.btnColRemove.UseVisualStyleBackColor = true;
            this.btnColRemove.Click += new System.EventHandler(this.btnColRemove_Click);
            // 
            // btnColSelect
            // 
            this.btnColSelect.Location = new System.Drawing.Point(0, 48);
            this.btnColSelect.Name = "btnColSelect";
            this.btnColSelect.Size = new System.Drawing.Size(100, 30);
            this.btnColSelect.TabIndex = 5;
            this.btnColSelect.Text = ">>";
            this.btnColSelect.UseVisualStyleBackColor = true;
            this.btnColSelect.Click += new System.EventHandler(this.btnColSelect_Click);
            // 
            // pnlColMove
            // 
            this.pnlColMove.Controls.Add(this.btnColMoveDown);
            this.pnlColMove.Controls.Add(this.btnColMoveUp);
            this.pnlColMove.Location = new System.Drawing.Point(567, 3);
            this.pnlColMove.Name = "pnlColMove";
            this.pnlColMove.Size = new System.Drawing.Size(103, 310);
            this.pnlColMove.TabIndex = 10;
            // 
            // btnColMoveDown
            // 
            this.btnColMoveDown.Location = new System.Drawing.Point(0, 84);
            this.btnColMoveDown.Name = "btnColMoveDown";
            this.btnColMoveDown.Size = new System.Drawing.Size(100, 30);
            this.btnColMoveDown.TabIndex = 12;
            this.btnColMoveDown.Text = "Move down";
            this.btnColMoveDown.UseVisualStyleBackColor = true;
            this.btnColMoveDown.Click += new System.EventHandler(this.btnColMoveDown_Click);
            // 
            // btnColMoveUp
            // 
            this.btnColMoveUp.Location = new System.Drawing.Point(0, 48);
            this.btnColMoveUp.Name = "btnColMoveUp";
            this.btnColMoveUp.Size = new System.Drawing.Size(100, 30);
            this.btnColMoveUp.TabIndex = 11;
            this.btnColMoveUp.Text = "Move up";
            this.btnColMoveUp.UseVisualStyleBackColor = true;
            this.btnColMoveUp.Click += new System.EventHandler(this.btnColMoveUp_Click);
            // 
            // gbxSelectedColumns
            // 
            this.gbxSelectedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxSelectedColumns.Controls.Add(this.listSelectedColumns);
            this.gbxSelectedColumns.Location = new System.Drawing.Point(340, 3);
            this.gbxSelectedColumns.Name = "gbxSelectedColumns";
            this.gbxSelectedColumns.Size = new System.Drawing.Size(221, 326);
            this.gbxSelectedColumns.TabIndex = 9;
            this.gbxSelectedColumns.TabStop = false;
            this.gbxSelectedColumns.Text = "Selected columns";
            // 
            // listSelectedColumns
            // 
            this.listSelectedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSelectedColumns.FormattingEnabled = true;
            this.listSelectedColumns.Location = new System.Drawing.Point(4, 18);
            this.listSelectedColumns.Name = "listSelectedColumns";
            this.listSelectedColumns.Size = new System.Drawing.Size(213, 303);
            this.listSelectedColumns.TabIndex = 9;
            this.listSelectedColumns.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
            this.listSelectedColumns.DoubleClick += new System.EventHandler(this.listSelectedColumns_DoubleClick);
            this.listSelectedColumns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listSelectedColumns_KeyDown);
            // 
            // ColumnsSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(684, 477);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.pnlDistinctOptions);
            this.Controls.Add(this.tblColSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(600, 432);
            this.Name = "ColumnsSelectForm";
            this.Text = "Select columns";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColumnsSelectForm_FormClosing);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            this.Controls.SetChildIndex(this.tblColSelect, 0);
            this.Controls.SetChildIndex(this.pnlDistinctOptions, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            this.pnlDistinctOptions.ResumeLayout(false);
            this.pnlDistinctOptions.PerformLayout();
            this.tblColSelect.ResumeLayout(false);
            this.gbxAvailableColumns.ResumeLayout(false);
            this.pnlColSelect.ResumeLayout(false);
            this.pnlColMove.ResumeLayout(false);
            this.gbxSelectedColumns.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnColSelect_Click(object sender, EventArgs e)
        {
            // add selected column to list
            if (listAvailableColumns.SelectedItem != null)
            {
                // add and remove from available list
                listSelectedColumns.Items.Add(listAvailableColumns.SelectedItem);
                listAvailableColumns.Items.Remove(listAvailableColumns.SelectedItem);

                // enable buttons
                EvaluateOkButton();
            }
        }

        private void btnColRemove_Click(object sender, EventArgs e)
        {
            // remove selected column from list
            if (listSelectedColumns.SelectedItem != null)
            {
                // remove from selected list and add to available list
                listSelectedColumns.Items.Remove(listSelectedColumns.SelectedItem);
                RefreshAvailableColumns();

                // remove from selected list
                EvaluateOkButton();
            }
        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            // add all available columns from list
            if (listAvailableColumns.Items.Count > 0)
            {
                foreach (var item in listAvailableColumns.Items.Cast<object>().ToList())
                {
                    listSelectedColumns.Items.Add(item);
                }
                listAvailableColumns.Items.Clear();
                EvaluateOkButton();
            }
        }

        private void btnAllRemove_Click(object sender, EventArgs e)
        {
            // remove all selected column from list
            if (listSelectedColumns.Items.Count > 0)
            {
                listSelectedColumns.Items.Clear();
                RefreshAvailableColumns();
                EvaluateOkButton();
            }
        }

        private void listAvailableColumns_DoubleClick(object sender, EventArgs e)
        {
            // double click to add column
            btnColSelect_Click(sender, e);
        }
        private void listAvailableColumns_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnColSelect_Click(sender, e);
        }

        private void listSelectedColumns_DoubleClick(object sender, EventArgs e)
        {
            // double click to remove column
            btnColRemove_Click(sender, e);
        }

        private void listSelectedColumns_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnColRemove_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.Up)
            {
                btnColMoveUp_Click(sender, e);
                listSelectedColumns.SelectedIndex++;
            }
            else if (e.Control && e.KeyCode == Keys.Down)
            {
                btnColMoveDown_Click(sender, e);
                listSelectedColumns.SelectedIndex--;
            }
        }

        private void btnColMoveUp_Click(object sender, EventArgs e)
        {
            // move item up in the list
            if (listSelectedColumns.SelectedItem != null && listSelectedColumns.SelectedIndex > 0)
            {
                var index = listSelectedColumns.SelectedIndex;
                var item = listSelectedColumns.SelectedItem;
                listSelectedColumns.Items.RemoveAt(index);
                listSelectedColumns.Items.Insert(index - 1, item);
                listSelectedColumns.SelectedIndex = index - 1;
            }
        }

        private void btnColMoveDown_Click(object sender, EventArgs e)
        {
            // move item down in the list
            if (listSelectedColumns.SelectedItem != null && listSelectedColumns.SelectedIndex < listSelectedColumns.Items.Count - 1)
            {
                var index = listSelectedColumns.SelectedIndex;
                var item = listSelectedColumns.SelectedItem;
                listSelectedColumns.Items.RemoveAt(index);
                listSelectedColumns.Items.Insert(index + 1, item);
                listSelectedColumns.SelectedIndex = index + 1;
            }
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            EvaluateOkButton();
        }

        private void OnChkbx_CheckedChanged(object sender, EventArgs e)
        {
            // which checkbox, see index in Tag property
            bool chk = (sender as CheckBox).Checked;
            ToggleControlBasedOnControl(sender as CheckBox, chk);
        }

        private void rdbtns_MouseDown(object sender, MouseEventArgs e)
        {
            // right click to de-select (select none for no sorting)
            if (e.Button == MouseButtons.Right) (sender as RadioButton).Checked = false;
        }
    }
}
