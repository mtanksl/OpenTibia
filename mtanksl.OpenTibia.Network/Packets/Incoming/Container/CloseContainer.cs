using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CloseContainer : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ContainerId = reader.ReadByte();
        }
    }
}