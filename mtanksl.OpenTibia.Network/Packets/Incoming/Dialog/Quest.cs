using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class Quest : IIncomingPacket
    {
        public ushort QuestId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            QuestId = reader.ReadUShort();
        }
    }
}