using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort itemId, byte toSlot, byte count) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToSlot = toSlot;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Inventory toInventory = Player.Inventory;

                    if (IsMoveable(context, fromItem, Count) && IsPickupable(context, fromItem) )
                    {
                        context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toInventory, ToSlot, Count) ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );
                    }
                }
            } );
        }
    }
}