using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : CommandResult<Item>
    {
        public ItemTransformCommand(Item item, ushort openTibiaId, byte typeCount)
        {
            Item = item;

            OpenTibiaId = openTibiaId;

            TypeCount = typeCount;
        }

        public Item Item { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte TypeCount { get; set; }

        public override PromiseResult<Item> Execute()
        {
            Item toItem = Context.Server.ItemFactory.Create(OpenTibiaId, TypeCount);

            if (toItem != null)
            {
                toItem.ActionId = Item.ActionId;

                toItem.UniqueId = Item.UniqueId;

                if (Item.Charges > 0)
                {
                    toItem.Charges = Item.Charges;
                }

                if (Item.DurationInMilliseconds > 0)
                {
                    toItem.DurationInMilliseconds = Item.DurationInMilliseconds;
                }

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

                        toReadableItem.WrittenBy = fromReadableItem.WrittenBy;

                        toReadableItem.WrittenDate = fromReadableItem.WrittenDate;
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

                Context.Server.ItemFactory.Attach(toItem);

                switch (Item.Parent)
                {
                    case Tile tile:
                        
                        if (Detach(Context, Item) )
                        {
                            return Context.AddCommand(new TileReplaceItemCommand(tile, Item, toItem) ).Then( () =>
                            {                            
                                Context.Server.QueueForExecution( () =>
                                {
                                    ClearComponentsAndEventHandlers(Context, Item);

                                    return Promise.Completed;
                                } );
                            
                                return Promise.FromResult(toItem);
                            } );
                        }

                        break;

                    case Inventory inventory:

                        if (Detach(Context, Item) )
                        {
                            return Context.AddCommand(new InventoryReplaceItemCommand(inventory, Item, toItem) ).Then( () =>
                            {                            
                                Context.Server.QueueForExecution( () =>
                                {
                                    ClearComponentsAndEventHandlers(Context, Item);

                                    return Promise.Completed;
                                } );

                                return Promise.FromResult(toItem);
                            } );
                        }

                        break;

                    case Container container:

                        if (Detach(Context, Item) )
                        {
                            return Context.AddCommand(new ContainerReplaceItemCommand(container, Item, toItem) ).Then( () =>
                            {                           
                                Context.Server.QueueForExecution( () =>
                                {
                                    ClearComponentsAndEventHandlers(Context, Item);

                                    return Promise.Completed;
                                } );

                                return Promise.FromResult(toItem);
                            } );
                        }

                        break;

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