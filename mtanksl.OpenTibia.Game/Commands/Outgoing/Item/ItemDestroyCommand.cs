using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemDestroyCommand : Command
    {
        public ItemDestroyCommand(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            if (Detach(Context, Item) )
            {
                Context.Server.QueueForExecution( () =>
                {
                    ClearComponentsAndEventHandlers(Context, Item);

                    switch (Item.Parent)
                    {
                        case Tile tile:

                            return Context.AddCommand(new TileRemoveItemCommand(tile, Item) );

                        case Inventory inventory:

                            return Context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) );

                        case Container container:

                            return Context.AddCommand(new ContainerRemoveItemCommand(container, Item) );
                    }

                    return Promise.Completed;
                } );
            }

            return Promise.Completed;
        }

        private static bool Detach(Context context, Item item)
        {
            if (context.Server.ItemFactory.Detach(item) )
            {
                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        Detach(context, child);
                    }
                }

                return true;
            }

            return false;
        }

        private static void ClearComponentsAndEventHandlers(Context context, Item item)
        {
            context.Server.ItemFactory.ClearComponentsAndEventHandlers(item);

            if (item is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    ClearComponentsAndEventHandlers(context, child);
		        }
	        }
        }
    }
}