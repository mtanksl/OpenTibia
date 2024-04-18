using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromInventoryCommand : ParseLookCommand
    {
        public ParseLookFromInventoryCommand(Player player, byte fromSlot, ushort tibiaId) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item item = fromInventory.GetContent(FromSlot) as Item;

            if (item != null && item.Metadata.TibiaId == TibiaId)
            {
                return Context.AddCommand(new PlayerLookItemCommand(Player, item) );
            }

            return Promise.Break;
        }
    }
}