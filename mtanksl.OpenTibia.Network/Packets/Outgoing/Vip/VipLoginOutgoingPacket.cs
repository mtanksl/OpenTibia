using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class VipLoginOutgoingPacket : IOutgoingPacket
    {
        public VipLoginOutgoingPacket(uint id)
        {
            this.Id = id;
        }

        public uint Id { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xD3 );

            writer.Write(Id);
        }
    }
}