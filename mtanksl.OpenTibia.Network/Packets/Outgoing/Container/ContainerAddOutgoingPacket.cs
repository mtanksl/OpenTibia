using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x70 );

            writer.Write(ContainerId);

            if (features.HasFeatureFlag(FeatureFlag.ContainerPagination) )
            {
                writer.Write( (ushort)0x00); //TODO: FeatureFlag.ContainerPagination, index
            }

            writer.Write(features, Item);
        }
    }
}