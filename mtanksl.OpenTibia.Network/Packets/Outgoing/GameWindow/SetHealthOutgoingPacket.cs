using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetHealthOutgoingPacket : IOutgoingPacket
    {
        public SetHealthOutgoingPacket(uint creatureId, byte healthPercantage)
        {
            this.CreatureId = creatureId;

            this.HealthPercentage = healthPercantage;
        }

        public uint CreatureId { get; set; }

        public byte HealthPercentage { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x8C );

            writer.Write(CreatureId);

            writer.Write(HealthPercentage);
        }
    }
}