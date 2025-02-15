using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
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
            switch (Item.Parent)
            {
                case Tile tile:

                    return Context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then( () =>
                    {
                        if (Detach(Context, Item) )
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                ClearComponentsAndEventHandlers(Context, Item);

                                return Promise.Completed;
                            } );
                        }

                        return Promise.Completed;
                    } );

                case Inventory inventory:

                    return Context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then( () =>
                    {
                        if (Detach(Context, Item) )
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                ClearComponentsAndEventHandlers(Context, Item);

                                return Promise.Completed;
                            } );
                        }

                        return Promise.Completed;
                    } );

                case Container container:

                    return Context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then( () =>
                    {
                        if (Detach(Context, Item) )
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                ClearComponentsAndEventHandlers(Context, Item);

                                return Promise.Completed;
                            } );
                        }

                        return Promise.Completed;
                    } );

                default:

                    throw new NotImplementedException();
            }            
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