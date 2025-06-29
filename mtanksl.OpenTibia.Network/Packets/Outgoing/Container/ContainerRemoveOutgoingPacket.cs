using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ContainerRemoveOutgoingPacket : IOutgoingPacket
    {
        public ContainerRemoveOutgoingPacket(byte containerId, ushort index)
        {
            this.ContainerId = containerId;

            this.Index = index;
        }

        public byte ContainerId { get; set; }

        public ushort Index { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x72 );

            writer.Write(ContainerId);

            if ( !features.HasFeatureFlag(FeatureFlag.ContainerPagination) )
            {
                writer.Write( (byte)Index);
            }
            else
            {
                writer.Write( (ushort)Index);

                writer.Write( (ushort)0); //TODO: FeatureFlag.ContainerPagination, last item
            }
        }
    }
}