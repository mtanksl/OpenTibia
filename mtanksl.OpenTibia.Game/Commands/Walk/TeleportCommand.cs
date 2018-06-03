using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class TeleportCommand : Command
    {
        private Server server;

        public TeleportCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Position Position { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Tile toTile = server.Map.GetTile(Position);

            //Act

            byte fromIndex = fromTile.RemoveContent(Player);

            byte toIndex = toTile.AddContent(Player);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    bool canSeeFromPosition = observer.Tile.Position.CanSee(fromTile.Position);

                    bool canSeeToPosition = observer.Tile.Position.CanSee(toTile.Position);

                    if (canSeeFromPosition && canSeeToPosition)
                    {
                        context.Response.Write(observer.Client.Connection, new Walk(fromTile.Position, fromIndex, toTile.Position) );
                    }
                    else if (canSeeFromPosition)
                    {
                        context.Response.Write(observer.Client.Connection, new ThingRemove(fromTile.Position, fromIndex) );
                    }
                    else if (canSeeToPosition)
                    {
                        uint removeId;

                        if (observer.Client.IsKnownCreature(Player.Id, out removeId) )
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, Player) );
                        }
                        else
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, removeId, Player) );
                        }
                    }
                }
            }

            int deltaY = toTile.Position.Y - fromTile.Position.Y;

            int deltaX = toTile.Position.X - fromTile.Position.X;

            if (deltaY < -1 || deltaY > 1 || deltaX < -1 || deltaX > 1)
            {
                context.Response.Write(Player.Client.Connection, new SendTiles(server.Map, Player.Client, toTile.Position) );
            }
            else
            {
                context.Response.Write(Player.Client.Connection, new Walk(fromTile.Position, fromIndex, toTile.Position) );

                if (deltaY == -1)
                {
                    context.Response.Write(Player.Client.Connection, new SendMapNorth(server.Map, Player.Client, fromTile.Position) );
                }
                else if (deltaY == 1)
                {
                    context.Response.Write(Player.Client.Connection, new SendMapSouth(server.Map, Player.Client, fromTile.Position) );
                }
                            
                if (deltaX == -1)
                {
                    context.Response.Write(Player.Client.Connection, new SendMapWest(server.Map, Player.Client, fromTile.Position) );
                }
                else if (deltaX == 1)
                {
                    context.Response.Write(Player.Client.Connection, new SendMapEast(server.Map, Player.Client, fromTile.Position) );
                }
            }
        }
    }
}