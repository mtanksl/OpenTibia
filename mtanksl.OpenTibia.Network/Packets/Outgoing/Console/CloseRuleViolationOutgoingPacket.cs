using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class CloseRuleViolationOutgoingPacket : IOutgoingPacket
    {
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB1 );
        }
    }
}