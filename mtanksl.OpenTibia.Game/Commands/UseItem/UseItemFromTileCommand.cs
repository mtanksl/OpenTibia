using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromTileCommand : UseItemCommand
    {
        public UseItemFromTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
                    {
                        MoveDirection[] moveDirections = server.Pathfinding.GetMoveDirections(Player.Tile.Position, fromTile.Position);

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
                        Container container = fromItem as Container;

                        if (container != null)
                        {
                            //Act

                            OpenOrCloseContainer(Player, container, server, context);

                            base.Execute(server, context);
                        }
                        else
                        {
                            //TODO: Refactor

                            Command command = null;

                            switch (fromItem.Metadata.OpenTibiaId)
                            {
                                case 6356:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6357);

                                    break;

                                case 6357:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6356);

                                    break;

                                case 6358:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6359);

                                    break;

                                case 6359:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6358);

                                    break;

                                case 6360:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6361);

                                    break;

                                case 6361:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6360);

                                    break;

                                case 6362:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6363);

                                    break;

                                case 6363:

                                    command = new TileReplaceItemCommand(fromTile, FromIndex, 6362);

                                    break;
                            }

                            if (command != null)
                            {
                                command.Completed += (s, e) =>
                                {
                                    //Act

                                    base.Execute(server, context);
                                };

                                command.Execute(server, context);
                            }
                            else
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                            }
                        }
                    }
                }
            }
        }
    }
}