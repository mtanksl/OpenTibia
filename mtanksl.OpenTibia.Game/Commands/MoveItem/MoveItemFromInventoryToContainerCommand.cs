using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToContainerCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToContainerCommand(Player player, byte fromSlot, ushort itemId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    //Act

                    if ( IsMoveable(fromItem, server, context) &&

                        IsPickupable(fromItem, server, context) &&

                        IsPossible(fromItem, toContainer, server, context) &&

                        IsEnoughtSpace(fromItem, toContainer, server, context) )
                    {
                        MoveItem(fromItem, toContainer, ToContainerIndex, Count, server, context);
                    }     
                }
            }
        }
    }
}