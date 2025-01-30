using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class VipLogoutOutgoingPacket : IOutgoingPacket
    {
        public VipLogoutOutgoingPacket(uint id)
        {
            this.Id = id;
        }

        public uint Id { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xD4 );

            writer.Write(Id);
        }
    }
}