using OpenTibia.IO;

namespace OpenTibia
{
    public class TutorialHintOutgoingPacket : IOutgoingPacket
    {
        public TutorialHintOutgoingPacket(byte tutorialId)
        {
            this.TutorialId = tutorialId;
        }

        public byte TutorialId { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xDC );

            writer.Write(TutorialId);
        }
    }
}