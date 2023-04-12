using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemFromHotkeyCommand : ParseUseItemCommand
    {
        public ParseUseItemFromHotkeyCommand(Player player, ushort itemId) : base(player)
        {
            ItemId = itemId;
        }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            foreach (var pair in fromInventory.GetIndexedContents() )
            {
                Item fromItem = (Item)pair.Value;

                if (fromItem.Metadata.TibiaId == ItemId)
                {
                    return Context.AddCommand(new PlayerUseItemCommand(Player, fromItem, null) );
                }
            }

            return Promise.Break;
        }
    }
}