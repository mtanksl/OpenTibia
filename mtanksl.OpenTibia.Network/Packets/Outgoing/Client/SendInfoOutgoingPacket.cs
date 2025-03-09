using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendInfoOutgoingPacket : IOutgoingPacket
    {
        public SendInfoOutgoingPacket(uint creatureId, bool canReportBugs)
        {
            this.CreatureId = creatureId;

            this.CanReportBugs = canReportBugs;
        }

        public uint CreatureId { get; set; }

        public bool CanReportBugs { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x0A );

            writer.Write(CreatureId);

            writer.Write( (ushort)0x32 );

            writer.Write(CanReportBugs);
        }
    }
}