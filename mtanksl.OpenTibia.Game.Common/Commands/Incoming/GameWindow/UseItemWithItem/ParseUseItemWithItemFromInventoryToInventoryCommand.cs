using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromInventoryToInventoryCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort fromTibiaId, byte toSlot, ushort toTibiaId) : base(player)
        {
            FromSlot = fromSlot;

            FromTibiaId = fromTibiaId;

            ToSlot = toSlot;

            ToTibiaId = toTibiaId;
        }

        public byte FromSlot { get; set; }

        public ushort FromTibiaId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
            {
                Inventory toInventory = Player.Inventory;

                Item toItem = toInventory.GetContent(ToSlot) as Item;

                if (toItem != null && toItem.Metadata.TibiaId == ToTibiaId)
                {
                    if ( IsUseable(fromItem) )
                    {
                        return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}