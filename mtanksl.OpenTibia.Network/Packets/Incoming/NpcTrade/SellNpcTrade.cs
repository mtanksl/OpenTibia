using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SellNpcTrade : IIncomingPacket
    {
        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public byte Count { get; set; }

        public bool KeepEquipped { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            ItemId = reader.ReadUShort();

            Type = reader.ReadByte();

            Count = reader.ReadByte();

            KeepEquipped = reader.ReadBool();
        }
    }
}