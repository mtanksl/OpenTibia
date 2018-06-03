using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class YellCommand : Command
    {
        private Server server;

        public YellCommand(Server server)
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
                    if (observer.Tile.Position.CanHearYell(Player.Tile.Position) )
                    {
                        context.Response.Write(observer.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Yell, Player.Tile.Position, Message.ToUpper() ) );
                    }
                }
            }

            context.Response.Write(Player.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Yell, Player.Tile.Position, Message.ToUpper() ) );
        }
    }
}