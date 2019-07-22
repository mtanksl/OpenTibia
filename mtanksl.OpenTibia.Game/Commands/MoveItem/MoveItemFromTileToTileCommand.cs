using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToTileCommand : MoveItemCommand
    {
        public MoveItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, Position toPosition, byte count)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                        {
                            MoveDirection[] moveDirections = server.Pathfinding.Walk(Player.Tile.Position, fromTile.Position);

                            if (moveDirections.Length == 0)
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );
                            }
                            else
                            {
                                WalkToCommand command = new WalkToCommand(Player, moveDirections);

                                command.Completed += (s, e) =>
                                {
                                    Execute(e.Server, e.Context);
                                };

                                command.Execute(server, context);
                            }
                        }
                        else
                        {
                            if ( !server.Pathfinding.IsLineOfSightClear(Player.Tile.Position, toTile.Position) )
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );
                            }
                            else
                            {
                                //Act

                                RemoveItem(fromTile, FromIndex, server, context);

                                AddItem(toTile, fromItem, server, context);

                                base.Execute(server, context);
                            }
                        }
                    }
                }
            }
        }
    }
}