using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseTradeOutgoingPacket : IOutgoingPacket
    {
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7F );
        }
    }
}