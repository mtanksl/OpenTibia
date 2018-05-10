using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetHealth : IOutgoingPacket
    {
        public SetHealth(uint creatureId, byte health)
        {
            this.CreatureId = creatureId;

            this.Health = health;
        }

        public uint CreatureId { get; set; }

        public byte Health { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x8C );

            writer.Write(CreatureId);

            writer.Write(Health);
        }
    }
}