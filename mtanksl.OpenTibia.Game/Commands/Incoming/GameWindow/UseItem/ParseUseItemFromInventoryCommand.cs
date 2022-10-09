using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemFromInventoryCommand : ParseUseItemCommand
    {
        public ParseUseItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    context.AddCommand(new PlayerUseItemCommand(Player, fromItem, null) ).Then(ctx =>
                    {
                        resolve(ctx);
                    } );
                }
            } ); 
        }
    }
}