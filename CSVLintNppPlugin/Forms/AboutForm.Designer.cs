
namespace CSVLintNppPlugin.Forms
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
            this.lnkInfo = new System.Windows.Forms.LinkLabel();
            this.lnkGithub = new System.Windows.Forms.LinkLabel();
            this.picEasterEgg = new System.Windows.Forms.PictureBox();
            this.lblDisclaimer = new System.Windows.Forms.Label();
            this.btnDonate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEasterEgg)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(62, 197);
            this.btnOk.Size = new System.Drawing.Size(119, 25);
            // 
            // picHelpIcon
            // 
            this.picHelpIcon.Location = new System.Drawing.Point(216, 8);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoSize = false;
            this.lblTitle.Location = new System.Drawing.Point(8, 10);
            this.lblTitle.Size = new System.Drawing.Size(228, 20);
            this.lblTitle.Text = "CSV Lint v";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblHorizontalLine
            // 
            this.lblHorizontalLine.Size = new System.Drawing.Size(236, 2);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(132, 187);
            this.btnCancel.Visible = false;
            // 
            // lnkInfo
            // 
            this.lnkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkInfo.LinkArea = new System.Windows.Forms.LinkArea(54, 67);
            this.lnkInfo.Location = new System.Drawing.Point(8, 49);
            this.lnkInfo.Name = "lnkInfo";
            this.lnkInfo.Size = new System.Drawing.Size(228, 30);
            this.lnkInfo.TabIndex = 2;
            this.lnkInfo.TabStop = true;
            this.lnkInfo.Tag = "1";
            this.lnkInfo.Text = "Validate, reformat and clean up dataset textfiles, by Bas de Reuver";
            this.lnkInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkInfo.UseCompatibleTextRendering = true;
            this.lnkInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // lnkGithub
            // 
            this.lnkGithub.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkGithub.Location = new System.Drawing.Point(8, 138);
            this.lnkGithub.Name = "lnkGithub";
            this.lnkGithub.Size = new System.Drawing.Size(228, 13);
            this.lnkGithub.TabIndex = 3;
            this.lnkGithub.TabStop = true;
            this.lnkGithub.Tag = "2";
            this.lnkGithub.Text = "https://github.com/BdR76/CSVLint/";
            this.lnkGithub.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // picEasterEgg
            // 
            this.picEasterEgg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picEasterEgg.Location = new System.Drawing.Point(206, 191);
            this.picEasterEgg.Name = "picEasterEgg";
            this.picEasterEgg.Size = new System.Drawing.Size(32, 32);
            this.picEasterEgg.TabIndex = 7;
            this.picEasterEgg.TabStop = false;
            this.picEasterEgg.Tag = "0";
            this.picEasterEgg.Click += new System.EventHandler(this.picEasterEgg_Click);
            // 
            // lblDisclaimer
            // 
            this.lblDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisclaimer.Location = new System.Drawing.Point(8, 85);
            this.lblDisclaimer.Name = "lblDisclaimer";
            this.lblDisclaimer.Size = new System.Drawing.Size(228, 48);
            this.lblDisclaimer.TabIndex = 2;
            this.lblDisclaimer.Text = "This software is free-to-use and it is provided as-is without warranty of any kin" +
    "d, always back-up your data files to prevent data loss.";
            this.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnDonate
            // 
            this.btnDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDonate.Location = new System.Drawing.Point(62, 165);
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Size = new System.Drawing.Size(119, 25);
            this.btnDonate.TabIndex = 8;
            this.btnDonate.Text = "Donate";
            this.btnDonate.UseVisualStyleBackColor = true;
            this.btnDonate.Click += new System.EventHandler(this.btnDonate_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(244, 229);
            this.Controls.Add(this.btnDonate);
            this.Controls.Add(this.picEasterEgg);
            this.Controls.Add(this.lnkGithub);
            this.Controls.Add(this.lblDisclaimer);
            this.Controls.Add(this.lnkInfo);
            this.Name = "AboutForm";
            this.Text = "About";
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLine, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.lnkInfo, 0);
            this.Controls.SetChildIndex(this.lblDisclaimer, 0);
            this.Controls.SetChildIndex(this.lnkGithub, 0);
            this.Controls.SetChildIndex(this.picEasterEgg, 0);
            this.Controls.SetChildIndex(this.btnDonate, 0);
            this.Controls.SetChildIndex(this.picHelpIcon, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picHelpIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEasterEgg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkInfo;
        private System.Windows.Forms.LinkLabel lnkGithub;
        private System.Windows.Forms.PictureBox picEasterEgg;
        private System.Windows.Forms.Label lblDisclaimer;
        private System.Windows.Forms.Button btnDonate;
    }
}
