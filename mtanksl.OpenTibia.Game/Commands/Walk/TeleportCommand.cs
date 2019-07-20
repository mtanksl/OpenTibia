using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TeleportCommand : Command
    {
        public TeleportCommand(Player player, Tile toTile)
        {
            Player = player;

            ToTile = toTile;
        }

        public Player Player { get; set; }

        public Tile ToTile { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            Position fromPosition = fromTile.Position;

            Position toPosition = ToTile.Position;

            //Act

            fromTile.RemoveContent(fromIndex);

            byte toIndex = ToTile.AddContent(Player);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    bool canSeeFromPosition = observer.Tile.Position.CanSee(fromPosition);

                    bool canSeeToPosition = observer.Tile.Position.CanSee(toPosition);

                    if (canSeeFromPosition && canSeeToPosition)
                    {
                        context.Write(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );
                    }
                    else if (canSeeFromPosition)
                    {
                        context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                    }
                    else if (canSeeToPosition)
                    {
                        uint removeId;

                        if (observer.Client.CreatureCollection.IsKnownCreature(Player.Id, out removeId) )
                        {
                            context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, Player) );
                        }
                        else
                        {
                            context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, removeId, Player) );
                        }
                    }
                }
            }

            context.Write(Player.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );

            int deltaY = toPosition.Y - fromPosition.Y;

            if (deltaY == -1)
            {
                context.Write(Player.Client.Connection, new SendMapNorthOutgoingPacket(server.Map, Player.Client, fromPosition) );
            }
            else if (deltaY == 1)
            {
                context.Write(Player.Client.Connection, new SendMapSouthOutgoingPacket(server.Map, Player.Client, fromPosition) );
            }
            
            int deltaX = toPosition.X - fromPosition.X;

            if (deltaX == -1)
            {
                context.Write(Player.Client.Connection, new SendMapWestOutgoingPacket(server.Map, Player.Client, fromPosition) );
            }
            else if (deltaX == 1)
            {
                context.Write(Player.Client.Connection, new SendMapEastOutgoingPacket(server.Map, Player.Client, fromPosition) );
            }

            base.Execute(server, context);
        }
    }
}