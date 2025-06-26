using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendEnterWorldOutgoingPacket : IOutgoingPacket
    {
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x0F);
        }
    }    
}