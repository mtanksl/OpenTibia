using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class BuyNpcTradeIncomingPacket : IIncomingPacket
    {
        public ushort TibiaId { get; set; }

        public byte Type { get; set; }

        public byte Count { get; set; }

        public bool IgnoreCapacity { get; set; }

        public bool BuyWithBackpacks { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            TibiaId = reader.ReadUShort();

            Type = reader.ReadByte();

            Count = reader.ReadByte();

            IgnoreCapacity = reader.ReadBool();

            BuyWithBackpacks = reader.ReadBool();
        }
    }
}