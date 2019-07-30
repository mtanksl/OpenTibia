using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort itemId, byte toSlot, byte count) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToSlot = toSlot;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

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
                        MoveItem(fromItem, toInventory, ToSlot, Count, server, context);
                    }
                }
            }
        }
    }
}