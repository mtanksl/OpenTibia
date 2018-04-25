using OpenTibia.IO;

namespace OpenTibia
{
    public class OpenChannelOutgoingPacket : IOutgoingPacket
    {
        public OpenChannelOutgoingPacket(ushort channelId, string name)
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