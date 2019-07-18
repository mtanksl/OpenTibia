using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendConnectionInfoOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x1F );

            writer.Write( (uint)0x00 );

            writer.Write( (byte)0x00 );
        }
    }
}