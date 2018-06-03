using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class OpenedMyPrivateChannelCommand : Command
    {
        private Server server;

        public OpenedMyPrivateChannelCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            PrivateChannel privateChannel = server.Channels.GetPrivateChannel(Player);

            //Act

            if (privateChannel == null)
            {
                privateChannel = new PrivateChannel()
                {
                    Owner = Player,

                    Name = Player.Name + " Channel"
                };

                server.Channels.AddChannel(privateChannel);
            }

            //Notify

            context.Response.Write(Player.Client.Connection, new OpenMyPrivateChannel(privateChannel.Id, privateChannel.Name) );
        }
    }
}