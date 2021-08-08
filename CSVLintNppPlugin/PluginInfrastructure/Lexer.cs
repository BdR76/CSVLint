using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Kbg.NppPluginNET;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace NppPluginNET.PluginInfrastructure
{
    internal static class ILexer
    {
        public static readonly string Name = "CSVLint\0";
        public static readonly string StatusText = "CSV Linter and validator\0";

        public static char separatorChar = '\t';
        public static int[] fixedWidths;

        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

        // Properties
        static readonly Dictionary<string, bool> SupportedProperties = new Dictionary<string, bool>
        {
            { "fold", true},
            { "fold.compact", false}
        };
        static readonly Dictionary<string, string> PropertyDescription = new Dictionary<string, string>
        {
            { "fold", "Enable or disable the folding functionality."},
            { "fold.compact", "If set to 0 closing tag is visible when collapsed else hidden." }
        };
        static readonly Dictionary<string, int> PropertyTypes = new Dictionary<string, int>
        {
            { "fold", (int)SciMsg.SC_TYPE_BOOLEAN},
            { "fold.compact", (int)SciMsg.SC_TYPE_BOOLEAN }
        };

        // Styles
        static List<string> NamedStylesList = new List<string> { "SCE_CSVLINT_DEFAULT" };
        static int NamedStylesListCount = NamedStylesList.Count;
        static List<string> TagsOfStyleList = new List<string> { "default" };
        static List<string> DescriptionOfStyleList = new List<string> { "Default style" };

        // 1. since cpp defines this as an interface with virtual functions, 
        //      there is an implicit first parameter, the class instance
        // 2. according to c# documentation delegates are used to simulate function pointers

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerVersion(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerRelease(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPropertyNames(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerPropertyType(IntPtr instance, IntPtr name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescribeProperty(IntPtr instance, IntPtr name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPropertySet(IntPtr instance, IntPtr key, IntPtr val);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescribeWordListSets(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerWordListSet(IntPtr instance, int kw_list_index, IntPtr key_word_list);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerLex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerFold(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPrivateCall(IntPtr instance, int operation, IntPtr pointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerLineEndTypesSupported(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerAllocateSubStyles(IntPtr instance, int style_base, int number_styles);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerSubStylesStart(IntPtr instance, int style_base);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerSubStylesLength(IntPtr instance, int style_base);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerStyleFromSubStyle(IntPtr instance, int sub_style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerPrimaryStyleFromStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerFreeSubStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerSetIdentifiers(IntPtr instance, int style, IntPtr identifiers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerDistanceToSecondaryStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerGetSubStyleBases(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerNamedStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerNameOfStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerTagsOfStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescriptionOfStyle(IntPtr instance, int style);

        [StructLayout(LayoutKind.Sequential)]
        public struct ILexer4
        {
            public ILexerVersion Version;
            public ILexerRelease Release;
            public ILexerPropertyNames PropertyNames;
            public ILexerPropertyType PropertyType;
            public ILexerDescribeProperty DescribeProperty;
            public ILexerPropertySet PropertySet;
            public ILexerDescribeWordListSets DescribeWordListSets;
            public ILexerWordListSet WordListSet;
            public ILexerLex Lex;
            public ILexerFold Fold;
            public ILexerPrivateCall PrivateCall;
            public ILexerLineEndTypesSupported LineEndTypesSupported;
            public ILexerAllocateSubStyles AllocateSubStyles;
            public ILexerSubStylesStart SubStylesStart;
            public ILexerSubStylesLength SubStylesLength;
            public ILexerStyleFromSubStyle StyleFromSubStyle;
            public ILexerPrimaryStyleFromStyle PrimaryStyleFromStyle;
            public ILexerFreeSubStyles FreeSubStyles;
            public ILexerSetIdentifiers SetIdentifiers;
            public ILexerDistanceToSecondaryStyles DistanceToSecondaryStyles;
            public ILexerGetSubStyleBases GetSubStyleBases;
            public ILexerNamedStyles NamedStyles;
            public ILexerNameOfStyle NameOfStyle;
            public ILexerTagsOfStyle TagsOfStyle;
            public ILexerDescriptionOfStyle DescriptionOfStyle;
        }

        static ILexer4 ilexer4 = new ILexer4 { };

        public static IntPtr ILexerImplementation()
        {
            if (ilexer4.Version == null) {
                // simulate a c++ vtable by creating an array of 25 function pointers
                ilexer4.Version = new ILexerVersion(Version);
                ilexer4.Release = new ILexerRelease(Release);
                ilexer4.PropertyNames = new ILexerPropertyNames(PropertyNames);
                ilexer4.PropertyType = new ILexerPropertyType(PropertyType);
                ilexer4.DescribeProperty = new ILexerDescribeProperty(DescribeProperty);
                ilexer4.PropertySet = new ILexerPropertySet(PropertySet);
                ilexer4.DescribeWordListSets = new ILexerDescribeWordListSets(DescribeWordListSets);
                ilexer4.WordListSet = new ILexerWordListSet(WordListSet);
                ilexer4.Lex = new ILexerLex(Lex);
                ilexer4.Fold = new ILexerFold(Fold);
                ilexer4.PrivateCall = new ILexerPrivateCall(PrivateCall);
                ilexer4.LineEndTypesSupported = new ILexerLineEndTypesSupported(LineEndTypesSupported);
                ilexer4.AllocateSubStyles = new ILexerAllocateSubStyles(AllocateSubStyles);
                ilexer4.SubStylesStart = new ILexerSubStylesStart(SubStylesStart);
                ilexer4.SubStylesLength = new ILexerSubStylesLength(SubStylesLength);
                ilexer4.StyleFromSubStyle = new ILexerStyleFromSubStyle(StyleFromSubStyle);
                ilexer4.PrimaryStyleFromStyle = new ILexerPrimaryStyleFromStyle(PrimaryStyleFromStyle);
                ilexer4.FreeSubStyles = new ILexerFreeSubStyles(FreeSubStyles);
                ilexer4.SetIdentifiers = new ILexerSetIdentifiers(SetIdentifiers);
                ilexer4.DistanceToSecondaryStyles = new ILexerDistanceToSecondaryStyles(DistanceToSecondaryStyles);
                ilexer4.GetSubStyleBases = new ILexerGetSubStyleBases(GetSubStyleBases);
                ilexer4.NamedStyles = new ILexerNamedStyles(NamedStyles);
                ilexer4.NameOfStyle = new ILexerNameOfStyle(NameOfStyle);
                ilexer4.TagsOfStyle = new ILexerTagsOfStyle(TagsOfStyle);
                ilexer4.DescriptionOfStyle = new ILexerDescriptionOfStyle(DescriptionOfStyle);
            }

            IntPtr vtable = Marshal.AllocHGlobal(Marshal.SizeOf(ilexer4));
            Marshal.StructureToPtr(ilexer4, vtable, false);
            IntPtr vtable_pointer = Marshal.AllocHGlobal(Marshal.SizeOf(vtable));
            Marshal.StructureToPtr(vtable, vtable_pointer, false);
            return vtable_pointer;  // return the address of the fake vtable
        }

        // virtual int SCI_METHOD Version() const = 0
        public static int Version(IntPtr instance)
        {
            /*returns an enumerated value specifying which version of the interface is implemented: 
             * lvRelease5 for ILexer5 and lvRelease4 for ILexer4. 
             * ILexer5 must be provided for Scintilla version 5.0 or later.
             */
            GC.Collect();  // test to see if the methods do get garbage collected
            return 2;
        }

        // virtual void SCI_METHOD Release() = 0
        public static void Release(IntPtr instance)
        {
            // is called to destroy the lexer object.
        }

        // virtual const char * SCI_METHOD PropertyNames() = 0
        public static IntPtr PropertyNames(IntPtr instance)
        {
            /*  returns a string with all of the valid properties separated by "\n".
             *  If the lexer does not support this call then an empty string is returned. 
             */
            return Marshal.StringToHGlobalAnsi(string.Join(Environment.NewLine, SupportedProperties.Keys));
        }

        // virtual int SCI_METHOD PropertyType(const char *name) = 0
        public static int PropertyType(IntPtr instance, IntPtr name)
        {
            // Properties may be boolean (SC_TYPE_BOOLEAN), integer (SC_TYPE_INTEGER), or string (SC_TYPE_STRING) 
            return PropertyTypes[Marshal.PtrToStringAnsi(name)];
        }

        // virtual const char * SCI_METHOD DescribeProperty(const char *name) = 0
        public static IntPtr DescribeProperty(IntPtr instance, IntPtr name)
        {
            // A description of a property in English
            return Marshal.StringToHGlobalAnsi(PropertyDescription[Marshal.PtrToStringAnsi(name)]);
        }

        // virtual i64 SCI_METHOD PropertySet(const char *key, const char *val) = 0
        public static IntPtr PropertySet(IntPtr instance, IntPtr key, IntPtr val)
        {
            /* The return values from PropertySet and WordListSet are used to indicate 
             * whether the change requires performing lexing or folding over any of the document. 
             * It is the position at which to restart lexing and folding or -1 
             * if the change does not require any extra work on the document. 
             * A simple approach is to return 0 if there is any possibility that a change 
             * requires lexing the document again while an optimisation could be to remember 
             * where a setting first affects the document and return that position.
             */
            string name = Marshal.PtrToStringAnsi(key);
            string value = Marshal.PtrToStringAnsi(val);
            SupportedProperties[name] = value == "0" ? false : true;
            return IntPtr.Zero;
        }

        // virtual const char * SCI_METHOD DescribeWordListSets() = 0
        public static IntPtr DescribeWordListSets(IntPtr instance)
        {
            // ? A string describing the different keywords, but how to separate is the question
            return IntPtr.Zero;
        }

        // virtual i64 SCI_METHOD WordListSet(int n, const char *wl) = 0
        public static IntPtr WordListSet(IntPtr instance, int kw_list_index, IntPtr key_word_list)
        {
            // CSV Lint doesn't use keyword lists
            return IntPtr.Zero;
        }

        // virtual void SCI_METHOD Lex(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Lex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            /* main lexing method. 
             * start_pos is always the startposition of a line
             * length_doc is NOT the total length of a document but the size of the text to be styled
             * init_style is the style of last styled byte
             * p_access is the pointer of the IDocument cpp class
             */

            //var sep = ';';
            //Main.GetCurrentFileLexerParameters(out sep);
            //separatorChar = sep;
            

            int start = (int)start_pos;
            int length = (int)length_doc;
            IntPtr range_ptr = editor.GetRangePointer(start, length);
            string content = Marshal.PtrToStringAnsi(range_ptr, length);

            // column color index
            int idx = 1;
            bool isEOL;

            //editor.StyleClearAll();

            for (int i = 0; i < length; i++)
            {
                int start_position = i;
                isEOL = false;

                while (i < length)
                {
                    if (content[i] == separatorChar)
                    {
                        break;
                    }
                    // read rest of the line
                    if (content[i] == '\n')
                    {
                        isEOL = true;
                        break;
                    }
                    i++;
                }
                // style this column
                editor.StartStyling(start + start_position, 0);
                editor.SetStyling(i - start_position, idx);

                // next color
                idx++;

                if ((idx > 8) || (isEOL)) idx = 1; // reset end of line
            }
        }
        
        // virtual void SCI_METHOD Fold(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Fold(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            /* 
             * is called with the exact range that needs folding. 
             * Previously, lexers were called with a range that started one line 
             * before the range that needs to be folded as this allowed fixing up 
             * the last line from the previous folding. 
             * The new approach allows the lexer to decide whether to backtrack 
             * or to handle this more efficiently.
             * 
             * Lessons I have learned so far are
             * - do not start with a base level of 0 to simplify the arithmetic int calculation
             * - scintilla recommends to use 0x400 as a base level
             * - when the value becomes smaller than the base value, set the base value
             * - create an additional margin in which you set the levels of the respective lines, 
             *      so it is easy to see when something breaks.
             */

            // CSV Lint -> no folding
            // TODO: maybe add folding to group 10x records or something?
            // 
            //int start = (int)start_pos;
            //int length = (int)length_doc;
            //IntPtr range_ptr = editor.GetRangePointer(start, length);
            //string content = Marshal.PtrToStringAnsi(range_ptr, length);
            //
            //
            //int cur_level = (int)SciMsg.SC_FOLDLEVELBASE;
            //int cur_line = editor.LineFromPosition(start);
            //
            //if (cur_line > 0)
            //{
            //    int prev_level = (int)editor.GetFoldLevel(cur_line - 1);
            //    bool header_flag_set = (prev_level & (int)SciMsg.SC_FOLDLEVELHEADERFLAG) == (int)SciMsg.SC_FOLDLEVELHEADERFLAG;
            //
            //    if (header_flag_set)
            //    {
            //        cur_level = (prev_level & (int)SciMsg.SC_FOLDLEVELNUMBERMASK) + 1;
            //    }
            //    else
            //    {
            //        cur_level = (prev_level & (int)SciMsg.SC_FOLDLEVELNUMBERMASK);
            //    }
            //}
            //
            //int next_level = cur_level;
            //
            //for (int i = 0; i < length; i++)
            //{
            //
            //    if (!SupportedProperties["fold"])
            //    {
            //        editor.SetFoldLevel(cur_line, (int)SciMsg.SC_FOLDLEVELBASE);
            //        while (i < length)
            //        {
            //            // read rest of the line
            //            if (content[i] == '\n') { break; }
            //            i++;
            //        }
            //        cur_line++;
            //        continue;
            //    }
            //
            //    string tag = "";
            //    if (i + 2 < length) { tag = content.Substring(i, 3); }
            //
            //
            //    if (FoldOpeningTags.Contains(tag))
            //    {
            //        next_level++;
            //        cur_level |= (int)SciMsg.SC_FOLDLEVELHEADERFLAG;
            //    }
            //    else if (FoldClosingTags.Contains(tag))
            //    {
            //        next_level--;
            //        if (SupportedProperties["fold.compact"]) { cur_level--; }
            //        cur_level &= (int)SciMsg.SC_FOLDLEVELNUMBERMASK;
            //    }
            //    else
            //    {
            //        cur_level &= (int)SciMsg.SC_FOLDLEVELNUMBERMASK;
            //    }
            //
            //    while (i < length)
            //    {
            //        // read rest of the line
            //        if (content[i] == '\n') { break; }
            //        i++;
            //    }
            //    // set fold level
            //    if (cur_level < (int)SciMsg.SC_FOLDLEVELBASE)
            //    {
            //        cur_level = (int)SciMsg.SC_FOLDLEVELBASE;
            //    }
            //    editor.SetFoldLevel(cur_line, cur_level);
            //    cur_line++;
            //    cur_level = next_level;
            //
            //}
        }

        // virtual int SCI_METHOD NamedStyles() = 0;
        public static int NamedStyles(IntPtr instance)
        {
            // Retrieve the number of named styles for the lexer.
            return NamedStylesListCount;
        }

        // virtual const char * SCI_METHOD NameOfStyle(int style) = 0;
        public static IntPtr NameOfStyle(IntPtr instance, int style)
        {
            // Retrieve the name of a style.
            return Marshal.StringToHGlobalAnsi(NamedStylesList[style]);
        }

        // virtual const char * SCI_METHOD TagsOfStyle(int style) = 0;
        public static IntPtr TagsOfStyle(IntPtr instance, int style)
        {
            /*
             * Retrieve the tags of a style.
             * This is a space-separated set of words like "comment documentation".
             */
            return Marshal.StringToHGlobalAnsi(TagsOfStyleList[style]);
        }

        // virtual const char * SCI_METHOD DescriptionOfStyle(int style) = 0;
        public static IntPtr DescriptionOfStyle(IntPtr instance, int style)
        {
            /* 
             * Retrieve an English-language description of a style which may be suitable 
             * for display in a user interface.
             * This looks like "Doc comment: block comments beginning with /** or /*!".
             */
            return Marshal.StringToHGlobalAnsi(DescriptionOfStyleList[style]);
        }

        // virtual int SCI_METHOD LineEndTypesSupported() = 0;
        public static int LineEndTypesSupported(IntPtr instance)
        {
            /*
             *  reports the different types of line ends supported by the current lexer. 
             *  This is a bit set although there is currently only a single choice
             *  with either SC_LINE_END_TYPE_DEFAULT (0) or SC_LINE_END_TYPE_UNICODE (1). 
             *  These values are also used by the other messages concerned with Unicode line ends.
             */
            return (int)SciMsg.SC_LINE_END_TYPE_DEFAULT;
        }

        // virtual void * SCI_METHOD PrivateCall(int operation, void *pointer) = 0;
        public static IntPtr PrivateCall(IntPtr instance, int operation, IntPtr pointer)
        {
            /* allows for direct communication between the application and a lexer.
             * An example would be where an application maintains a single large data structure 
             * containing symbolic information about system headers(like Windows.h) 
             * and provides this to the lexer where it can be applied to each document.
             * This avoids the costs of constructing the system header information for each document.
             * This is invoked with the SCI_PRIVATELEXERCALL API.
             */
            return IntPtr.Zero;
        }


        /***********************************************************************************************
         * 
         * NOTE:
         * 
         * THE FOLLOWING METHODS ARE NEEDED ONLY IF THE DOCUMENT IS TO BE LEXED WITH DIFFERENT LEXERS,
         * LIKE THE HTML LEXER DOES, WHICH BESIDES HTML CODE ALSO HAS TO LEX PHP, JS etc.
         * 
         * THE IMPLEMENTATION IS LEFT TO THOSE INTERESTED :)
         * 
         * *********************************************************************************************/


        // virtual int SCI_METHOD AllocateSubStyles(int styleBase, int numberStyles) = 0;
        public static int AllocateSubStyles(IntPtr instance, int style_base, int number_styles)
        {
            /* Allocate some number of substyles for a particular base style, 
             * returning the first substyle number allocated.
             * Substyles are allocated contiguously.
             */
            return -1;
        }

        // virtual int SCI_METHOD SubStylesStart(int styleBase) = 0;
        public static int SubStylesStart(IntPtr instance, int style_base)
        {
            // Return the start of the substyles allocated for a base style.
            return -1;
        }

        // virtual int SCI_METHOD SubStylesLength(int styleBase) = 0;
        public static int SubStylesLength(IntPtr instance, int style_base)
        {
            // Return the length of the substyles allocated for a base style.
            return 0;
        }

        // virtual int SCI_METHOD StyleFromSubStyle(int subStyle) = 0;
        public static int StyleFromSubStyle(IntPtr instance, int sub_style)
        {
            // For a sub style, return the base style, else return the argument.
            return 0;
        }

        // virtual int SCI_METHOD PrimaryStyleFromStyle(int style) = 0;
        public static int PrimaryStyleFromStyle(IntPtr instance, int style)
        {
            // For a secondary style, return the primary style, else return the argument.
            return 0;
        }

        // virtual void SCI_METHOD FreeSubStyles() = 0;
        public static void FreeSubStyles(IntPtr instance)
        {
            // Free all allocated substyles.
        }

        // virtual void SCI_METHOD SetIdentifiers(int style, const char *identifiers) = 0;
        public static void SetIdentifiers(IntPtr instance, int style, IntPtr identifiers)
        {
            /* Similar to SCI_SETKEYWORDS but for substyles. 
             * The prefix feature available with SCI_SETKEYWORDS is not implemented for SCI_SETIDENTIFIERS.
             */
        }

        // virtual int SCI_METHOD DistanceToSecondaryStyles() = 0;
        public static int DistanceToSecondaryStyles(IntPtr instance)
        {
            // Returns the distance between a primary style and its corresponding secondary style.
            return 0;
        }

        // virtual const char * SCI_METHOD GetSubStyleBases() = 0;
        public static IntPtr GetSubStyleBases(IntPtr instance)
        {
            // Fill styles with a byte for each style that can be split into substyles.
            return IntPtr.Zero;
        }
    }
}
