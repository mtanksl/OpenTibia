using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToTileCommand : MoveItemCommand
    {
        public MoveItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, Position toPosition, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Tile toTile = server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
                        {
                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject));
                        }
                        else
                        {
                            if ( !server.Pathfinding.CanThrow(Player.Tile.Position, toTile.Position) )
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );
                            }
                            else
                            {
                                //Act

                                RemoveItem(fromContainer, FromContainerIndex, server, context);

                                AddItem(toTile, fromItem, server, context);

                                Container container = fromItem as Container;

                                if (container != null)
                                {
                                    switch (fromContainer.GetRootContainer() )
                                    {
                                        case Tile fromTile:

                                            CloseContainer(fromTile, toTile, container, server, context);

                                            break;

                                        case Inventory fromInventory:

                                            CloseContainer(fromInventory, toTile, container, server, context);
                                            
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