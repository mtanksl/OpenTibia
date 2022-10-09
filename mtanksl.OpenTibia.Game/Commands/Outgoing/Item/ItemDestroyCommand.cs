using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemDestroyCommand : Command
    {
        public ItemDestroyCommand(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                switch (Item.Parent)
                {
                    case Tile tile:

                        context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then( (ctx, index) =>
                        {
                            ctx.Server.ItemFactory.Destroy(Item);

                            resolve(ctx);
                        } );
                  
                        break;

                    case Inventory inventory:

                        context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then( (ctx, index) =>
                        {
                            ctx.Server.ItemFactory.Destroy(Item);

                            resolve(ctx);
                        } );
                   
                        break;

                    case Container container:

                        context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then( (ctx, index) =>
                        {
                            ctx.Server.ItemFactory.Destroy(Item);

                            resolve(ctx);
                        } );

                        break;
                }
            } );            
        }
    }
}