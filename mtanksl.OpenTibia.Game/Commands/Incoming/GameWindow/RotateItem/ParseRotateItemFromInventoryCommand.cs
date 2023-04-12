using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseRotateItemFromInventoryCommand : ParseRotateItemCommand
    {
        public ParseRotateItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                if ( IsRotatable(Context, fromItem) )
                {
                    return Context.AddCommand(new PlayerRotateItemCommand(Player, fromItem) );
                }
            }

            return Promise.Break;
        }
    }
}