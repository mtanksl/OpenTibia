using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateParentToInventoryCommand : Command
    {
        public ItemUpdateParentToInventoryCommand(Item item, Inventory toInventory, byte slot)
        {
            Item = item;

            ToInventory = toInventory;

            Slot = slot;
        }

        public Item Item { get; set; }

        public Inventory ToInventory { get; set; }

        public byte Slot { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                switch (Item.Parent)
                {
                    case Tile fromTile:

                        context.AddCommand(new TileRemoveItemCommand(fromTile, Item) ).Then( (ctx, index) =>
                        {
                            return ctx.AddCommand(new InventoryAddItemCommand(ToInventory, Slot, Item) );

                        } ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );

                        break;

                    case Inventory fromInventory:

                        context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) ).Then( (ctx, index) =>
                        {
                            return ctx.AddCommand(new InventoryAddItemCommand(ToInventory, Slot, Item) );

                        } ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );

                        break;

                    case Container fromContainer:

                        context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) ).Then( (ctx, index) =>
                        {
                            return ctx.AddCommand(new InventoryAddItemCommand(ToInventory, Slot, Item) );

                        } ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );

                        break;
                }
            } );
        }
    }
}