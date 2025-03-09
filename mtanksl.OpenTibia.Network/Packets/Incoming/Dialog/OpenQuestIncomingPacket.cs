using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenQuestIncomingPacket : IIncomingPacket
    {
        public ushort QuestId { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            QuestId = reader.ReadUShort();
        }
    }
}