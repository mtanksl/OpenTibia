using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ContainerRemove : IOutgoingPacket
    {
        public ContainerRemove(byte containerId, byte index)
        {
            this.ContainerId = containerId;

            this.Index = index;
        }

        public byte ContainerId { get; set; }

        public byte Index { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x72 );

            writer.Write(ContainerId);

            writer.Write(Index);
        }
    }
}