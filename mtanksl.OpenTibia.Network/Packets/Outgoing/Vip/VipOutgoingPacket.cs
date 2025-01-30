using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class VipOutgoingPacket : IOutgoingPacket
    {
        public VipOutgoingPacket(uint id, string name, bool online)
        {
            this.Id = id;

            this.Name = name;

            this.Online = online;
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xD2 );

            writer.Write(Id);

            writer.Write(Name);

            writer.Write(Online);
        }
    }
}