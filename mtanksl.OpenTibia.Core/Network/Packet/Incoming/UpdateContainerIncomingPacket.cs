using OpenTibia.IO;

namespace OpenTibia
{
    public class UpdateContainerIncomingPacket : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            ContainerId = reader.ReadByte();
        }
    }
}