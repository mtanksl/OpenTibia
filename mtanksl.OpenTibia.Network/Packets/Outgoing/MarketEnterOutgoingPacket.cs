using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class MarketEnterOutgoingPacket : IOutgoingPacket
    {
        public MarketEnterOutgoingPacket()
        {

        }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xF6 );

            //TODO
        }
    }
}