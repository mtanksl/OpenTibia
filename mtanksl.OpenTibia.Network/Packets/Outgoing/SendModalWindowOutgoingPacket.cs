using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendModalWindowOutgoingPacket : IOutgoingPacket
    {
        public SendModalWindowOutgoingPacket()
        {

        }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xFA );

            //TODO
        }
    }
}