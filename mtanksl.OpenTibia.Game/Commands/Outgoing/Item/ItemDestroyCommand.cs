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
            if (Context.Server.ItemFactory.Detach(Item) )
            {
                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.ItemFactory.Destroy(Item);

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
    }
}