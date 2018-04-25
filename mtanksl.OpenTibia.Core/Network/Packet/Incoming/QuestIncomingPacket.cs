using OpenTibia.IO;

namespace OpenTibia
{
    public class QuestIncomingPacket : IIncomingPacket
    {
        public ushort QuestId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            QuestId = reader.ReadUShort();
        }
    }
}