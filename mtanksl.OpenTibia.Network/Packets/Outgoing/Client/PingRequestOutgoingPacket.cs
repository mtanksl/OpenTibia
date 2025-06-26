using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class PingRequestOutgoingPacket : IOutgoingPacket
    {
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x1D );
        }
    }
}