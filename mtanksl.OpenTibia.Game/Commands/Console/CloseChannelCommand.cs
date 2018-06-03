using OpenTibia.Common.Objects;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class CloseChannelCommand : Command
    {
        private Server server;

        public CloseChannelCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public override void Execute(Context context)
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
        }
    }
}