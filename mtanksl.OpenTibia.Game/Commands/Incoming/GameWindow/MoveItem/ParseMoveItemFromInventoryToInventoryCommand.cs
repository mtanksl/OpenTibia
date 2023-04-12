using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromInventoryToInventoryCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort itemId, byte toSlot, byte count) : base(player)
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

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Inventory toInventory = Player.Inventory;

                if (IsMoveable(Context, fromItem, Count) )
                {
                    return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toInventory, ToSlot, Count, true) );
                }
            }

            return Promise.Break;
        }
    }
}