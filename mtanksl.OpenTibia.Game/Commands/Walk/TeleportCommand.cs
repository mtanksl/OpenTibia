using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TeleportCommand : Command
    {
        public TeleportCommand(Player player, Position toPosition)
        {
            Player = player;

            ToPosition = toPosition;
        }

        public Player Player { get; set; }

        public Position ToPosition { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile toTile = server.Map.GetTile(ToPosition);

            if (toTile != null)
            {
                Tile fromTile = Player.Tile;

                Position fromPosition = fromTile.Position;

                byte fromIndex = fromTile.GetIndex(Player);

                //Act

                fromTile.RemoveContent(fromIndex);

                byte toIndex = toTile.AddContent(Player);
            
                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer != Player)
                    {
                        bool canSeeFromPosition = observer.Tile.Position.CanSee(fromPosition);

                        bool canSeeToPosition = observer.Tile.Position.CanSee(ToPosition);

                        if (canSeeFromPosition && canSeeToPosition)
                        {
                            context.Write(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, ToPosition) );
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
                                context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(ToPosition, toIndex, Player) );
                            }
                            else
                            {
                                context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(ToPosition, toIndex, removeId, Player) );
                            }
                        }
                    }
                }

                context.Write(Player.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, ToPosition) );

                int deltaY = ToPosition.Y - fromPosition.Y;

                if (deltaY == -1)
                {
                    context.Write(Player.Client.Connection, new SendMapNorthOutgoingPacket(server.Map, Player.Client, fromPosition) );
                }
                else if (deltaY == 1)
                {
                    context.Write(Player.Client.Connection, new SendMapSouthOutgoingPacket(server.Map, Player.Client, fromPosition) );
                }
            
                int deltaX = ToPosition.X - fromPosition.X;

                if (deltaX == -1)
                {
                    context.Write(Player.Client.Connection, new SendMapWestOutgoingPacket(server.Map, Player.Client, fromPosition) );
                }
                else if (deltaX == 1)
                {
                    context.Write(Player.Client.Connection, new SendMapEastOutgoingPacket(server.Map, Player.Client, fromPosition) );
                }

                foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
                {
                    Tile tile = pair.Value.GetRootContainer() as Tile;

                    if (tile != null)
                    {
                        if ( !ToPosition.IsNextTo(tile.Position) )
                        {
                            //Act

                            Player.Client.ContainerCollection.CloseContainer(pair.Key);

                            //Notify

                            context.Write(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }

                base.Execute(server, context);
            }
        }
    }
}