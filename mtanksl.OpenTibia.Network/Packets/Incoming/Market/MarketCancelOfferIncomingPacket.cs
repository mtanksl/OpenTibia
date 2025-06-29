using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class MarketCancelOfferIncomingPacket : IIncomingPacket
    {
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            //TODO: FeatureFlag.PlayerMarket
        }
    }
}