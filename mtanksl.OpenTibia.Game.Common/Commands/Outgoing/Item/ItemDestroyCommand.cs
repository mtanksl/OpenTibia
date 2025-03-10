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

                    if (Context.Server.ItemFactory.Detach(Item) )
                    {
                        return Context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then( () =>
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                Context.Server.ItemFactory.ClearComponentsAndEventHandlers(Item);

                                return Promise.Completed;
                            } );

                            return Promise.Completed;
                        } );
                    }

                    break;

                case Inventory inventory:

                    if (Context.Server.ItemFactory.Detach(Item) )
                    {
                        return Context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then( () =>
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                Context.Server.ItemFactory.ClearComponentsAndEventHandlers(Item);

                                return Promise.Completed;
                            } );

                            return Promise.Completed;
                        } );
                    }

                    break;

                case Container container:

                    if (Context.Server.ItemFactory.Detach(Item) )
                    {
                        return Context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then( () =>
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                Context.Server.ItemFactory.ClearComponentsAndEventHandlers(Item);

                                return Promise.Completed;
                            } );

                            return Promise.Completed;
                        } );
                    }

                    break;

                default:

                    throw new NotImplementedException();
            }       

            return Promise.Completed;
        }
    }
}