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
                            Destroy(ctx, Item);

                            resolve(ctx);
                        } );
                  
                        break;

                    case Inventory inventory:

                        context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then( (ctx, index) =>
                        {
                            Destroy(ctx, Item);

                            resolve(ctx);
                        } );
                   
                        break;

                    case Container container:

                        context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then( (ctx, index) =>
                        {
                            Destroy(ctx, Item);

                            resolve(ctx);
                        } );

                        break;
                }
            } );            
        }

        private void Destroy(Context context, Item item)
        {
            if (item is Container container)
            {
                foreach (var child in container.GetItems() )
                {
                    Destroy(context, child);
                }
            }

            context.Server.ItemFactory.Destroy(item);
        }
    }
}