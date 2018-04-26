using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseChannel : IIncomingPacket
    {
        public ushort ChannelId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ChannelId = reader.ReadUShort();
        }
    }
}