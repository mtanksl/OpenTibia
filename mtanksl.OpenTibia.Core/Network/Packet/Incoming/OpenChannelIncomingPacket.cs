using OpenTibia.IO;

namespace OpenTibia
{
    public class OpenChannelIncomingPacket : IIncomingPacket
    {
        public ushort ChannelId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ChannelId = reader.ReadUShort();
        }
    }
}