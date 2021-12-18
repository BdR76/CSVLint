using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSVLintNppPlugin.Forms
{
    public partial class CsvEditFormBase : Form
    {
        public CsvEditFormBase()
        {
            InitializeComponent();
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
                        int.TryParse(t.ToString(), out int CtrlTag);

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

    }
}
