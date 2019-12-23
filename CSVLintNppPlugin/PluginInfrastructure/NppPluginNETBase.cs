// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System;

namespace Kbg.NppPluginNET.PluginInfrastructure
{
    class PluginBase
    {
        internal static NppData nppData;
        internal static FuncItems _funcItems = new FuncItems();
        protected static ScintillaGateway[] scintillaGateways = new ScintillaGateway[2];

        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }
        
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }
        
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }
        
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
        }

        internal static IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(nppData._nppHandle, (uint) NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
        }


        static readonly Func<IScintillaGateway> gatewayFactory = () => new ScintillaGateway(GetCurrentScintilla());

        public static Func<IScintillaGateway> GetGatewayFactory()
        {
            return gatewayFactory;
        }

        /// <summary> Get gateway to currently active scintilla  </summary>
        /// <remarks>
        /// Notepad++ has two instances of Scintilla - the main one, and a second one that is only used when you show two documents side-by-side.
        /// Since a document can be moved between these at any time, we need to check current scintilla constantly (or listen to events, but meh)
        /// </remarks>
        public static ScintillaGateway CurrentScintillaGateway
        {
            get
            {
                Win32.SendMessage(nppData._nppHandle, (uint)NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out int curScintilla);
                return scintillaGateways[curScintilla] ?? (scintillaGateways[curScintilla] = new ScintillaGateway(
                           curScintilla == 0
                               ? nppData._scintillaMainHandle
                               : nppData._scintillaSecondHandle));
            }
        }
    }
}
