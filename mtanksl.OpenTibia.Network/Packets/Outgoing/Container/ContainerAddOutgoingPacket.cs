using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ContainerAddOutgoingPacket : IOutgoingPacket
    {
        public ContainerAddOutgoingPacket(byte containerId, Item item)
        {
            this.ContainerId = containerId;

            this.Item = item;
        }

        public byte ContainerId { get; set; }

        public Item Item { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x70 );

            writer.Write(ContainerId);

            writer.Write(Item);
        }
    }
}