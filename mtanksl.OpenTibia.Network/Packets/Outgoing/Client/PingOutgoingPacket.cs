using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class PingOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x1E );
        }
    }
}