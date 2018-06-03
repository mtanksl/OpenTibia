using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class OpenedPrivateChannelCommand : Command
    {
        private Server server;

        public OpenedPrivateChannelCommand(Server server)
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
                    //Notify

                    context.Response.Write(Player.Client.Connection, new OpenPrivateChannel(Name) );
                }
            }
        }
    }
}