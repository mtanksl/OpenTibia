using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public interface IIncomingPacket
    {
        void Read(IByteArrayStreamReader reader, IHasFeatureFlag features);
    }
}