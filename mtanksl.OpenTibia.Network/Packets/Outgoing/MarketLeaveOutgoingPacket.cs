using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class MarketLeaveOutgoingPacket : IOutgoingPacket
    {
        public MarketLeaveOutgoingPacket()
        {

        }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xF7 );

            //TODO
        }
    }
}