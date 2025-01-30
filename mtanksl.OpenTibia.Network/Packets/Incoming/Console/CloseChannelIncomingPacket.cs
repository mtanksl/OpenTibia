using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseChannelIncomingPacket : IIncomingPacket
    {
        public ushort ChannelId { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            ChannelId = reader.ReadUShort();
        }
    }
}