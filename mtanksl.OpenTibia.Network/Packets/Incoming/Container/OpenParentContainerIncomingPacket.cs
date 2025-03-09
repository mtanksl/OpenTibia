using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenParentContainerIncomingPacket : IIncomingPacket
    {
        public byte ContainerId { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            ContainerId = reader.ReadByte();
        }
    }
}