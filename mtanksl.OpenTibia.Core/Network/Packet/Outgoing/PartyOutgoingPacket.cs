using OpenTibia.IO;

namespace OpenTibia
{
    public class PartyOutgoingPacket : IOutgoingPacket
    {
        public PartyOutgoingPacket(uint creatureId, Party party)
        {
            this.CreatureId = creatureId;

            this.Party = party;
        }

        public uint CreatureId { get; set; }

        public Party Party { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x91 );

            writer.Write(CreatureId);

            writer.Write( (byte)Party );
        }
    }
}