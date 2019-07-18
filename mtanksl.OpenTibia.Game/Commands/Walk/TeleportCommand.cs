using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TeleportCommand : Command
    {
        public TeleportCommand(Player player, Position position)
        {
            Player = player;

            Position = position;
        }

        public Player Player { get; set; }

        public Position Position { get; set; }

        public override void Execute(Server server, CommandContext context)
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
                        context.Write(observer.Client.Connection, new Walk(fromTile.Position, fromIndex, toTile.Position) );
                    }
                    else if (canSeeFromPosition)
                    {
                        context.Write(observer.Client.Connection, new ThingRemove(fromTile.Position, fromIndex) );
                    }
                    else if (canSeeToPosition)
                    {
                        uint removeId;

                        if (observer.Client.IsKnownCreature(Player.Id, out removeId) )
                        {
                            context.Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, Player) );
                        }
                        else
                        {
                            context.Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, removeId, Player) );
                        }
                    }
                }
            }

            int deltaY = toTile.Position.Y - fromTile.Position.Y;

            int deltaX = toTile.Position.X - fromTile.Position.X;

            if (deltaY < -1 || deltaY > 1 || deltaX < -1 || deltaX > 1)
            {
                context.Write(Player.Client.Connection, new SendTilesOutgoingPacket(server.Map, Player.Client, toTile.Position) );
            }
            else
            {
                context.Write(Player.Client.Connection, new Walk(fromTile.Position, fromIndex, toTile.Position) );

                if (deltaY == -1)
                {
                    context.Write(Player.Client.Connection, new SendMapNorthOutgoingPacket(server.Map, Player.Client, fromTile.Position) );
                }
                else if (deltaY == 1)
                {
                    context.Write(Player.Client.Connection, new SendMapSouthOutgoingPacket(server.Map, Player.Client, fromTile.Position) );
                }
                            
                if (deltaX == -1)
                {
                    context.Write(Player.Client.Connection, new SendMapWestOutgoingPacket(server.Map, Player.Client, fromTile.Position) );
                }
                else if (deltaX == 1)
                {
                    context.Write(Player.Client.Connection, new SendMapEastOutgoingPacket(server.Map, Player.Client, fromTile.Position) );
                }
            }
        }
    }
}