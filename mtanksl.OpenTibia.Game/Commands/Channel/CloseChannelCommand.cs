using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CloseChannelCommand : Command
    {
        public CloseChannelCommand(Player player, ushort channelId)
        {
            Player = player;

            ChannelId = channelId;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Channel channel = server.Channels.GetChannel(ChannelId);

            if (channel != null)
            {
                //Act

                if (channel.ContainsPlayer(Player) )
                {
                    channel.RemovePlayer(Player);
                }

                PrivateChannel privateChannel = channel as PrivateChannel;

                if (privateChannel != null)
                {
                    if (privateChannel.Owner == Player)
                    {
                        foreach (var observer in privateChannel.GetPlayers().ToList() )
                        {
                            context.Write(observer.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );

                            privateChannel.RemovePlayer(observer);
                        }

                        foreach (var observer in privateChannel.GetInvitations().ToList() )
                        {
                            privateChannel.RemoveInvitation(observer);
                        }

                        server.Channels.RemoveChannel(privateChannel);
                    }
                }

                //Notify

                context.Write(Player.Client.Connection, new CloseChannelOutgoingPacket(channel.Id) );

                base.Execute(server, context);
            }
        }
    }
}