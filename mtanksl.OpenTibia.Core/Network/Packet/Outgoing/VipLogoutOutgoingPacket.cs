using OpenTibia.IO;

namespace OpenTibia
{
    public class VipLogoutOutgoingPacket : IOutgoingPacket
    {
        public VipLogoutOutgoingPacket(uint creatureId)
        {
            this.CreatureId = creatureId;
        }

        public uint CreatureId { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xD4 );

            writer.Write(CreatureId);
        }
    }
}