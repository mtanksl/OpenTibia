using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemUpdateParentToTileCommand : Command
    {
        public ItemUpdateParentToTileCommand(Item item, Tile toTile)
        {
            Item = item;

            ToTile = toTile;
        }

        public Item Item { get; set; }

        public Tile ToTile { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                switch (Item.Parent)
                {
                    case Tile fromTile:

                        context.AddCommand(new TileRemoveItemCommand(fromTile, Item) ).Then( (ctx, index) =>
                        {
                            return ctx.AddCommand(new TileAddItemCommand(ToTile, Item) );

                        } ).Then( (ctx, index) =>
                        {
                            resolve(ctx);
                        } );

                        break;

                    case Inventory fromInventory:

                        context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) ).Then( (ctx, index) =>
                        {
                            return ctx.AddCommand(new TileAddItemCommand(ToTile, Item) );

                        } ).Then( (ctx, index) =>
                        {
                            resolve(ctx);
                        } );

                        break;

                    case Container fromContainer:

                        context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) ).Then( (ctx, index) =>
                        {
                            return ctx.AddCommand(new TileAddItemCommand(ToTile, Item) );

                        } ).Then( (ctx, index) =>
                        {
                            resolve(ctx);
                        } );

                        break;
                }
            } );
        }
    }
}