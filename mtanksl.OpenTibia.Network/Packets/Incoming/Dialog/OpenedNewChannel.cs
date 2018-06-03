using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenedNewChannel : IIncomingPacket
    {
        public ushort ChannelId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ChannelId = reader.ReadUShort();
        }
    }
}