using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseChannelOutgoingPacket : IOutgoingPacket
    {
        public CloseChannelOutgoingPacket(ushort channelId)
        {
            this.ChannelId = channelId;
        }

        public ushort ChannelId { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB3 );

            writer.Write(ChannelId);
        }
    }
}