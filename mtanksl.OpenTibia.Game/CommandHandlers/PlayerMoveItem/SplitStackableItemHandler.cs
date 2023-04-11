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
                if (toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || (toTile.GetCreatures().Any(c => c.Block) && command.Item.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
                {
                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );
                
                    return Promise.Stop();
                }
                else
                {
                    if (command.Item is StackableItem fromStackableItem)
                    {
                        if (toTile.TopItem is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                        {
                            if (toStackableItem.Count + command.Count > 100)
                            {
                                Context.AddCommand(new TileCreateItemCommand(toTile, fromStackableItem.Metadata.OpenTibiaId, (byte)(toStackableItem.Count + command.Count - 100) ) );

                                Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                                Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                            }
                            else
                            {
                                Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) );

                                Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                            }
                        }
                        else
                        {
                            if (fromStackableItem.Count == command.Count)
                            {
                                return next();
                            }
                            else
                            {
                                Context.AddCommand(new TileCreateItemCommand(toTile, fromStackableItem.Metadata.OpenTibiaId, command.Count) );

                                Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                            }
                        }
                    }
                    else
                    {
                        return next();
                    }
                }
            }
            else if (command.ToContainer is Inventory toInventory)
            {
                IContent toContent = toInventory.GetContent(command.ToIndex);

                if (toContent is Container toContainer2)
                {
                    return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toContainer2, 254, command.Count, false) );
                }
                else
                {
                    if (command.Item is StackableItem fromStackableItem)
                    {
                        if (toContent is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                        {
                            if (toStackableItem.Count + command.Count > 100)
                            {
                                byte count = (byte)(100 - toStackableItem.Count);

                                Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                                Context.AddCommand(new ItemDecrementCommand(fromStackableItem, count) );
                            }
                            else
                            {
                                Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) );

                                Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                            }
                        }
                        else
                        {
                            if (toContent == null)
                            {
                                if (fromStackableItem.Count == command.Count)
                                {
                                    return next();
                                }
                                else
                                {
                                    Context.AddCommand(new InventoryCreateItemCommand(toInventory, command.ToIndex, fromStackableItem.Metadata.OpenTibiaId, command.Count) );

                                    Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                }
                            }
                            else
                            {
                                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                        
                                return Promise.Stop();
                            }
                        }
                    }
                    else
                    {
                        if (toContent == null)
                        {
                            return next();
                        }
                        else
                        {
                            Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                        
                            return Promise.Stop();
                        }
                    }
                }
            }
            else if (command.ToContainer is Container toContainer)
            {
                IContent toContent = toContainer.GetContent(command.ToIndex);

                if (toContent is Container toContainer2)
                {
                    return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toContainer2, 254, command.Count, false) );
                }
                else
                {
                    if (command.Item is StackableItem fromStackableItem)
                    {
                        if (toContent is StackableItem toStackableItem && toStackableItem.Metadata.OpenTibiaId == fromStackableItem.Metadata.OpenTibiaId)
                        {
                            if (toStackableItem.Count + command.Count > 100)
                            {
                                if (toContainer.Count < toContainer.Metadata.Capacity)
                                {
                                    Context.AddCommand(new ContainerCreateItemCommand(toContainer, fromStackableItem.Metadata.OpenTibiaId, (byte)(toStackableItem.Count + command.Count - 100) ) );

                                    Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                                    Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                }
                                else
                                {
                                    byte count = (byte)(100 - toStackableItem.Count);

                                    Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) );

                                    Context.AddCommand(new ItemDecrementCommand(fromStackableItem, count) );
                                }
                            }
                            else
                            {
                                Context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(toStackableItem.Count + command.Count) ) );

                                Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                            }
                        }
                        else
                        {
                            if (toContainer.Count < toContainer.Metadata.Capacity)
                            {
                                if (fromStackableItem.Count == command.Count)
                                {
                                    return next();
                                }
                                else
                                {
                                    Context.AddCommand(new ContainerCreateItemCommand(toContainer, fromStackableItem.Metadata.OpenTibiaId, command.Count) );

                                    Context.AddCommand(new ItemDecrementCommand(fromStackableItem, command.Count) );
                                }
                            }
                            else
                            {
                                Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                    
                                return Promise.Stop();
                            }
                        }
                    }
                    else
                    {
                        if (toContainer.Count < toContainer.Metadata.Capacity)
                        {
                            return next();
                        }
                        else
                        {
                            Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );
                    
                            return Promise.Stop();
                        }
                    }
                }
            }

            return Promise.Completed();
        }
    }
}