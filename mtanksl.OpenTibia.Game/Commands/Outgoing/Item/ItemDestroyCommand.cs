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

        public override Promise Execute()
        {
            if (Context.Server.ItemFactory.Detach(Item) )
            {
                if (Item is Container parent)
                {
                    foreach (var child in parent.GetItems() )
                    {
                        Detach(child);
                    }
                }

                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.ItemFactory.Destroy(Item);

                    if (Item is Container parent)
                    {
                        foreach (var child in parent.GetItems() )
                        {
                            Destroy(child);
                        }
                    }

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

        private void Detach(Item parent)
        {
            Context.Server.ItemFactory.Detach(parent);

            if (parent is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Detach(child);
		        }
	        }
        }

        private void Destroy(Item parent)
        {
            Context.Server.ItemFactory.Destroy(parent);

            if (parent is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Destroy(child);
		        }
	        }
        }
    }
}