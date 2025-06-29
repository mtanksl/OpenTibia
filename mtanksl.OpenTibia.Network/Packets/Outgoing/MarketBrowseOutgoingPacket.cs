using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class MarketBrowseOutgoingPacket : IOutgoingPacket
    {
        public MarketBrowseOutgoingPacket()
        {

        }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xF9 );

            //TODO
        }
    }
}