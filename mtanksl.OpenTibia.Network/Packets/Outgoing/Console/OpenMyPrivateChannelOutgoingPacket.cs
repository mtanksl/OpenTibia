using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenMyPrivateChannelOutgoingPacket : IOutgoingPacket
    {
        public OpenMyPrivateChannelOutgoingPacket(ushort channelId, string name)
        {
            this.ChannelId = channelId;

            this.Name = name;
        }

        public ushort ChannelId { get; set; }

        public string Name { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xB2 );

            writer.Write(ChannelId);

            writer.Write(Name);
        }
    }
}