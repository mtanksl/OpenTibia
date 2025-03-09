using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendConnectionInfoOutgoingPacket : IOutgoingPacket
    {
        public SendConnectionInfoOutgoingPacket(uint timestamp, byte random)
        {
            this.Timestamp = timestamp;

            this.Random = random;
        }

        public uint Timestamp { get; set; }

        public byte Random { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x1F );

            writer.Write(Timestamp);

            writer.Write(Random);
        }
    }
}