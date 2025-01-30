using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseNpcTradeOutgoingPacket : IOutgoingPacket
    {
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7C );
        }
    }
}