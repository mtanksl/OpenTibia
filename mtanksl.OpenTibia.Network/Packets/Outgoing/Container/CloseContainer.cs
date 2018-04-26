using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseContainer : IOutgoingPacket
    {
        public CloseContainer(byte containerId)
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