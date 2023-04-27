using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Kbg.NppPluginNET.PluginInfrastructure
{
    /// <summary>
    /// Holds the BGR values of the active dark mode theme.
    /// <see href "https://github.com/notepad-plus-plus/notepad-plus-plus/blob/master/PowerEditor/src/NppDarkMode.h"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DarkModeColors
    {
        public int Background;
        public int SofterBackground;
        public int HotBackground;
        public int PureBackground;
        public int ErrorBackground;
        public int Text;
        public int DarkerText;
        public int DisabledText;
        public int LinkText;
        public int Edge;
        public int HotEdge;
        public int DisabledEdge;
    }

    /// <summary>
    /// Extends <see cref="NotepadPPGateway"/> with methods implementing Npp's dark mode API.
    /// </summary>
    public partial class NotepadPPGateway : INotepadPPGateway
    {
        public IntPtr GetDarkModeColors()
        {
            DarkModeColors darkModeColors = new DarkModeColors();
            IntPtr _cbSize = new IntPtr(Marshal.SizeOf(darkModeColors));
            IntPtr _ptrDarkModeColors = Marshal.AllocHGlobal(_cbSize);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETDARKMODECOLORS, _cbSize, _ptrDarkModeColors);
            return _ptrDarkModeColors;
        }

        public bool IsDarkModeEnabled()
        {
            IntPtr result = Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ISDARKMODEENABLED, Unused, Unused);
            return ((int)result == 1);
        }
    }

    static class NppDarkMode
    {
        public static Color BGRToColor(int bgr)
        {
            return Color.FromArgb((bgr & 0xFF), ((bgr >> 8) & 0xFF), ((bgr >> 16) & 0xFF));
        }
    }
}