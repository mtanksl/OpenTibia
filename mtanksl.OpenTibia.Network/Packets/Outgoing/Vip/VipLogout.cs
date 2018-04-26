using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class VipLogout : IOutgoingPacket
    {
        public VipLogout(uint creatureId)
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