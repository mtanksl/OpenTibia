using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenQuestIncomingPacket : IIncomingPacket
    {
        public ushort QuestId { get; set; }

        public void Read(IByteArrayStreamReader reader)
        {
            QuestId = reader.ReadUShort();
        }
    }
}