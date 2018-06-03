using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class SendMessageToPlayerCommand : Command
    {
        private Server server;

        public SendMessageToPlayerCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

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

                    context.Response.Write(observer.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Private, Message) );
                }
            }
        }
    }
}