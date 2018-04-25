using OpenTibia.IO;

namespace OpenTibia
{
    public class CloseNpcTradeOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7C );
        }
    }
}