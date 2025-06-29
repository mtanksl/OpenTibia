using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class MarketDetailOutgoingPacket : IOutgoingPacket
    {
        public MarketDetailOutgoingPacket()
        {

        }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xF8 );

            //TODO
        }
    }
}