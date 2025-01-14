using System.Collections.Generic;
using System.Text;

namespace SimsLib.FAR3
{
    /// <summary>
    /// Represents an entry in a FAR3 archive.
    /// </summary>
    public class Far3Entry
    {
        public uint DecompressedFileSize;
        public uint CompressedFileSize;
        public byte DataType;
        public uint DataOffset;
        public byte Compressed;
        public byte AccessNumber;
        public ushort FilenameLength;
        public uint TypeID;
        public uint FileID;
        public string Filename;
    }
}