using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class SplitStackableItemHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile) 
            {
                // Move an item to tile

                if (toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || (toTile.GetCreatures().Any(c => c.Block) && command.Item.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );
                
                    return Promise.Break;
                }
                else
                {
                    if (command.Item is StackableItem fromStackableItem)
                    {
                        // If the source is stackable

                        if (toTile.TopItem is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                        {
                            // And the destination is stackable, then do the math

                            if (toStackableItem.Count + command.Count > 100)
                            {
                                return Context.AddCommand(new TileCreateItemCommand(toTile, fromStackableItem.Metadata.OpenTibiaId, (byte)(toStackableItem.Count + command.Count - 100) ) ).Then( (item) =>
                                {
                                    return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                                } ).Then( () =>
                                {
                                    return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                } );
                            }
                            else
                            {
                                return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) ).Then( () =>
                                {
                                    return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                } );
                            }
                        }
                        else
                        {
                            // Otherwise, move the selected amount

                            if (fromStackableItem.Count == command.Count)
                            {
                                return next();
                            }
                            else
                            {
                                return Context.AddCommand(new TileCreateItemCommand(toTile, fromStackableItem.Metadata.OpenTibiaId, command.Count) ).Then( (item) =>
                                {
                                    return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                } );
                            }
                        }
                    }
                    else
                    {
                        // Otherwise, move all

                        return next();
                    }
                }
            }
            else if (command.ToContainer is Inventory toInventory) 
            {
                // Move an item to inventory

                IContent toContent = toInventory.GetContent(command.ToIndex);

                if (toContent is Container toContainer2)
                {
                    // if it is already occupied by a container, move an item to container

                    return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toContainer2, 254, command.Count, false) );
                }
                else
                {
                    if (command.Item is StackableItem fromStackableItem)
                    {
                        // If the source is stackable

                        if (toContent is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                        {
                            // And the destination is stackable, then do the math

                            if (toStackableItem.Count + command.Count > 100)
                            {
                                byte count = (byte)(100 - toStackableItem.Count);

                                return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) ).Then( () =>
                                {
                                    return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, count) );
                                } );
                            }
                            else
                            {
                                return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) ).Then( () =>
                                {
                                    return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                } );
                            }
                        }
                        else
                        {
                            // Otherwise, move the selected amount

                            if (toContent == null)
                            {
                                if (fromStackableItem.Count == command.Count)
                                {
                                    return next();
                                }
                                else
                                {
                                    return Context.AddCommand(new InventoryCreateItemCommand(toInventory, command.ToIndex, fromStackableItem.Metadata.OpenTibiaId, command.Count) ).Then( (item) =>
                                    {
                                        return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                    } );
                                }
                            }
                            else
                            {
                                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                        
                                return Promise.Break;
                            }
                        }
                    }
                    else
                    {
                        // Otherwise, move all

                        if (toContent == null)
                        {
                            return next();
                        }
                        else
                        {
                            Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                        
                            return Promise.Break;
                        }
                    }
                }
            }
            else if (command.ToContainer is Container toContainer) 
            {
                // Move an item to container

                IContent toContent = toContainer.GetContent(command.ToIndex);

                if (toContent is Container toContainer2)
                {
                    // if it is already occupied by a container, move an item to container

                    return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toContainer2, 254, command.Count, false) );
                }
                else
                {
                    if (command.Item is StackableItem fromStackableItem)
                    {
                        // If the source is stackable

                        if (toContent is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                        {
                            // And the destination is stackable, then do the math

                            if (toStackableItem.Count + command.Count > 100)
                            {
                                if (toContainer.Count < toContainer.Metadata.Capacity)
                                {
                                    return Context.AddCommand(new ContainerCreateItemCommand(toContainer, fromStackableItem.Metadata.OpenTibiaId, (byte)(toStackableItem.Count + command.Count - 100) ) ).Then( (item) =>
                                    {
                                        return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                                    } ).Then( () =>
                                    {
                                        return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                    } );
                                }
                                else
                                {
                                    byte count = (byte)(100 - toStackableItem.Count);

                                    return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) ).Then( () =>
                                    {
                                        return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, count) );
                                    } );
                                }
                            }
                            else
                            {
                                return Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) ).Then( () =>
                                {
                                    return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                } );
                            }
                        }
                        else
                        {
                            // Otherwise, move the selected amount

                            if (toContainer.Count < toContainer.Metadata.Capacity)
                            {
                                if (fromStackableItem.Count == command.Count)
                                {
                                    return next();
                                }
                                else
                                {
                                    return Context.AddCommand(new ContainerCreateItemCommand(toContainer, fromStackableItem.Metadata.OpenTibiaId, command.Count) ).Then( (item) =>
                                    {
                                        return Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                    } );
                                }
                            }
                            else
                            {
                                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                    
                                return Promise.Break;
                            }
                        }
                    }
                    else
                    {
                        // Otherwise, move all

                        if (toContainer.Count < toContainer.Metadata.Capacity)
                        {
                            return next();
                        }
                        else
                        {
                            Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                    
                            return Promise.Break;
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}