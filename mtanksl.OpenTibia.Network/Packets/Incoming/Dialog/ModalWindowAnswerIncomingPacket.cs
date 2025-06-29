using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ModalWindowAnswerIncomingPacket : IIncomingPacket
    {
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            //TODO: FeatureFlag.OfflineTrainingTime
        }
    }
}