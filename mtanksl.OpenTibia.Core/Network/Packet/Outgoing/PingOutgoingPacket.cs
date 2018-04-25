using OpenTibia.IO;

namespace OpenTibia
{
    public class PingOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x1E );
        }
    }
}