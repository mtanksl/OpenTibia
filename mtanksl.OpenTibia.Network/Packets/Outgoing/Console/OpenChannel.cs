using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenChannel : IOutgoingPacket
    {
        public OpenChannel(ushort channelId, string name)
        {
            this.ChannelId = channelId;

            this.Name = name;
        }

        public ushort ChannelId { get; set; }

        public string Name { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xAC );

            writer.Write(ChannelId);

            writer.Write(Name);
        }
    }
}