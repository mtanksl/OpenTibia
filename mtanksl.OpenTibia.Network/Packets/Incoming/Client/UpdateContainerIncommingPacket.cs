using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class UpdateContainerIncommingPacket : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ContainerId = reader.ReadByte();
        }
    }
}