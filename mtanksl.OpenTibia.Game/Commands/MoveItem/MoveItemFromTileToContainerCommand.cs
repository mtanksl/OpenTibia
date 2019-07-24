using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToContainerCommand : MoveItemCommand
    {
        public MoveItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

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
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
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
                            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );
                            }
                            else
                            {
                                if ( toContainer.GetRootContainer() is Inventory && !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
                                {
                                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );
                                }
                                else
                                {
                                    if ( toContainer.IsChild(fromItem) )
                                    {
                                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );
                                    }
                                    else
                                    {
                                        //Act

                                        RemoveItem(fromTile, FromIndex, server, context);

                                        AddItem(toContainer, fromItem, server, context);

                                        Container container = fromItem as Container;

                                        if (container != null)
                                        {
                                            switch (toContainer.GetRootContainer() )
                                            {
                                                case Tile toTile:

                                                    CloseContainer(fromTile, toTile, container, server, context);

                                                    break;

                                                case Inventory toInventory:

                                                    CloseContainer(fromTile, toInventory, container, server, context);
                                            
                                                    break;
                                            }

                                            ShowOrHideOpenParentContainer(container, server, context);
                                        }

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
}