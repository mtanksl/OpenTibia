using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemFromInventoryCommand : ParseUseItemCommand
    {
        public ParseUseItemFromInventoryCommand(Player player, byte fromSlot, ushort tibiaId) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
            {
                return Context.AddCommand(new PlayerUseItemCommand(Player, fromItem, null) );
            }

            return Promise.Break;
        }
    }
}