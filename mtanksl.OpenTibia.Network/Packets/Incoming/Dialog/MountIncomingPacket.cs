using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class MountIncomingPacket : IIncomingPacket
    {
        public bool IsMounted { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            IsMounted = reader.ReadBool();
        }
    }
}