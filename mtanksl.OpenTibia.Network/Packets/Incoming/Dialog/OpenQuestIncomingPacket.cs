using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OpenQuestIncomingPacket : IIncomingPacket
    {
        public ushort QuestId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            QuestId = reader.ReadUShort();
        }
    }
}