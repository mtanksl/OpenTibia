using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Speed : IOutgoingPacket
    {
        public Speed(uint creatureId, ushort speed)
        {
            this.CreatureId = creatureId;

            this.Speed = speed;
        }

        public uint CreatureId { get; set; }
        
        public ushort Speed { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x8F );

            writer.Write(CreatureId);

            writer.Write(Speed);
        }
    }
}