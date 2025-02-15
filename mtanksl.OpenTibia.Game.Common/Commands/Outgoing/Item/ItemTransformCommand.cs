using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : CommandResult<Item>
    {
        public ItemTransformCommand(Item item, ushort openTibiaId, byte count)
        {
            Item = item;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Item Item { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override PromiseResult<Item> Execute()
        {
            Item toItem = Context.Server.ItemFactory.Create(OpenTibiaId, Count);

            if (toItem != null)
            {
                Context.Server.ItemFactory.Attach(toItem);

                toItem.ActionId = Item.ActionId;

                toItem.UniqueId = Item.UniqueId;

                if (Item is Container fromContainer)
                {
                    if (toItem is Container toContainer)
                    {
                        while (fromContainer.Count > 0)
                        {
                            IContent content = fromContainer.GetContent(fromContainer.Count - 1);

                            fromContainer.RemoveContent(fromContainer.Count - 1);

                            toContainer.AddContent(content);
                        }
                    }
                    else if (fromContainer.Count > 0)
                    {
                        throw new InvalidOperationException("ToItem must be Container.");
                    }
                }
                else if (Item is DoorItem fromDoorItem)
                {
                    if (toItem is DoorItem toDoorItem)
                    {
                        toDoorItem.DoorId = fromDoorItem.DoorId;
                    }
                    else
                    {
                        throw new InvalidOperationException("ToItem must be DoorItem.");
                    }
                }
                else if (Item is ReadableItem fromReadableItem)
                {
                    if (toItem is ReadableItem toReadableItem)
                    {
                        toReadableItem.Text = fromReadableItem.Text;

                        toReadableItem.Author = fromReadableItem.Author;

                        toReadableItem.Date = fromReadableItem.Date;
                    }
                    else
                    {
                        throw new InvalidOperationException("ToItem must be ReadableItem.");
                    }
                }
                else if (Item is TeleportItem fromTeleportItem)
                {
                    if (toItem is TeleportItem toTeleportItem)
                    {
                        toTeleportItem.Position = fromTeleportItem.Position;
                    }
                    else 
                    {
                        throw new InvalidOperationException("ToItem must be TeleportItem."); 
                    }                    
                }

                switch (Item.Parent)
                {
                    case Tile tile:

                        return Context.AddCommand(new TileReplaceItemCommand(tile, Item, toItem) ).Then( () =>
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

                        } ).Then( () =>
                        {
                            return Promise.FromResult(toItem);
                        } );

                    case Inventory inventory:

                        return Context.AddCommand(new InventoryReplaceItemCommand(inventory, Item, toItem) ).Then( () =>
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

                        } ).Then( () =>
                        {
                            return Promise.FromResult(toItem);
                        } );

                    case Container container:

                        return Context.AddCommand(new ContainerReplaceItemCommand(container, Item, toItem) ).Then( () =>
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

                        } ).Then( () =>
                        {
                            return Promise.FromResult(toItem);
                        } );

                    default:

                        throw new NotImplementedException();
                }
            }

            return Promise.FromResult(toItem);
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