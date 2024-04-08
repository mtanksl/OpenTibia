using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromInventoryToInventoryCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort tibiaId, byte toSlot, byte count) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;

            ToSlot = toSlot;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
            {
                Inventory toInventory = Player.Inventory;

                if (IsPickupable(fromItem) && IsMoveable(fromItem, Count) )
                {
                    return Context.AddCommand(new PlayerMoveItemCommand(this, Player, fromItem, toInventory, ToSlot, Count, true) );
                }
            }

            return Promise.Break;
        }
    }
}