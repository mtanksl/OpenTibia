using OpenTibia.IO;

namespace OpenTibia
{
    public class CloseContainerOutgoingPacket : IOutgoingPacket
    {
        public CloseContainerOutgoingPacket(byte containerId)
        {
            this.ContainerId = containerId;
        }

        public byte ContainerId { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6F );

            writer.Write(ContainerId);
        }
    }
}