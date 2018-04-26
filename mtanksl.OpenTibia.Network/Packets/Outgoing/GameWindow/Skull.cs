using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Skull : IOutgoingPacket
    {
        public Skull(uint creatureId, Skull skull)
        {
            this.CreatureId = creatureId;

            this.Skull = skull;
        }

        public uint CreatureId { get; set; }

        public Skull Skull { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x90 );

            writer.Write(CreatureId);

            writer.Write( (byte)Skull );
        }
    }
}