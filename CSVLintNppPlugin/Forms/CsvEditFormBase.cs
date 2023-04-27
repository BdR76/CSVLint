using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
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
        // OnLoad, to access the controls on derived form
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // set darkmode at first activated
            initDarkLightTheme();
        }

        private INotepadPPGateway notepad;
        private IntPtr theme_ptr;

        public void ToggleDarkLightTheme(bool isDark)
        {
            // get ptr to Notepad and darkmode theme
            notepad = new NotepadPPGateway();
            theme_ptr = notepad.GetDarkModeColors();

            // start recursive iteration with this form
            ApplyThemeColors(this, isDark);

            Marshal.FreeHGlobal(theme_ptr);
            notepad = null;
        }

        public void initDarkLightTheme()
        {
            // get ptr to Notepad and darkmode theme
            notepad = new NotepadPPGateway();
            theme_ptr = notepad.GetDarkModeColors();

            if (notepad.IsDarkModeEnabled()) {
                // start recursive iteration with this form
                ApplyThemeColors(this, true);
            }

            Marshal.FreeHGlobal(theme_ptr);
            notepad = null;
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


        // NOTE: this function is called recursively
        private void ApplyThemeColors(Control ctrl, bool isDark)
        {
            // darkmode or not
            DarkModeColors theme = (DarkModeColors)Marshal.PtrToStructure(theme_ptr, typeof(DarkModeColors)); // TODO: testing but THIS IS NOT CORRECT
            //if (isDark) theme = (DarkModeColors)Marshal.PtrToStructure(theme_ptr, typeof(DarkModeColors));

            // check for all control types
            if (ctrl.GetType() == typeof(Label))
            {
                ((Label)ctrl).BackColor = (!isDark ? Label.DefaultBackColor                      : NppDarkMode.BGRToColor(theme.PureBackground) );
                ((Label)ctrl).ForeColor = (!isDark ? Label.DefaultForeColor                      : NppDarkMode.BGRToColor(theme.Text) );
            }

            // check for all control types
            if (ctrl.GetType() == typeof(LinkLabel))
            {
                ((LinkLabel)ctrl).LinkColor =        (!isDark ? Color.Blue                       : NppDarkMode.BGRToColor(theme.LinkText));
                ((LinkLabel)ctrl).VisitedLinkColor = (!isDark ? Color.DarkMagenta                : NppDarkMode.BGRToColor(theme.LinkText));
            }

            // ButtonBase == Button, RadioButton, CheckBox
            //if (ctrl is System.Windows.Forms.ButtonBase)
            if (ctrl is System.Windows.Forms.RadioButton || ctrl is System.Windows.Forms.CheckBox)
            {
                ((ButtonBase)ctrl).BackColor = (!isDark ? Label.DefaultBackColor                 : NppDarkMode.BGRToColor(theme.PureBackground));
                ((ButtonBase)ctrl).ForeColor = (!isDark ? Label.DefaultForeColor                 : NppDarkMode.BGRToColor(theme.Text));
            }

            // edit box
            if (ctrl is System.Windows.Forms.TextBox)
            {
                ((TextBox)ctrl).BackColor = (!isDark ? TextBox.DefaultBackColor                    : NppDarkMode.BGRToColor(theme.SofterBackground));
                ((TextBox)ctrl).ForeColor = (!isDark ? TextBox.DefaultForeColor                    : NppDarkMode.BGRToColor(theme.Text));
            }

            // ListBox and ComboBox
            if (ctrl is System.Windows.Forms.ListControl)
            {
                ((ListControl)ctrl).BackColor = (!isDark ? Label.DefaultBackColor : NppDarkMode.BGRToColor(theme.SofterBackground));
                ((ListControl)ctrl).ForeColor = (!isDark ? Label.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
            }

            if (ctrl.GetType() == typeof(Button))
            {
                ((Button)ctrl).BackColor = (!isDark ? Color.FromKnownColor(KnownColor.ButtonFace) : NppDarkMode.BGRToColor(theme.SofterBackground));
                ((Button)ctrl).ForeColor = (!isDark ? Button.DefaultForeColor : NppDarkMode.BGRToColor(theme.Text));
            }

            // dialog form or panel/groupbox
            if (ctrl is System.Windows.Forms.Form || ctrl is System.Windows.Forms.GroupBox)
            {
                ((Control)ctrl).BackColor = (!isDark ? Control.DefaultBackColor                      : NppDarkMode.BGRToColor(theme.PureBackground));
                ((Control)ctrl).ForeColor = (!isDark ? Control.DefaultForeColor                      : NppDarkMode.BGRToColor(theme.Text));
            }

            // recursively call for child controls
            if (ctrl.HasChildren) {
                foreach (Control c in ctrl.Controls)
                {
                    ApplyThemeColors(c, isDark);
                }
            }
        }

        private void lblTitle_TextChanged(object sender, EventArgs e)
        {
            string txt = "View documentation on " + lblTitle.Text;
            toolTipBase.SetToolTip(this.picHelpIcon, txt);
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
