using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromTileToInventoryCommand : Command
    {
        public UseItemWithItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, ushort fromItemId, byte toSlot, ushort toItemId)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromItemId = fromItemId;

            ToSlot = toSlot;

            ToItemId = toItemId;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Inventory toInventory = Player.Inventory;

                    Item toItem = toInventory.GetContent(ToSlot) as Item;

                    if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
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
                            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                            }
                            else
                            {
                                //Act

                                ItemUseWithItemScript script;

                                if ( !server.ItemUseWithItemScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, toItem, server, context) )
                                {
                                    //Notify

                                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                                }
                                else
                                {
                                    base.Execute(server, context);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}