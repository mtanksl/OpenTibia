using OpenTibia.IO;

namespace OpenTibia
{
    public class CloseChannelIncomingPacket : IIncomingPacket
    {
        public ushort ChannelId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ChannelId = reader.ReadUShort();
        }
    }
}