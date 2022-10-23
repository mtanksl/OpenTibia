using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromHotkeyToInventoryCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromHotkeyToInventoryCommand(Player player, ushort fromItemId, byte toSlot, ushort toItemId) : base(player)
        {
            FromItemId = fromItemId;

            ToSlot = toSlot;

            ToItemId = toItemId;
        }

        public ushort FromItemId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToItemId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                foreach (var pair in fromInventory.GetIndexedContents() )
                {
                    Item fromItem = (Item)pair.Value;

                    if (fromItem.Metadata.TibiaId == FromItemId)
                    {
                        Inventory toInventory = Player.Inventory;

                        Item toItem = toInventory.GetContent(ToSlot) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            if ( IsUseable(context, fromItem) )
                            {
                                context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) ).Then(ctx =>
                                {
                                    resolve(context);
                                } );
                            }
                        }

                        break;
                    }
                }
            } );
        }
    }
}