using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromInventoryCommand : ParseLookCommand
    {
        public ParseLookFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item item = fromInventory.GetContent(FromSlot) as Item;

            if (item != null && item.Metadata.TibiaId == ItemId)
            {
                return Context.AddCommand(new PlayerLookItemCommand(Player, item) );
            }

            return Promise.Break;
        }
    }
}