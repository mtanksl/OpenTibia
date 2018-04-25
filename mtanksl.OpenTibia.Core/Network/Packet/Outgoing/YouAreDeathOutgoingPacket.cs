using OpenTibia.IO;

namespace OpenTibia
{
    public class YouAreDeathOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x28 );
        }
    }
}