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
            if (Detach(Item) )
            {
                Context.Server.QueueForExecution( () =>
                {
                    Destroy(Item);

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

        private bool Detach(Item item)
        {
            if (Context.Server.ItemFactory.Detach(item) )
            {
                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        Detach(child);
                    }
                }

                return true;
            }

            return false;
        }

        private void Destroy(Item item)
        {
            Context.Server.ItemFactory.Destroy(item);

            if (item is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Destroy(child);
		        }
	        }
        }
    }
}