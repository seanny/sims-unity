using System.IO;

namespace SimsLib.IFF
{
    public class FWAV : AbstractIffChunk
    {
        public string Name { get; private set; }

        public override void Read(Iff iff, Stream stream)
        {
            using (var io = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN))
            {
                Name = io.ReadNullTerminatedString();
            }
        }
    }
}