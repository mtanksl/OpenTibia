using OpenTibia.IO;

namespace OpenTibia
{
    public class OpenRuleViolationsChannelOutgoingPacket : IOutgoingPacket
    {
        public OpenRuleViolationsChannelOutgoingPacket(ushort channelId)
        {
            this.ChannelId = channelId;
        }

        public ushort ChannelId { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xAE );

            writer.Write(ChannelId);
        }
    }
}