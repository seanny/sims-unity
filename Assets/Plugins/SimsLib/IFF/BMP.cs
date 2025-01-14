using System.IO;
using UnityEngine;

namespace SimsLib.IFF
{
    /// <summary>
    /// This chunk holds an image in a bitmap (.bmp) format
    /// </summary>
    public class BMP : AbstractIffChunk
    {
        /// <summary>
        /// Bitmap Image Data
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Reads a BMP chunk from the stream
        /// </summary>
        /// <param name="iff">IFF file instance</param>
        /// <param name="stream">Stream holding the BMP chunk</param>
        public override void Read(Iff iff, Stream stream)
        {
            Data = new byte[stream.Length];
            stream.Read(Data,0, (int)stream.Length);
        }
    }
}