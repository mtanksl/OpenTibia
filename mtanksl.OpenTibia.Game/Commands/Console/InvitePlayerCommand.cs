using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Controllers
{
    internal class InvitePlayerCommand : Command
    {
        private Server server;

        public InvitePlayerCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Player observer = server.Map.GetPlayers().Where(p => p.Name == Name).FirstOrDefault();

            //Act

            if (observer != null)
            {
                if (observer != Player)
                {
                    PrivateChannel privateChannel = server.Channels.GetPrivateChannel(Player);

                    if (privateChannel != null)
                    {
                        if ( !privateChannel.ContainsInvitation(observer) )
                        {
                            if ( !privateChannel.ContainsPlayer(observer) )
                            {
                                privateChannel.AddInvitation(observer);

                                //Notify

                                context.Response.Write(Player.Client.Connection, new ShowWindowText(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );

                                context.Response.Write(observer.Client.Connection, new ShowWindowText(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to his private chat channel." ) );
                            }
                        }
                    }
                }
            }
        }
    }
}