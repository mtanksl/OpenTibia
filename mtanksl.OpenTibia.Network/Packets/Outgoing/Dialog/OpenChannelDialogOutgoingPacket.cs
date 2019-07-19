using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenChannelDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenChannelDialogOutgoingPacket(List<Channel> channels)
        {
            this.channels = channels;
        }

        private List<Channel> channels;

        public List<Channel> Channels
        {
            get
            {
                return channels ?? ( channels = new List<Channel>() );
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