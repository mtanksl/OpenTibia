using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseRuleViolationOutgoingPacket : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB1 );
        }
    }
}