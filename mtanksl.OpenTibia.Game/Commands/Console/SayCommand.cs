using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class SayCommand : Command
    {
        private Server server;

        public SayCommand(Server server)
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
                    if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                    {
                        context.Response.Write(observer.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message) );
                    }
                }
            }

            context.Response.Write(Player.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message) );
        }
    }
}