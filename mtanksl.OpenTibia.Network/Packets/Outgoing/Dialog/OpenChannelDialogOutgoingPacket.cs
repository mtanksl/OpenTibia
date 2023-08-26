using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenChannelDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenChannelDialogOutgoingPacket(List<ChannelDto> channels)
        {
            this.channels = channels;
        }

        private List<ChannelDto> channels;

        public List<ChannelDto> Channels
        {
            get
            {
                return channels ?? ( channels = new List<ChannelDto>() );
            }
            set
            {
                channels = value;
            }
        }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xAB );

            writer.Write( (byte)Channels.Count );

            foreach (var channel in Channels)
            {
                writer.Write(channel.Id);

                writer.Write(channel.Name);
            }
        }
    }
}