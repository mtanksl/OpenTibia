using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class TurnCommand : Command
    {
        private Server server;

        public TurnCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Direction Direction { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            //Act

            Player.Direction = Direction;

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(fromTile.Position) )
                {
                    context.Response.Write(observer.Client.Connection, new ThingUpdate(fromTile.Position, fromIndex, Player.Id, Direction) );                        
                }
            }
        }
    }
}