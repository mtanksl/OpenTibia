using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class MoveItemCommand : Command
    {
        protected void RemoveItem(Tile fromTile, byte fromIndex, Server server, CommandContext context)
        {
            //Act

            fromTile.RemoveContent(fromIndex);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(fromTile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );
                }
            }
        }

        protected void RemoveItem(Inventory fromInventory, byte fromSlot, Server server, CommandContext context)
        {
            //Act

            fromInventory.RemoveContent(fromSlot);

            //Notify

            context.Write(fromInventory.Player.Client.Connection, new SlotRemoveOutgoingPacket( (Slot)fromSlot ) );
        }

        protected void RemoveItem(Container fromContainer, byte fromIndex, Server server, CommandContext context)
        {
            //Act

            fromContainer.RemoveContent(fromIndex);

            //Notify

            foreach (var observer in fromContainer.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == fromContainer)
                    {
                        context.Write(observer.Client.Connection, new ContainerRemoveOutgoingPacket(pair.Key, fromIndex) );
                    }
                }
            }
        }
        
        protected void AddItem(Tile toTile, Item fromItem, Server server, CommandContext context)
        {
            //Act

            byte toIndex = toTile.AddContent(fromItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(toTile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toTile.Position, toIndex, fromItem) );
                }
            }
        }

        protected void AddItem(Inventory toInventory, byte toSlot, Item fromItem, Server server, CommandContext context)
        {
            //Act

            toInventory.AddContent(toSlot, fromItem);

            //Notify

            context.Write(toInventory.Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)toSlot, fromItem ) );
        }

        protected void AddItem(Container toContainer, Item fromItem, Server server, CommandContext context)
        {
            //Act

            byte toIndex = toContainer.AddContent(fromItem);

            //Notify

            foreach (var observer in toContainer.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == toContainer)
                    {
                        context.Write(observer.Client.Connection, new ContainerAddOutgoingPacket(pair.Key, fromItem) );
                    }
                }
            }
        }

        protected void CloseContainer(Tile fromTile, Tile toTile, Container container, Server server, CommandContext context)
        {
            foreach (var observer in server.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.IsNextTo(fromTile.Position) && !observer.Tile.Position.IsNextTo(toTile.Position) )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if ( pair.Value.IsChildOfParent(container) )
                        {
                            //Act

                            observer.Client.ContainerCollection.CloseContainer(pair.Key);

                            //Notify

                            context.Write(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }
            }
        }

        protected void CloseContainer(Tile fromTile, Inventory toInventory, Container container, Server server, CommandContext context)
        {
            foreach (var observer in server.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.IsNextTo(fromTile.Position) && observer != toInventory.Player )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if ( pair.Value.IsChildOfParent(container) )
                        {
                            //Act

                            observer.Client.ContainerCollection.CloseContainer(pair.Key);

                            //Notify

                            context.Write(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }
            }
        }

        protected void CloseContainer(Inventory fromInventory, Tile toTile, Container container, Server server, CommandContext context)
        {
            if ( !fromInventory.Player.Tile.Position.IsNextTo(toTile.Position) )
            {
                foreach (var pair in fromInventory.Player.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if ( pair.Value.IsChildOfParent(container) )
                    {
                        //Act

                        fromInventory.Player.Client.ContainerCollection.CloseContainer(pair.Key);

                        //Notify

                        context.Write(fromInventory.Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                    }
                }
            }
        }

        protected void CloseContainer(Inventory fromInventory, Inventory toInventory, Container container, Server server, CommandContext context)
        {
            
        }

        protected void ShowOrHideOpenParentContainer(Container container, Server server, CommandContext context)
        {
            foreach (var observer in container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == container)
                    {
                        //Notify

                        var items = new List<Item>();

                        foreach (var item in container.GetItems() )
                        {
                            items.Add(item);
                        }

                        context.Write(observer.Client.Connection, new OpenContainerOutgoingPacket(pair.Key, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity, container.Container is Container, items) );
                    }
                }                 
            }
        }
    }
}