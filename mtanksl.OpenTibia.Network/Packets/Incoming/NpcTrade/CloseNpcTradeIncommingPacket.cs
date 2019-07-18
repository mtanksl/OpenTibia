using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseNpcTradeIncommingPacket : IIncomingPacket
    {
        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public byte Count { get; set; }

        public bool IgnoreCapacity { get; set; }

        public bool BuyWithBackpacks { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            ItemId = reader.ReadUShort();

            Type = reader.ReadByte();

            Count = reader.ReadByte();

            IgnoreCapacity = reader.ReadBool();

            BuyWithBackpacks = reader.ReadBool();
        }
    }
}