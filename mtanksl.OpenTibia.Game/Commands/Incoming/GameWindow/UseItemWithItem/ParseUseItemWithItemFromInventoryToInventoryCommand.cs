using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromInventoryToInventoryCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort fromItemId, byte toSlot, ushort toItemId) : base(player)
        {
            FromSlot = fromSlot;

            FromItemId = fromItemId;

            ToSlot = toSlot;

            ToItemId = toItemId;
        }

        public byte FromSlot { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToItemId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
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
                }
            } );
        }
    }
}