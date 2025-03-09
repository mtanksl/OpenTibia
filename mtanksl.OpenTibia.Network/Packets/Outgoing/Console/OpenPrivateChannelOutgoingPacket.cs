using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenPrivateChannelOutgoingPacket : IOutgoingPacket
    {
        public OpenPrivateChannelOutgoingPacket(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xAD );

            writer.Write(Name);
        }
    }
}