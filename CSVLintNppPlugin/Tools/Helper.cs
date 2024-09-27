// -------------------------------------
// CsvDefinition
// Helper functions and methods
// -------------------------------------
using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSVLint.Tools
{
    static class Helper
    {
        /// <summary>
        /// Safely increase a count (when using a dictionary to count stuff)
        /// </summary>
        /// <typeparam name="T">Type being counted</typeparam>
        /// <param name="counts">Dictionary containging counters</param>
        /// <param name="c">Occurance that should be counted</param>
        public static void Increase<T>(this Dictionary<T, int> counts, T c)
        {
            if (!counts.ContainsKey(c))
                counts.Add(c, 1);
            else
                counts[c]++;
        }

        /// <summary>Tell the user the first time they try to use a plugin command on a too-long file, but let them turn off such notifications</summary>
        private static bool notifyUserIfFileTooLong = true;

        /// <summary>
        /// Let <c>longLen</c> be the number of bytes in the document managed by scintilla<br></br>
        /// If <c>longLen</c> is less than <see cref="int.MaxValue"/>, returns true and sets <c>length = longLen</c>.<br></br>
        /// Otherwise, <c>length = -1</c> and returns false, and if (<see cref="notifyUserIfFileTooLong"/> && notifyUser), throws up a MessageBox warning the user that the file is too long.
        /// </summary>
        public static bool TryGetLengthAsInt(IScintillaGateway scintilla, bool notifyUser, out int length)
        {
            long longLen = scintilla.GetLength();
            if (longLen >= 0 && longLen <= int.MaxValue)
            {
                length = (int)longLen;
                return true;
            }
            length = -1;
            if (notifyUser && notifyUserIfFileTooLong)
            {
                notifyUserIfFileTooLong = MessageBox.Show(
                    $"This plugin command cannot be performed on this document, because the document has more than {int.MaxValue} characters.\r\n" +
                        $"Do you want to stop showing these messages for too-long documents?",
                    "Document too long for this command",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                ) == DialogResult.No;
            }
            return false;
        }
    }
}
