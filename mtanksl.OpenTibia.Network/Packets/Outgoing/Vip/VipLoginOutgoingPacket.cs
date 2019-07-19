using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class VipLoginOutgoingPacket : IOutgoingPacket
    {
        public VipLoginOutgoingPacket(uint creatureId)
        {
            this.CreatureId = creatureId;
        }

        public uint CreatureId { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xD3 );

            writer.Write(CreatureId);
        }
    }
}