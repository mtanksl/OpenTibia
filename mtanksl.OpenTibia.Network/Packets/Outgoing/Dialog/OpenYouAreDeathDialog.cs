using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenYouAreDeathDialog : IOutgoingPacket
    {
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x28 );
        }
    }
}