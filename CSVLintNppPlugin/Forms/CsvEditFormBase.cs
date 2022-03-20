using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class CsvEditFormBase : Form
    {
        public CsvEditFormBase()
        {
            InitializeComponent();

            this.picHelpIcon.Image = Bitmap.FromHicon(SystemIcons.Question.Handle);
        }

        public void ToggleControlBasedOnControl(Control CheckCtrl, bool bEnable)
        {
            // get tag from control
            int.TryParse(CheckCtrl.Tag.ToString(), out int iTag);
            if (iTag > 0)
            {
                // check all controls on THIS form
                ToggleChildControlsWithTag(CheckCtrl, this, iTag, bEnable);
            }
        }

        public static void ToggleChildControlsWithTag(Control CheckCtrl, Control parent, int iTag, bool bEnable)
        {
            // loop through all other controls
            foreach (Control ctrl in parent.Controls)
            {
                // not the control that triggered this event
                if (ctrl != CheckCtrl)
                {
                    // try find tag, not all controls have tag
                    var t = ctrl.Tag;
                    if (t != null)
                    {
                        string tagtest = t.ToString();

                        int CtrlTag = -1;
                        if (tagtest.Contains(","))
                        {
                            var tmp = tagtest.Split(',');
                            foreach (string s in tmp)
                            {
                                if (s == iTag.ToString()) CtrlTag = iTag;
                            }
                        }
                        else
                        {
                            int.TryParse(tagtest, out CtrlTag);
                        }

                        // if tag same
                        if (CtrlTag == iTag)
                        {
                            ctrl.Enabled = bEnable;
                        }
                    }
                }

                // recursive call for child controls withing this control (for example a groupbox)
                ToggleChildControlsWithTag(CheckCtrl, ctrl, iTag, bEnable);
            }
        }

        private void picHelpIcon_Click(object sender, EventArgs e)
        {
            // icon Tag contains name of help chapter
            object tag = (sender as Control).Tag;
            string sec = (tag == null ? "" : tag.ToString());

            string url = "https://github.com/BdR76/CSVLint/tree/master/docs" + (sec == "" ? "" : "#" + sec);

            // Call the Process.Start method to open the default browser with a URL:
            System.Diagnostics.Process.Start(url);
        }
    }
}
