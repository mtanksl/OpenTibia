using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendChannelEventOutgoingPacket : IOutgoingPacket
    {
        public SendChannelEventOutgoingPacket(ushort channelId, string playerName, ChannelEvent channelEvent)
        {
            this.ChannelId = channelId;

            this.PlayerName = playerName;

            this.ChannelEvent = channelEvent;
        }

        public ushort ChannelId { get; set; }

        public string PlayerName { get; set; }

        public ChannelEvent ChannelEvent { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xF3 );

            writer.Write(ChannelId);

            writer.Write(PlayerName);

            writer.Write( (byte)ChannelEvent);
        }
    }
}