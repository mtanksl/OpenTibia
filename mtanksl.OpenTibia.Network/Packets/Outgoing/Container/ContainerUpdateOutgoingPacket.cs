using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ContainerUpdateOutgoingPacket : IOutgoingPacket
    {
        public ContainerUpdateOutgoingPacket(byte containerId, ushort index, Item item)
        {
            this.ContainerId = containerId;

            this.Index = index;

            this.Item = item;
        }

        public byte ContainerId { get; set; }

        public ushort Index { get; set; }

        public Item Item { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x71 );

            writer.Write(ContainerId);

            if (features.HasFeatureFlag(FeatureFlag.ContainerPagination) )
            {
                writer.Write( (ushort)Index);
            }
            else
            {
                writer.Write( (byte)Index);
            }

            writer.Write(features, Item);
        }
    }
}