using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public interface IOutgoingPacket
    {
        void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features);
    }
}