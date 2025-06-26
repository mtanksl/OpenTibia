using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenChannelOutgoingPacket : IOutgoingPacket
    {
        public OpenChannelOutgoingPacket(ushort channelId, string name)
        {
            this.ChannelId = channelId;

            this.Name = name;
        }

        public ushort ChannelId { get; set; }

        public string Name { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xAC );

            writer.Write(ChannelId);

            writer.Write(Name);

            if (features.HasFeatureFlag(FeatureFlag.ChannelPlayerList) )
            {
                writer.Write( (ushort)0 ); //TODO: FeatureFlag.ChannelPlayerList, count and names

                writer.Write( (ushort)0 ); //TODO: FeatureFlag.ChannelPlayerList, count and names
            }
        }
    }
}