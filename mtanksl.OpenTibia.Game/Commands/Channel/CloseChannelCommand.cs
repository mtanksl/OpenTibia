using OpenTibia.Common.Objects;

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

            //Act

            if (channel != null)
            {
                PrivateChannel privateChannel = channel as PrivateChannel;

                if (privateChannel != null)
                {
                    if (privateChannel.ContainsInvitation(Player) )
                    {
                        //
                    }
                    else if (privateChannel.ContainsPlayer(Player) )
                    {
                        channel.RemovePlayer(Player);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (channel.ContainsPlayer(Player) )
                    {
                        channel.RemovePlayer(Player);
                    }
                    else
                    {
                        //
                    }
                }
            }

            //Notify

            base.Execute(server, context);
        }
    }
}