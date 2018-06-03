using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Controllers
{
    public class ExcludePlayerCommand : Command
    {
        private Server server;

        public ExcludePlayerCommand(Server server)
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
                        if (privateChannel.ContainsInvitation(observer) )
                        {
                            privateChannel.RemoveInvitation(observer);

                            //Notify

                            context.Response.Write(Player.Client.Connection, new ShowWindowText(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );
                        }
                        else if (privateChannel.ContainsPlayer(observer) )
                        {
                            privateChannel.RemovePlayer(observer);

                            //Notify

                            context.Response.Write(Player.Client.Connection, new ShowWindowText(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );

                            context.Response.Write(observer.Client.Connection, new CloseChannel(privateChannel.Id) );
                        }
                    }
                }
            }
        }
    }
}