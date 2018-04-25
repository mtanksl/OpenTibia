using OpenTibia.IO;

namespace OpenTibia
{
    public class InfoOutgoingPacket : IOutgoingPacket
    {
        public InfoOutgoingPacket(uint creatureId, bool canReportBugs)
        {
            this.CreatureId = creatureId;

            this.CanReportBugs = canReportBugs;
        }

        public uint CreatureId { get; set; }

        public bool CanReportBugs { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x0A );

            writer.Write(CreatureId);

            writer.Write( (ushort)0x32 );

            writer.Write(CanReportBugs);
        }
    }
}