namespace Kbg.NppPluginNET
{
    partial class AboutForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lnkDonate = new System.Windows.Forms.LinkLabel();
            this.lnkContact = new System.Windows.Forms.LinkLabel();
            this.picEasterEgg = new System.Windows.Forms.PictureBox();
            this.lblDisclaimer = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picEasterEgg)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(66, 185);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(76, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(8, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(192, 20);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "CSV Lint plug-in NPP v";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(8, 41);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(192, 38);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Validate, reformat and clean up dataset textfiles, by Bas de Reuver";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lnkDonate
            // 
            this.lnkDonate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkDonate.Location = new System.Drawing.Point(8, 136);
            this.lnkDonate.Name = "lnkDonate";
            this.lnkDonate.Size = new System.Drawing.Size(192, 13);
            this.lnkDonate.TabIndex = 3;
            this.lnkDonate.TabStop = true;
            this.lnkDonate.Tag = "0";
            this.lnkDonate.Text = "Donate / Buy me a coffee";
            this.lnkDonate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkDonate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // lnkContact
            // 
            this.lnkContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkContact.Location = new System.Drawing.Point(8, 160);
            this.lnkContact.Name = "lnkContact";
            this.lnkContact.Size = new System.Drawing.Size(192, 13);
            this.lnkContact.TabIndex = 4;
            this.lnkContact.TabStop = true;
            this.lnkContact.Tag = "1";
            this.lnkContact.Text = "bdr1976@gmail.com";
            this.lnkContact.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkContact.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // picEasterEgg
            // 
            this.picEasterEgg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picEasterEgg.Location = new System.Drawing.Point(170, 179);
            this.picEasterEgg.Name = "picEasterEgg";
            this.picEasterEgg.Size = new System.Drawing.Size(32, 32);
            this.picEasterEgg.TabIndex = 7;
            this.picEasterEgg.TabStop = false;
            this.picEasterEgg.Click += new System.EventHandler(this.picEasterEgg_Click);
            // 
            // lblDisclaimer
            // 
            this.lblDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisclaimer.Location = new System.Drawing.Point(8, 78);
            this.lblDisclaimer.Name = "lblDisclaimer";
            this.lblDisclaimer.Size = new System.Drawing.Size(192, 59);
            this.lblDisclaimer.TabIndex = 2;
            this.lblDisclaimer.Text = "This software is free-to-use and it is provided as-is without warranty of any kin" +
    "d, always back-up your data files to prevent data loss.";
            this.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblLine
            // 
            this.lblLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLine.Location = new System.Drawing.Point(4, 34);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(200, 2);
            this.lblLine.TabIndex = 1;
            this.lblLine.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 217);
            this.Controls.Add(this.picEasterEgg);
            this.Controls.Add(this.lnkContact);
            this.Controls.Add(this.lnkDonate);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblDisclaimer);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.picEasterEgg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.LinkLabel lnkDonate;
        private System.Windows.Forms.LinkLabel lnkContact;
        private System.Windows.Forms.PictureBox picEasterEgg;
        private System.Windows.Forms.Label lblDisclaimer;
        private System.Windows.Forms.Label lblLine;
    }
}