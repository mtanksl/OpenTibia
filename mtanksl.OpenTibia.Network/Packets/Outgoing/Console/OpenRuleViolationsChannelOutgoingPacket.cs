using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenRuleViolationsChannelOutgoingPacket : IOutgoingPacket
    {
        public OpenRuleViolationsChannelOutgoingPacket(ushort channelId)
        {
            this.ChannelId = channelId;
        }

        public ushort ChannelId { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xAE );

            writer.Write(ChannelId);
        }
    }
}