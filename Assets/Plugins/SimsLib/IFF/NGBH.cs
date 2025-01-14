using System.Collections.Generic;
using System.IO;

namespace SimsLib.IFF
{
    public class NGBH : AbstractIffChunk
    {
        public short[] NeighborhoodData = new short[16];
        public Dictionary<short, List<InventoryItem>> InventoryByID = new Dictionary<short, List<InventoryItem>>();

        public uint Version;

        /// <summary>
        /// Reads a NGBH chunk from a stream.
        /// </summary>
        /// <param name="iff">An Iff instance.</param>
        /// <param name="stream">A Stream object holding a OBJf chunk.</param>
        public override void Read(Iff iff, Stream stream)
        {
            using (var io = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN))
            {
                io.ReadUInt32(); //pad
                Version = io.ReadUInt32(); //0x49 for latest game
                string magic = io.ReadChars(4); //HBGN

                for (int i = 0; i < 16; i++)
                {
                    NeighborhoodData[i] = io.ReadInt16();
                }

                if (!io.HasMore) return; //no inventory present (yet)
                var count = io.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    if (io.ReadInt32() != 1) { }
                    var neighID = io.ReadInt16();
                    var inventoryCount = io.ReadInt32();
                    var inventory = new List<InventoryItem>();

                    for (int j = 0; j < inventoryCount; j++)
                    {
                        inventory.Add(new InventoryItem(io));
                    }
                    InventoryByID[neighID] = inventory;
                }
            }
        }
    }

    public class InventoryItem
    {
        public int Type;
        public uint GUID;
        public ushort Count;

        public InventoryItem() { }

        public InventoryItem(IoBuffer io)
        {
            Type = io.ReadInt32();
            GUID = io.ReadUInt32();
            Count = io.ReadUInt16();
        }

        public InventoryItem Clone()
        {
            return new InventoryItem() { Type = Type, GUID = GUID, Count = Count };
        }

        public override string ToString()
        {
            return "Type: " + Type + ", GUID: " + GUID + ", Count: " + Count;
        }
    }
}