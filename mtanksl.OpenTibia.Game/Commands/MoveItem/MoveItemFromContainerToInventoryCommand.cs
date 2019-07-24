using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toSlot, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;
            
            ToSlot = toSlot;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToSlot { get; set; }

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
                    Inventory toInventory = Player.Inventory;

                    Item toItem = toInventory.GetContent(ToSlot) as Item;

                    if (toItem == null)
                    {
                        if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
                        {
                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );
                        }
                        else
                        {
                            if ( fromContainer.GetRootContainer() is Tile && !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
                            {
                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );
                            }
                            else
                            {
                                //Act

                                RemoveItem(fromContainer, FromContainerIndex, server, context);

                                AddItem(toInventory, ToSlot, fromItem, server, context);

                                Container container = fromItem as Container;

                                if (container != null)
                                {
                                    switch (fromContainer.GetRootContainer() )
                                    {
                                        case Tile fromTile:

                                            CloseContainer(fromTile, toInventory, container, server, context);

                                            break;

                                        case Inventory fromInventory:

                                            CloseContainer(fromInventory, toInventory, container, server, context);
                                            
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