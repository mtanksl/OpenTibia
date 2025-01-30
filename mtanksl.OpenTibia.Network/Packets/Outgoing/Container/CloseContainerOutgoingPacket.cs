using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseContainerOutgoingPacket : IOutgoingPacket
    {
        public CloseContainerOutgoingPacket(byte containerId)
        {
            this.ContainerId = containerId;
        }

        public byte ContainerId { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6F );

            writer.Write(ContainerId);
        }
    }
}