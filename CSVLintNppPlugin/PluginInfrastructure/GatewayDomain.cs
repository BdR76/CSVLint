// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Kbg.NppPluginNET.PluginInfrastructure
{
    /// <summary>
    /// Colours are set using the RGB format (Red, Green, Blue). The intensity of each colour is set in the range 0 to 255.
    /// If you have three such intensities, they are combined as: red | (green &lt;&lt; 8) | (blue &lt;&lt; 16).
    /// If you set all intensities to 255, the colour is white. If you set all intensities to 0, the colour is black.
    /// When you set a colour, you are making a request. What you will get depends on the capabilities of the system and the current screen mode.
    /// </summary>
    public class Colour
    {
        public readonly int Red, Green, Blue;

        public Colour(int rgb)
		{
			Red = rgb & 0xFF;
			Green = (rgb >> 8) & 0xFF;
			Blue = (rgb >> 16) & 0xFF;
		}

        public Colour(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public int Value => Red + (Green << 8) + (Blue << 16);
    }

    /// <summary>
    /// Colours are set using the RGB format (Red, Green, Blue). The intensity of each colour is set in the range 0 to 255.
    /// If you have three such intensities, they are combined as: red | (green &lt;&lt; 8) | (blue &lt;&lt; 16).
    /// If you set all intensities to 255, the colour is white. If you set all intensities to 0, the colour is black.
    /// When you set a colour, you are making a request. What you will get depends on the capabilities of the system and the current screen mode.
    /// </summary>
    public class ColourAlpha
    {
        public readonly int Red, Green, Blue, Alpha;

        public ColourAlpha(Int64 rgba)
        {
            Red = (byte)rgba & 0xFF;
            Green = (byte)(rgba >> 8) & 0xFF;
            Blue = (byte)(rgba >> 16) & 0xFF;
            Alpha = (byte)(rgba >> 24) & 0xFF;
        }

        public ColourAlpha(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public int Value => Red + (Green << 8) + (Blue << 16) + (Alpha << 24);
    }

    /// <summary>
    /// Positions within the Scintilla document refer to a character or the gap before that character.
    /// The first character in a document is 0, the second 1 and so on. If a document contains nLen characters, the last character is numbered nLen-1. The caret exists between character positions and can be located from before the first character (0) to after the last character (nLen).
    ///
    /// There are places where the caret can not go where two character bytes make up one character.
    /// This occurs when a DBCS character from a language like Japanese is included in the document or when line ends are marked with the CP/M
    /// standard of a carriage return followed by a line feed.The INVALID_POSITION constant(-1) represents an invalid position within the document.
    ///
    /// All lines of text in Scintilla are the same height, and this height is calculated from the largest font in any current style.This restriction
    /// is for performance; if lines differed in height then calculations involving positioning of text would require the text to be styled first.
    ///
    /// If you use messages, there is nothing to stop you setting a position that is in the middle of a CRLF pair, or in the middle of a 2 byte character.
    /// However, keyboard commands will not move the caret into such positions.
    /// </summary>
    public class Position : IEquatable<Position>
    {
        private readonly Int64 pos;

        public Position(IntPtr ptr) : this(ptr.ToInt64())
        { }

        public Position(Int64 pos)
        {
            this.pos = pos;
        }

        public Int64 Value
        {
            get { return pos; }
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.pos + b.pos);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.pos - b.pos);
        }

        public static bool operator ==(Position a, Position b)
        {
	        if (ReferenceEquals(a, b))
		        return true;
			if (ReferenceEquals(a, null))
				return false;
			if (ReferenceEquals(b, null))
				return false;
			return  a.pos == b.pos;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public static bool operator >(Position a, Position b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(Position a, Position b)
        {
            return a.Value < b.Value;
        }

        public static Position Min(Position a, Position b)
        {
            if (a < b)
                return a;
            return b;
        }

		public static Position Max(Position a, Position b)
		{
			if (a > b)
				return a;
			return b;
		}

		public override string ToString()
        {
            return "Postion: " + pos;
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return pos == other.pos;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position)obj);
        }

        //public static implicit operator Position(int i) => new Position(i);
        //public static implicit operator int(Position i) => i.pos;

        public override int GetHashCode()
        {
            return pos.GetHashCode();
        }
    }

    /// <summary>
    /// Class containing key and modifiers
    ///
    /// The key code is a visible or control character or a key from the SCK_* enumeration, which contains:
    /// SCK_ADD, SCK_BACK, SCK_DELETE, SCK_DIVIDE, SCK_DOWN, SCK_END, SCK_ESCAPE, SCK_HOME, SCK_INSERT, SCK_LEFT, SCK_MENU, SCK_NEXT(Page Down), SCK_PRIOR(Page Up), S
    /// CK_RETURN, SCK_RIGHT, SCK_RWIN, SCK_SUBTRACT, SCK_TAB, SCK_UP, and SCK_WIN.
    ///
    /// The modifiers are a combination of zero or more of SCMOD_ALT, SCMOD_CTRL, SCMOD_SHIFT, SCMOD_META, and SCMOD_SUPER.
    /// On OS X, the Command key is mapped to SCMOD_CTRL and the Control key to SCMOD_META.SCMOD_SUPER is only available on GTK+ which is commonly the Windows key.
    /// If you are building a table, you might want to use SCMOD_NORM, which has the value 0, to mean no modifiers.
    /// </summary>
    public class KeyModifier
    {
        private readonly int value;

        /// <summary>
        /// The key code is a visible or control character or a key from the SCK_* enumeration, which contains:
        /// SCK_ADD, SCK_BACK, SCK_DELETE, SCK_DIVIDE, SCK_DOWN, SCK_END, SCK_ESCAPE, SCK_HOME, SCK_INSERT, SCK_LEFT, SCK_MENU, SCK_NEXT(Page Down), SCK_PRIOR(Page Up),
        /// SCK_RETURN, SCK_RIGHT, SCK_RWIN, SCK_SUBTRACT, SCK_TAB, SCK_UP, and SCK_WIN.
        ///
        /// The modifiers are a combination of zero or more of SCMOD_ALT, SCMOD_CTRL, SCMOD_SHIFT, SCMOD_META, and SCMOD_SUPER.
        /// On OS X, the Command key is mapped to SCMOD_CTRL and the Control key to SCMOD_META.SCMOD_SUPER is only available on GTK+ which is commonly the Windows key.
        /// If you are building a table, you might want to use SCMOD_NORM, which has the value 0, to mean no modifiers.
        /// </summary>
        public KeyModifier(SciMsg SCK_KeyCode, SciMsg SCMOD_modifier)
        {
            value = (int) SCK_KeyCode | ((int) SCMOD_modifier << 16);
        }

        public int Value
        {
            get { return Value; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterRange
    {
        public CharacterRange(IntPtr cpmin, IntPtr cpmax) { cpMin = cpmin; cpMax = cpmax; }
        public IntPtr cpMin;
        public IntPtr cpMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterRangeFull
    {
        public CharacterRangeFull(IntPtr cpmin, IntPtr cpmax) { cpMin = cpmin; cpMax = cpmax; }
        public IntPtr cpMin;
        public IntPtr cpMax;
    }

    /// <summary>
    /// This is used before N++ 8.3
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterRangeLegacy
    {
        public int cpMin;
        public int cpMax;
    }

    public class Cells
    {
        char[] charactersAndStyles;

        public Cells(char[] charactersAndStyles)
        {
            this.charactersAndStyles = charactersAndStyles;
        }
        public char[] Value { get { return charactersAndStyles; } }
    }

    public class TextRange : IDisposable
    {
        Sci_TextRange _sciTextRange;
        IntPtr _ptrSciTextRange;
        bool _disposed = false;

        public TextRange(CharacterRange chrRange, long stringCapacity)
            : this(chrRange.cpMin, chrRange.cpMax, stringCapacity)
        { }

        public TextRange(IntPtr cpmin, IntPtr cpmax, long stringCapacity = 0)
        {
            // The capacity must be _at least_ the given range plus one
            stringCapacity = Math.Max(stringCapacity, Math.Abs(cpmax.ToInt64() - cpmin.ToInt64()) + 1);

            _sciTextRange.chrg.cpMin = cpmin;
            _sciTextRange.chrg.cpMax = cpmax;
            _sciTextRange.lpstrText = Marshal.AllocHGlobal(new IntPtr(stringCapacity));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Sci_TextRange
        {
            public CharacterRange chrg;
            public IntPtr lpstrText;
        }

        public IntPtr NativePointer { get { _initNativeStruct(); return _ptrSciTextRange; } }

        public string lpstrText { get { _readNativeStruct(); return Marshal.PtrToStringAnsi(_sciTextRange.lpstrText); } }

        public CharacterRange chrg { get { _readNativeStruct(); return _sciTextRange.chrg; } set { _sciTextRange.chrg = value; _initNativeStruct(); } }


        void _initNativeStruct()
        {
            if (_ptrSciTextRange == IntPtr.Zero)
                _ptrSciTextRange = Marshal.AllocHGlobal(Marshal.SizeOf(_sciTextRange));
            Marshal.StructureToPtr(_sciTextRange, _ptrSciTextRange, false);
        }

        void _readNativeStruct()
        {
            if (_ptrSciTextRange != IntPtr.Zero)
                _sciTextRange = (Sci_TextRange)Marshal.PtrToStructure(_ptrSciTextRange, typeof(Sci_TextRange));
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_sciTextRange.lpstrText != IntPtr.Zero) Marshal.FreeHGlobal(_sciTextRange.lpstrText);
                if (_ptrSciTextRange != IntPtr.Zero) Marshal.FreeHGlobal(_ptrSciTextRange);
                _disposed = true;
            }
        }

        ~TextRange()
        {
            Dispose();
        }
    }
    public class TextRangeFull : IDisposable
    {
        Sci_TextRangeFull _sciTextRangeFull;
        IntPtr _ptrSciTextRangeFull;
        bool _disposed = false;

        public TextRangeFull(CharacterRange chrRange, long stringCapacity)
            : this(chrRange.cpMin, chrRange.cpMax, stringCapacity)
        { }

        public TextRangeFull(IntPtr cpmin, IntPtr cpmax, long stringCapacity = 0)
        {
            // The capacity must be _at least_ the given range plus one
            stringCapacity = Math.Max(stringCapacity, Math.Abs(cpmax.ToInt64() - cpmin.ToInt64()) + 1);

            _sciTextRangeFull.chrg.cpMin = cpmin;
            _sciTextRangeFull.chrg.cpMax = cpmax;
            _sciTextRangeFull.lpstrText = Marshal.AllocHGlobal(new IntPtr(stringCapacity));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Sci_TextRangeFull
        {
            public CharacterRange chrg;
            public IntPtr lpstrText;
        }

        public IntPtr NativePointer { get { _initNativeStruct(); return _ptrSciTextRangeFull; } }

        public string lpstrText { get { _readNativeStruct(); return Marshal.PtrToStringAnsi(_sciTextRangeFull.lpstrText); } }

        public CharacterRange chrg { get { _readNativeStruct(); return _sciTextRangeFull.chrg; } set { _sciTextRangeFull.chrg = value; _initNativeStruct(); } }


        void _initNativeStruct()
        {
            if (_ptrSciTextRangeFull == IntPtr.Zero)
                _ptrSciTextRangeFull = Marshal.AllocHGlobal(Marshal.SizeOf(_sciTextRangeFull));
            Marshal.StructureToPtr(_sciTextRangeFull, _ptrSciTextRangeFull, false);
        }

        void _readNativeStruct()
        {
            if (_ptrSciTextRangeFull != IntPtr.Zero)
                _sciTextRangeFull = (Sci_TextRangeFull)Marshal.PtrToStructure(_ptrSciTextRangeFull, typeof(Sci_TextRangeFull));
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_sciTextRangeFull.lpstrText != IntPtr.Zero) Marshal.FreeHGlobal(_sciTextRangeFull.lpstrText);
                if (_ptrSciTextRangeFull != IntPtr.Zero) Marshal.FreeHGlobal(_ptrSciTextRangeFull);
                _disposed = true;
            }
        }

        ~TextRangeFull()
        {
            Dispose();
        }
    }


    /* ++Autogenerated -- start of section automatically generated from Scintilla.iface */
        /// <summary>Is undo history being collected? (Scintilla feature SCWS_)</summary>
        public enum WhiteSpace
        {
            INVISIBLE = 0,
            VISIBLEALWAYS = 1,
            VISIBLEAFTERINDENT = 2,
            VISIBLEONLYININDENT = 3
        }
        /// <summary>Make white space characters invisible, always visible or visible outside indentation. (Scintilla feature SCTD_)</summary>
        public enum TabDrawMode
        {
            LONGARROW = 0,
            STRIKEOUT = 1
        }
        /// <summary>Retrieve the position of the last correctly styled character. (Scintilla feature SC_EOL_)</summary>
        public enum EndOfLine
        {
            CRLF = 0,
            CR = 1,
            LF = 2
        }
        /// <summary>Get the locale for displaying text. (Scintilla feature SC_IME_)</summary>
        public enum IMEInteraction
        {
            WINDOWED = 0,
            INLINE = 1
        }
        /// <summary>Choose to display the IME in a window or inline. (Scintilla feature SC_ALPHA_)</summary>
        public enum Alpha
        {
            TRANSPARENT = 0,
            OPAQUE = 255,
            NOALPHA = 256
        }
        /// <summary>Choose to display the IME in a window or inline. (Scintilla feature SC_CURSOR)</summary>
        public enum CursorShape
        {
            NORMAL = -1,
            ARROW = 2,
            WAIT = 4,
            REVERSEARROW = 7
        }
        /// <summary>Choose to display the IME in a window or inline. (Scintilla feature SC_MARK_)</summary>
        public enum MarkerSymbol
        {
            CIRCLE = 0,
            ROUNDRECT = 1,
            ARROW = 2,
            SMALLRECT = 3,
            SHORTARROW = 4,
            EMPTY = 5,
            ARROWDOWN = 6,
            MINUS = 7,
            PLUS = 8,
            VLINE = 9,
            LCORNER = 10,
            TCORNER = 11,
            BOXPLUS = 12,
            BOXPLUSCONNECTED = 13,
            BOXMINUS = 14,
            BOXMINUSCONNECTED = 15,
            LCORNERCURVE = 16,
            TCORNERCURVE = 17,
            CIRCLEPLUS = 18,
            CIRCLEPLUSCONNECTED = 19,
            CIRCLEMINUS = 20,
            CIRCLEMINUSCONNECTED = 21,
            BACKGROUND = 22,
            DOTDOTDOT = 23,
            ARROWS = 24,
            PIXMAP = 25,
            FULLRECT = 26,
            LEFTRECT = 27,
            AVAILABLE = 28,
            UNDERLINE = 29,
            RGBAIMAGE = 30,
            BOOKMARK = 31,
            VERTICALBOOKMARK = 32,
            BAR = 33,
            CHARACTER = 10000
        }
        /// <summary>Invisible mark that only sets the line background colour. (Scintilla feature SC_MARKNUM_)</summary>
        public enum MarkerOutline
        {
            HISTORY_REVERTED_TO_ORIGIN = 21,
            HISTORY_SAVED = 22,
            HISTORY_MODIFIED = 23,
            HISTORY_REVERTED_TO_MODIFIED = 24,
            FOLDEREND = 25,
            FOLDEROPENMID = 26,
            FOLDERMIDTAIL = 27,
            FOLDERTAIL = 28,
            FOLDERSUB = 29,
            FOLDER = 30,
            FOLDEROPEN = 31
        }
        /// <summary>Set the layer used for a marker that is drawn in the text area, not the margin. (Scintilla feature SC_MARGIN_)</summary>
        public enum MarginType
        {
            SYMBOL = 0,
            NUMBER = 1,
            BACK = 2,
            FORE = 3,
            TEXT = 4,
            RTEXT = 5,
            COLOUR = 6
        }
        /// <summary>Styles in range 32..39 are predefined for parts of the UI and are not used as normal styles. (Scintilla feature STYLE_)</summary>
        public enum StylesCommon
        {
            DEFAULT = 32,
            LINENUMBER = 33,
            BRACELIGHT = 34,
            BRACEBAD = 35,
            CONTROLCHAR = 36,
            INDENTGUIDE = 37,
            CALLTIP = 38,
            FOLDDISPLAYTEXT = 39,
            LASTPREDEFINED = 39,
            MAX = 255
        }
        /// <summary>
        /// Character set identifiers are used in StyleSetCharacterSet.
        /// The values are the same as the Windows *_CHARSET values.
        /// (Scintilla feature SC_CHARSET_)
        /// </summary>
        public enum CharacterSet
        {
            ANSI = 0,
            DEFAULT = 1,
            BALTIC = 186,
            CHINESEBIG5 = 136,
            EASTEUROPE = 238,
            GB2312 = 134,
            GREEK = 161,
            HANGUL = 129,
            MAC = 77,
            OEM = 255,
            RUSSIAN = 204,
            OEM866 = 866,
            CYRILLIC = 1251,
            SHIFTJIS = 128,
            SYMBOL = 2,
            TURKISH = 162,
            JOHAB = 130,
            HEBREW = 177,
            ARABIC = 178,
            VIETNAMESE = 163,
            THAI = 222,
            _8859_15 = 1000
        }
        /// <summary>Set a style to be underlined or not. (Scintilla feature SC_CASE_)</summary>
        public enum CaseVisible
        {
            MIXED = 0,
            UPPER = 1,
            LOWER = 2,
            CAMEL = 3
        }
        /// <summary>Get the size of characters of a style in points multiplied by 100 (Scintilla feature SC_WEIGHT_)</summary>
        public enum FontWeight
        {
            NORMAL = 400,
            SEMIBOLD = 600,
            BOLD = 700
        }
        /// <summary>Get the invisible representation for a style. (Scintilla feature SC_ELEMENT_)</summary>
        public enum Element
        {
            LIST = 0,
            LIST_BACK = 1,
            LIST_SELECTED = 2,
            LIST_SELECTED_BACK = 3,
            SELECTION_TEXT = 10,
            SELECTION_BACK = 11,
            SELECTION_ADDITIONAL_TEXT = 12,
            SELECTION_ADDITIONAL_BACK = 13,
            SELECTION_SECONDARY_TEXT = 14,
            SELECTION_SECONDARY_BACK = 15,
            SELECTION_INACTIVE_TEXT = 16,
            SELECTION_INACTIVE_BACK = 17,
            CARET = 40,
            CARET_ADDITIONAL = 41,
            CARET_LINE_BACK = 50,
            WHITE_SPACE = 60,
            WHITE_SPACE_BACK = 61,
            HOT_SPOT_ACTIVE = 70,
            HOT_SPOT_ACTIVE_BACK = 71,
            FOLD_LINE = 80,
            HIDDEN_LINE = 81
        }
        /// <summary>Set the selection to have its end of line filled or not. (Scintilla feature SC_LAYER_)</summary>
        public enum Layer
        {
            BASE = 0,
            UNDER_TEXT = 1,
            OVER_TEXT = 2
        }
        /// <summary>Indicator style enumeration and some constants (Scintilla feature INDIC_)</summary>
        public enum IndicatorStyle
        {
            PLAIN = 0,
            SQUIGGLE = 1,
            TT = 2,
            DIAGONAL = 3,
            STRIKE = 4,
            HIDDEN = 5,
            BOX = 6,
            ROUNDBOX = 7,
            STRAIGHTBOX = 8,
            DASH = 9,
            DOTS = 10,
            SQUIGGLELOW = 11,
            DOTBOX = 12,
            SQUIGGLEPIXMAP = 13,
            COMPOSITIONTHICK = 14,
            COMPOSITIONTHIN = 15,
            FULLBOX = 16,
            TEXTFORE = 17,
            POINT = 18,
            POINTCHARACTER = 19,
            GRADIENT = 20,
            GRADIENTCENTRE = 21,
            POINT_TOP = 22,
            CONTAINER = 8,
            IME = 32,
            IME_MAX = 35,
            MAX = 35
        }
        /// <summary>
        /// INDIC_CONTAINER, INDIC_IME, INDIC_IME_MAX, and INDIC_MAX are indicator numbers,
        /// not IndicatorStyles so should not really be in the INDIC_ enumeration.
        /// They are redeclared in IndicatorNumbers INDICATOR_.
        /// (Scintilla feature INDICATOR_)
        /// </summary>
        public enum IndicatorNumbers
        {
            CONTAINER = 8,
            IME = 32,
            IME_MAX = 35,
            HISTORY_REVERTED_TO_ORIGIN_INSERTION = 36,
            HISTORY_REVERTED_TO_ORIGIN_DELETION = 37,
            HISTORY_SAVED_INSERTION = 38,
            HISTORY_SAVED_DELETION = 39,
            HISTORY_MODIFIED_INSERTION = 40,
            HISTORY_MODIFIED_DELETION = 41,
            HISTORY_REVERTED_TO_MODIFIED_INSERTION = 42,
            HISTORY_REVERTED_TO_MODIFIED_DELETION = 43,
            MAX = 43
        }
        /// <summary>Retrieve the foreground hover colour of an indicator. (Scintilla feature SC_INDICVALUE)</summary>
        public enum IndicValue
        {
            BIT = 0x1000000,
            MASK = 0xFFFFFF
        }
        /// <summary>Retrieve the foreground hover colour of an indicator. (Scintilla feature SC_INDICFLAG_)</summary>
        public enum IndicFlag
        {
            NONE = 0,
            VALUEFORE = 1
        }
        /// <summary>Define option flags for autocompletion lists (Scintilla feature SC_AUTOCOMPLETE_)</summary>
        public enum AutoCompleteOption
        {
            NORMAL = 0,
            FIXED_SIZE = 1
        }
        /// <summary>Is the horizontal scroll bar visible? (Scintilla feature SC_IV_)</summary>
        public enum IndentView
        {
            NONE = 0,
            REAL = 1,
            LOOKFORWARD = 2,
            LOOKBOTH = 3
        }
        /// <summary>Returns the print magnification. (Scintilla feature SC_PRINT_)</summary>
        public enum PrintOption
        {
            NORMAL = 0,
            INVERTLIGHT = 1,
            BLACKONWHITE = 2,
            COLOURONWHITE = 3,
            COLOURONWHITEDEFAULTBG = 4,
            SCREENCOLOURS = 5
        }
        /// <summary>Returns the print colour mode. (Scintilla feature SCFIND_)</summary>
        public enum FindOption
        {
            NONE = 0x0,
            WHOLEWORD = 0x2,
            MATCHCASE = 0x4,
            WORDSTART = 0x00100000,
            REGEXP = 0x00200000,
            POSIX = 0x00400000,
            CXX11REGEX = 0x00800000
        }
        /// <summary>Draw the document into a display context such as a printer. (Scintilla feature SC_CHANGE_HISTORY_)</summary>
        public enum ChangeHistoryOption
        {
            DISABLED = 0,
            ENABLED = 1,
            MARKERS = 2,
            INDICATORS = 4
        }
        /// <summary>The number of display lines needed to wrap a document line (Scintilla feature SC_FOLDLEVEL)</summary>
        public enum FoldLevel
        {
            NONE = 0x0,
            BASE = 0x400,
            WHITEFLAG = 0x1000,
            HEADERFLAG = 0x2000,
            NUMBERMASK = 0x0FFF
        }
        /// <summary>Switch a header line between expanded and contracted and show some text after the line. (Scintilla feature SC_FOLDDISPLAYTEXT_)</summary>
        public enum FoldDisplayTextStyle
        {
            HIDDEN = 0,
            STANDARD = 1,
            BOXED = 2
        }
        /// <summary>Get the default fold display text. (Scintilla feature SC_FOLDACTION_)</summary>
        public enum FoldAction
        {
            CONTRACT = 0,
            EXPAND = 1,
            TOGGLE = 2,
            CONTRACT_EVERY_LEVEL = 4
        }
        /// <summary>Ensure a particular line is visible by expanding any header line hiding it. (Scintilla feature SC_AUTOMATICFOLD_)</summary>
        public enum AutomaticFold
        {
            NONE = 0x0000,
            SHOW = 0x0001,
            CLICK = 0x0002,
            CHANGE = 0x0004
        }
        /// <summary>Get automatic folding behaviours. (Scintilla feature SC_FOLDFLAG_)</summary>
        public enum FoldFlag
        {
            NONE = 0x0000,
            LINEBEFORE_EXPANDED = 0x0002,
            LINEBEFORE_CONTRACTED = 0x0004,
            LINEAFTER_EXPANDED = 0x0008,
            LINEAFTER_CONTRACTED = 0x0010,
            LEVELNUMBERS = 0x0040,
            LINESTATE = 0x0080
        }
        /// <summary>Is the range start..end considered a word? (Scintilla feature SC_IDLESTYLING_)</summary>
        public enum IdleStyling
        {
            NONE = 0,
            TOVISIBLE = 1,
            AFTERVISIBLE = 2,
            ALL = 3
        }
        /// <summary>Retrieve the limits to idle styling. (Scintilla feature SC_WRAP_)</summary>
        public enum Wrap
        {
            NONE = 0,
            WORD = 1,
            CHAR = 2,
            WHITESPACE = 3
        }
        /// <summary>Retrieve whether text is word wrapped. (Scintilla feature SC_WRAPVISUALFLAG_)</summary>
        public enum WrapVisualFlag
        {
            NONE = 0x0000,
            END = 0x0001,
            START = 0x0002,
            MARGIN = 0x0004
        }
        /// <summary>Retrive the display mode of visual flags for wrapped lines. (Scintilla feature SC_WRAPVISUALFLAGLOC_)</summary>
        public enum WrapVisualLocation
        {
            DEFAULT = 0x0000,
            END_BY_TEXT = 0x0001,
            START_BY_TEXT = 0x0002
        }
        /// <summary>Retrive the start indent for wrapped lines. (Scintilla feature SC_WRAPINDENT_)</summary>
        public enum WrapIndentMode
        {
            FIXED = 0,
            SAME = 1,
            INDENT = 2,
            DEEPINDENT = 3
        }
        /// <summary>Retrieve how wrapped sublines are placed. Default is fixed. (Scintilla feature SC_CACHE_)</summary>
        public enum LineCache
        {
            NONE = 0,
            CARET = 1,
            PAGE = 2,
            DOCUMENT = 3
        }
        /// <summary>Append a string to the end of the document without changing the selection. (Scintilla feature SC_PHASES_)</summary>
        public enum PhasesDraw
        {
            ONE = 0,
            TWO = 1,
            MULTIPLE = 2
        }
        /// <summary>Control font anti-aliasing. (Scintilla feature SC_EFF_)</summary>
        public enum FontQuality
        {
            QUALITY_MASK = 0xF,
            QUALITY_DEFAULT = 0,
            QUALITY_NON_ANTIALIASED = 1,
            QUALITY_ANTIALIASED = 2,
            QUALITY_LCD_OPTIMIZED = 3
        }
        /// <summary>Scroll so that a display line is at the top of the display. (Scintilla feature SC_MULTIPASTE_)</summary>
        public enum MultiPaste
        {
            ONCE = 0,
            EACH = 1
        }
        /// <summary>Set the other colour used as a chequerboard pattern in the fold margin (Scintilla feature SC_ACCESSIBILITY_)</summary>
        public enum Accessibility
        {
            DISABLED = 0,
            ENABLED = 1
        }
        /// <summary>Set which document modification events are sent to the container. (Scintilla feature EDGE_)</summary>
        public enum EdgeVisualStyle
        {
            NONE = 0,
            LINE = 1,
            BACKGROUND = 2,
            MULTILINE = 3
        }
        /// <summary>Retrieves the number of lines completely visible. (Scintilla feature SC_POPUP_)</summary>
        public enum PopUp
        {
            NEVER = 0,
            ALL = 1,
            TEXT = 2
        }
        /// <summary>Retrieve the zoom level. (Scintilla feature SC_DOCUMENTOPTION_)</summary>
        public enum DocumentOption
        {
            DEFAULT = 0,
            STYLES_NONE = 0x1,
            TEXT_LARGE = 0x100
        }
        /// <summary>Get internal focus flag. (Scintilla feature SC_STATUS_)</summary>
        public enum Status
        {
            OK = 0,
            FAILURE = 1,
            BADALLOC = 2,
            WARN_START = 1000,
            WARN_REGEX = 1001
        }
        /// <summary>Constants for use with SetVisiblePolicy, similar to SetCaretPolicy. (Scintilla feature VISIBLE_)</summary>
        public enum VisiblePolicy
        {
            SLOP = 0x01,
            STRICT = 0x04
        }
        /// <summary>Set the focus to this Scintilla widget. (Scintilla feature CARET_)</summary>
        public enum CaretPolicy
        {
            SLOP = 0x01,
            STRICT = 0x04,
            JUMPS = 0x10,
            EVEN = 0x08
        }
        /// <summary>Copy argument text to the clipboard. (Scintilla feature SC_SEL_)</summary>
        public enum SelectionMode
        {
            STREAM = 0,
            RECTANGLE = 1,
            LINES = 2,
            THIN = 3
        }
        /// <summary>
        /// Get currently selected item text in the auto-completion list
        /// Returns the length of the item text
        /// Result is NUL-terminated.
        /// (Scintilla feature SC_CASEINSENSITIVEBEHAVIOUR_)
        /// </summary>
        public enum CaseInsensitiveBehaviour
        {
            RESPECTCASE = 0,
            IGNORECASE = 1
        }
        /// <summary>Get auto-completion case insensitive behaviour. (Scintilla feature SC_MULTIAUTOC_)</summary>
        public enum MultiAutoComplete
        {
            ONCE = 0,
            EACH = 1
        }
        /// <summary>Retrieve the effect of autocompleting when there are multiple selections. (Scintilla feature SC_ORDER_)</summary>
        public enum Ordering
        {
            PRESORTED = 0,
            PERFORMSORT = 1,
            CUSTOM = 2
        }
        /// <summary>
        /// Find the position of a column on a line taking into account tabs and
        /// multi-byte characters. If beyond end of line, return line end position.
        /// (Scintilla feature SC_CARETSTICKY_)
        /// </summary>
        public enum CaretSticky
        {
            OFF = 0,
            ON = 1,
            WHITESPACE = 2
        }
        /// <summary>Get the background alpha of the caret line. (Scintilla feature CARETSTYLE_)</summary>
        public enum CaretStyle
        {
            INVISIBLE = 0,
            LINE = 1,
            BLOCK = 2,
            OVERSTRIKE_BAR = 0,
            OVERSTRIKE_BLOCK = 0x10,
            CURSES = 0x20,
            INS_MASK = 0xF,
            BLOCK_AFTER = 0x100
        }
        /// <summary>Get the start of the range of style numbers used for margin text (Scintilla feature SC_MARGINOPTION_)</summary>
        public enum MarginOption
        {
            NONE = 0,
            SUBLINESELECT = 1
        }
        /// <summary>Clear the annotations from all lines (Scintilla feature ANNOTATION_)</summary>
        public enum AnnotationVisible
        {
            HIDDEN = 0,
            STANDARD = 1,
            BOXED = 2,
            INDENTED = 3
        }
        /// <summary>Allocate some extended (>255) style numbers and return the start of the range (Scintilla feature UNDO_)</summary>
        public enum UndoFlags
        {
            NONE = 0,
            MAY_COALESCE = 1
        }
        /// <summary>Return the virtual space of the anchor of the rectangular selection. (Scintilla feature SCVS_)</summary>
        public enum VirtualSpace
        {
            NONE = 0,
            RECTANGULARSELECTION = 1,
            USERACCESSIBLE = 2,
            NOWRAPLINESTART = 4
        }
        /// <summary>Scroll to end of document. (Scintilla feature SC_TECHNOLOGY_)</summary>
        public enum Technology
        {
            DEFAULT = 0,
            DIRECTWRITE = 1,
            DIRECTWRITERETAIN = 2,
            DIRECTWRITEDC = 3
        }
        /// <summary>
        /// Line end types which may be used in addition to LF, CR, and CRLF
        /// SC_LINE_END_TYPE_UNICODE includes U+2028 Line Separator,
        /// U+2029 Paragraph Separator, and U+0085 Next Line
        /// (Scintilla feature SC_LINE_END_TYPE_)
        /// </summary>
        public enum LineEndType
        {
            DEFAULT = 0,
            UNICODE = 1
        }
        /// <summary>Can draw representations in various ways (Scintilla feature SC_REPRESENTATION)</summary>
        public enum RepresentationAppearance
        {
            _PLAIN = 0,
            _BLOB = 1,
            _COLOUR = 0x10
        }
        /// <summary>Clear the end of annotations from all lines (Scintilla feature EOLANNOTATION_)</summary>
        public enum EOLAnnotationVisible
        {
            HIDDEN = 0x0,
            STANDARD = 0x1,
            BOXED = 0x2,
            STADIUM = 0x100,
            FLAT_CIRCLE = 0x101,
            ANGLE_CIRCLE = 0x102,
            CIRCLE_FLAT = 0x110,
            FLATS = 0x111,
            ANGLE_FLAT = 0x112,
            CIRCLE_ANGLE = 0x120,
            FLAT_ANGLE = 0x121,
            ANGLES = 0x122
        }
        /// <summary>Get the start of the range of style numbers used for end of line annotations (Scintilla feature SC_SUPPORTS_)</summary>
        public enum Supports
        {
            LINE_DRAWS_FINAL = 0,
            PIXEL_DIVISIONS = 1,
            FRACTIONAL_STROKE_WIDTH = 2,
            TRANSLUCENT_STROKE = 3,
            PIXEL_MODIFICATION = 4,
            THREAD_SAFE_MEASURE_WIDTHS = 5
        }
        /// <summary>Get whether a feature is supported (Scintilla feature SC_LINECHARACTERINDEX_)</summary>
        public enum LineCharacterIndexType
        {
            NONE = 0,
            UTF32 = 1,
            UTF16 = 2
        }
        /// <summary>
        /// Retrieve a '\n' separated list of properties understood by the current lexer.
        /// Result is NUL-terminated.
        /// (Scintilla feature SC_TYPE_)
        /// </summary>
        public enum TypeProperty
        {
            BOOLEAN = 0,
            INTEGER = 1,
            STRING = 2
        }
        /// <summary>
        /// Notifications
        /// Type of modification and the action which caused the modification.
        /// These are defined as a bit mask to make it easy to specify which notifications are wanted.
        /// One bit is set from each of SC_MOD_* and SC_PERFORMED_*.
        /// (Scintilla feature SC_MOD_ SC_PERFORMED_ SC_MULTISTEPUNDOREDO SC_LASTSTEPINUNDOREDO SC_MULTILINEUNDOREDO SC_STARTACTION SC_MODEVENTMASKALL)
        /// </summary>
        public enum ModificationFlags
        {
        }
        /// <summary>
        /// Notifications
        /// Type of modification and the action which caused the modification.
        /// These are defined as a bit mask to make it easy to specify which notifications are wanted.
        /// One bit is set from each of SC_MOD_* and SC_PERFORMED_*.
        /// (Scintilla feature SC_UPDATE_)
        /// </summary>
        public enum Update
        {
            NONE = 0x0,
            CONTENT = 0x1,
            SELECTION = 0x2,
            V_SCROLL = 0x4,
            H_SCROLL = 0x8
        }
        /// <summary>
        /// For compatibility, these go through the COMMAND notification rather than NOTIFY
        /// and should have had exactly the same values as the EN_* constants.
        /// Unfortunately the SETFOCUS and KILLFOCUS are flipped over from EN_*
        /// As clients depend on these constants, this will not be changed.
        /// (Scintilla feature SCEN_)
        /// </summary>
        public enum FocusChange
        {
            CHANGE = 768,
            SETFOCUS = 512,
            KILLFOCUS = 256
        }
        /// <summary>
        /// Symbolic key codes and modifier flags.
        /// ASCII and other printable characters below 256.
        /// Extended keys above 300.
        /// (Scintilla feature SCMOD_)
        /// </summary>
        public enum KeyMod
        {
            NORM = 0,
            SHIFT = 1,
            CTRL = 2,
            ALT = 4,
            SUPER = 8,
            META = 16
        }
        /// <summary>
        /// Symbolic key codes and modifier flags.
        /// ASCII and other printable characters below 256.
        /// Extended keys above 300.
        /// (Scintilla feature SC_AC_)
        /// </summary>
        public enum CompletionMethods
        {
            FILLUP = 1,
            DOUBLECLICK = 2,
            TAB = 3,
            NEWLINE = 4,
            COMMAND = 5,
            SINGLE_CHOICE = 6
        }
        /// <summary>characterSource for SCN_CHARADDED (Scintilla feature SC_CHARACTERSOURCE_)</summary>
        public enum CharacterSource
        {
            DIRECT_INPUT = 0,
            TENTATIVE_INPUT = 1,
            IME_RESULT = 2
        }
        /// <summary>For SciLexer.h (Scintilla feature SCLEX_)</summary>
        public enum Lexer
        {
            CONTAINER = 0,
            NULL = 1,
            PYTHON = 2,
            CPP = 3,
            HTML = 4,
            XML = 5,
            PERL = 6,
            SQL = 7,
            VB = 8,
            PROPERTIES = 9,
            ERRORLIST = 10,
            MAKEFILE = 11,
            BATCH = 12,
            XCODE = 13,
            LATEX = 14,
            LUA = 15,
            DIFF = 16,
            CONF = 17,
            PASCAL = 18,
            AVE = 19,
            ADA = 20,
            LISP = 21,
            RUBY = 22,
            EIFFEL = 23,
            EIFFELKW = 24,
            TCL = 25,
            NNCRONTAB = 26,
            BULLANT = 27,
            VBSCRIPT = 28,
            BAAN = 31,
            MATLAB = 32,
            SCRIPTOL = 33,
            ASM = 34,
            CPPNOCASE = 35,
            FORTRAN = 36,
            F77 = 37,
            CSS = 38,
            POV = 39,
            LOUT = 40,
            ESCRIPT = 41,
            PS = 42,
            NSIS = 43,
            MMIXAL = 44,
            CLW = 45,
            CLWNOCASE = 46,
            LOT = 47,
            YAML = 48,
            TEX = 49,
            METAPOST = 50,
            POWERBASIC = 51,
            FORTH = 52,
            ERLANG = 53,
            OCTAVE = 54,
            MSSQL = 55,
            VERILOG = 56,
            KIX = 57,
            GUI4CLI = 58,
            SPECMAN = 59,
            AU3 = 60,
            APDL = 61,
            BASH = 62,
            ASN1 = 63,
            VHDL = 64,
            CAML = 65,
            BLITZBASIC = 66,
            PUREBASIC = 67,
            HASKELL = 68,
            PHPSCRIPT = 69,
            TADS3 = 70,
            REBOL = 71,
            SMALLTALK = 72,
            FLAGSHIP = 73,
            CSOUND = 74,
            FREEBASIC = 75,
            INNOSETUP = 76,
            OPAL = 77,
            SPICE = 78,
            D = 79,
            CMAKE = 80,
            GAP = 81,
            PLM = 82,
            PROGRESS = 83,
            ABAQUS = 84,
            ASYMPTOTE = 85,
            R = 86,
            MAGIK = 87,
            POWERSHELL = 88,
            MYSQL = 89,
            PO = 90,
            TAL = 91,
            COBOL = 92,
            TACL = 93,
            SORCUS = 94,
            POWERPRO = 95,
            NIMROD = 96,
            SML = 97,
            MARKDOWN = 98,
            TXT2TAGS = 99,
            A68K = 100,
            MODULA = 101,
            COFFEESCRIPT = 102,
            TCMD = 103,
            AVS = 104,
            ECL = 105,
            OSCRIPT = 106,
            VISUALPROLOG = 107,
            LITERATEHASKELL = 108,
            STTXT = 109,
            KVIRC = 110,
            RUST = 111,
            DMAP = 112,
            AS = 113,
            DMIS = 114,
            REGISTRY = 115,
            BIBTEX = 116,
            SREC = 117,
            IHEX = 118,
            TEHEX = 119,
            JSON = 120,
            EDIFACT = 121,
            INDENT = 122,
            MAXIMA = 123,
            STATA = 124,
            SAS = 125,
            NIM = 126,
            CIL = 127,
            X12 = 128,
            DATAFLEX = 129,
            AUTOMATIC = 1000
        }
        /// <summary>GTK Specific to work around focus and accelerator problems: (Scintilla feature SC_BIDIRECTIONAL_)</summary>
        public enum Bidirectional
        {
            DISABLED = 0,
            L2R = 1,
            R2L = 2
        }
    /* --Autogenerated -- end of section automatically generated from Scintilla.iface */

}
