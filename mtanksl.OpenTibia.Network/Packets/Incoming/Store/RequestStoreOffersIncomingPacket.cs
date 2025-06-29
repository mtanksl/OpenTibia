using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class RequestStoreOffersIncomingPacket : IIncomingPacket
    {
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            //TODO: FeatureFlag.IngameStore
        }
    }
}