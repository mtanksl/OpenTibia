using OpenTibia.IO;

namespace OpenTibia
{
    public class HealthOutgoingPacket : IOutgoingPacket
    {
        public HealthOutgoingPacket(uint creatureId, byte health)
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