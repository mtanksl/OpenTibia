using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ContainerUpdateOutgoingPacket : IOutgoingPacket
    {
        public ContainerUpdateOutgoingPacket(byte containerId, byte index, Item item)
        {
            this.ContainerId = containerId;

            this.Index = index;

            this.Item = item;
        }

        public byte ContainerId { get; set; }

        public byte Index { get; set; }

        public Item Item { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x71 );

            writer.Write(ContainerId);

            writer.Write(Index);

            writer.Write(features, Item);
        }
    }
}