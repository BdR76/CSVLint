namespace CsvQuery.PluginInfrastructure
{
    using System.Diagnostics;
    using System.Text;
    using System.IO;
    using CSVLint.Tools;
    using Kbg.NppPluginNET.PluginInfrastructure;

    public class ScintillaStreams
    {
        /// <summary>
        /// Reads the whole document as a text stream, trying to use the right encoding
        /// </summary>
        public static StreamReader StreamAllText()
        {
            var doc = PluginBase.CurrentScintillaGateway;
            var codepage = doc.GetCodePage();
            var encoding = codepage == (int)SciMsg.SC_CP_UTF8 ? Encoding.UTF8 : Encoding.Default;
            return new StreamReader(StreamAllRawText(), encoding);
        }

        /// <summary>
        /// Reads the whole document as a byte stream.
        /// Will likely throw exceptions if the document is edited while the stream is open.
        /// </summary>
        public static Stream StreamAllRawText()
        {
            var doc = PluginBase.CurrentScintillaGateway;
            var length = doc.GetLength();

            // When editing a document Scintilla divides it into two - one before the cursor and one after, calling the break point the "gap"
            int gap = doc.GetGapPosition();
            Debug.WriteLine($"ScintillaStreams StreamAllRawText: gap at {gap}/{length}");

            // If not currently editing there is no "gap", and the gap is the end of the document
            if (length == gap)
            {
                var characterPointer = doc.GetCharacterPointer();
                unsafe
                {
                    return new UnmanagedMemoryStream((byte*)characterPointer.ToPointer(), length, length, FileAccess.Read);
                }
            }

            // If there is a gap we create two streams - one for the first part and one for the second, and concatenate them
            return ConcatenatingStream.FromFactoryFunctions(false, () =>
            {
                var rangePtr = doc.GetRangePointer(0, gap);
                unsafe
                {
                    return new UnmanagedMemoryStream((byte*) rangePtr.ToPointer(), gap, gap, FileAccess.Read);
                }
            }, () =>
            {
                var rangePtr = doc.GetRangePointer(gap+1, length);
                unsafe
                {
                    return new UnmanagedMemoryStream((byte*)rangePtr.ToPointer(), length-gap, length-gap, FileAccess.Read);
                }
            });
        }
    }
}
