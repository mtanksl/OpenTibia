using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toSlot, byte count) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;
            
            ToSlot = toSlot;

            Count = count;
        }

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
                        //Act

                        if ( IsMoveable(fromItem, server, context) &&

                             IsPickupable(fromItem, server, context) )
                        {
                            new ContainerRemoveItemCommand(fromContainer, FromContainerIndex).Execute(server, context);

                            new InventoryAddItemCommand(toInventory, ToSlot, fromItem).Execute(server, context);

                            base.Execute(server, context);
                        }
                    }
                }
            }
        }
    }
}