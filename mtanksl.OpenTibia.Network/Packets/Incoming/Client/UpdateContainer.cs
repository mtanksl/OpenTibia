using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class UpdateContainer : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ContainerId = reader.ReadByte();
        }
    }
}