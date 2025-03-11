using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendUseItemCooldownOutgoingPacket : IOutgoingPacket
    {
        public SendUseItemCooldownOutgoingPacket(uint time)
        {
            this.Time = time;
        }

        public uint Time { get; set; }
                
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA6);

            writer.Write(Time);
        }
    }
}