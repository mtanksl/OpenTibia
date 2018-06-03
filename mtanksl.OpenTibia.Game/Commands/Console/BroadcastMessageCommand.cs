using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class BroadcastMessageCommand : Command
    {
        private Server server;

        public BroadcastMessageCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    context.Response.Write(observer.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Broadcast, Message) );
                }
            }

            context.Response.Write(Player.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Broadcast, Message) );
        }
    }
}