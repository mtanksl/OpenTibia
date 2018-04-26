using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Vip : IOutgoingPacket
    {
        public Vip(uint creatureId, string name, bool online)
        {
            this.CreatureId = creatureId;

            this.Name = name;

            this.Online = online;
        }

        public uint CreatureId { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xD2 );

            writer.Write(CreatureId);

            writer.Write(Name);

            writer.Write(Online);
        }
    }
}