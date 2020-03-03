using System.Collections.Generic;
using System.IO;
using System.Text;

namespace util.BRLight
{
    public class EncodingDetector
    {
        /// <summary>
        /// Helper class to store information about encodings
        /// with a preamble
        /// </summary>
        protected class PreambleInfo
        {
            protected Encoding _encoding;
            protected byte[] _preamble;

            /// <summary>
            /// Property Encoding (Encoding).
            /// </summary>
            public Encoding Encoding
            {
                get { return _encoding; }
            }

            /// <summary>
            /// Property Preamble (byte[]).
            /// </summary>
            public byte[] Preamble
            {
                get { return _preamble; }
            }

            /// <summary>
            /// Constructor with preamble and encoding
            /// </summary>
            /// <param name="encoding"></param>
            /// <param name="preamble"></param>
            public PreambleInfo(Encoding encoding, byte[] preamble)
            {
                _encoding = encoding;
                _preamble = preamble;
            }
        }

        // The list of encodings with a preamble,
        // sorted longest preamble first.
        protected static SortedList<int, PreambleInfo> _preambles;

        // Maximum length of all preamles
        protected static int _maxPreambleLength;

        public static string ReadAllText(string filename, Encoding defaultEncoding, out Encoding usedEncoding)
        {
            // Read the contents of the file as an array of bytes
            byte[] bytes = File.ReadAllBytes(filename);

            // Detect the encoding of the file:
            usedEncoding = DetectEncoding(bytes);

            // If none found, use the default encoding.
            // Otherwise, determine the length of the encoding markers in the file
            int offset;
            if (usedEncoding == null)
            {
                offset = 0;
                usedEncoding = defaultEncoding;
            }
            else
            {
                offset = usedEncoding.GetPreamble().Length;
            }

            // Now interpret the bytes according to the encoding,
            // skipping the preample (if any)
            return usedEncoding.GetString(bytes, offset, bytes.Length - offset);
        }

        /// <summary>
        /// Detect the encoding in an array of bytes.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>The encoding found, or null</returns>
        public static Encoding DetectEncoding(byte[] bytes)
        {
            // Scan for encodings if we haven't done so
            if (_preambles == null)
                ScanEncodings();

            // Try each preamble in turn
            foreach (PreambleInfo info in _preambles.Values)
            {
                // Match all bytes in the preamble
                bool match = true;

                if (bytes.Length >= info.Preamble.Length)
                {
                    for (var i = 0; i < info.Preamble.Length; i++)
                    {
                        if (bytes[i] != info.Preamble[i])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        return info.Encoding;
                    }
                }
            }

            return null;
        }

        public static Encoding GetFileEncoding(string pathArquivo)
        {
            var enc = Encoding.Default;
            var buffer = new byte[5];
            var file = new FileStream(pathArquivo, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();
            file.Dispose();
            file = null;
            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;
        }

        /// <summary>
        /// Detect the encoding of a file. Reads just enough of
        /// the file to be able to detect a preamble.
        /// </summary>
        /// <param name="filename">The path name of the file</param>
        /// <returns>The encoding detected, or null if no preamble found</returns>
        public static Encoding DetectEncoding(string filename)
        {
            // Scan for encodings if we haven't done so
            if (_preambles == null)
                ScanEncodings();

            using (FileStream stream = File.OpenRead(filename))
            {
                // Never read more than the length of the file
                // or the maximum preamble length
                long n = stream.Length;

                // No bytes? No encoding!
                if (n == 0)
                    return null;

                // Read the minimum amount necessary
                if (n > _maxPreambleLength)
                    n = _maxPreambleLength;

                var bytes = new byte[n];

                stream.Read(bytes, 0, (int)n);
                stream.Close();
                stream.Dispose();
                // Detect the encoding from the byte array
                return DetectEncoding(bytes);
            }
        }

        /// <summary>
        /// Loop over all available encodings and store those
        /// with a preamble in the _preambles list.
        /// The list is sorted by preamble length,
        /// longest preamble first. This prevents
        /// a short preamble 'masking' a longer one
        /// later in the list.
        /// </summary>
        protected static void ScanEncodings()
        {
            // Create a new sorted list of preambles
            _preambles = new SortedList<int, PreambleInfo>();

            // Loop over all encodings
            foreach (EncodingInfo encodingInfo in Encoding.GetEncodings())
            {
                // Do we have a preamble?
                byte[] preamble = encodingInfo.GetEncoding().GetPreamble();
                if (preamble.Length > 0)
                {
                    // Add it to the collection, inversely sorted by preamble length
                    // (and code page, to keep the keys unique)
                    _preambles.Add(-(preamble.Length * 1000000 + encodingInfo.CodePage),
                                   new PreambleInfo(encodingInfo.GetEncoding(), preamble));

                    // Update the maximum preamble length if this one's longer
                    if (preamble.Length > _maxPreambleLength)
                    {
                        _maxPreambleLength = preamble.Length;
                    }
                }
            }
        }
    }
}
