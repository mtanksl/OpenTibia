using OpenTibia.IO;

namespace OpenTibia
{
    public class CloseReportOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB1 );
        }
    }
}