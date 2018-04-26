using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseChannel : IOutgoingPacket
    {
        public CloseChannel(ushort channelId)
        {
            this.ChannelId = channelId;
        }

        public ushort ChannelId { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB3 );

            writer.Write(ChannelId);
        }
    }
}