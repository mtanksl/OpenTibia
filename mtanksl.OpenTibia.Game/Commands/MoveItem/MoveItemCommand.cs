using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class MoveItemCommand : Command
    {
        protected void CloseContainer(Container toContainer, Container fromContainer, Server server, CommandContext context)
        {
            switch (toContainer.GetParent().Container)
            {
                case Tile tile:

                    CloseContainer(tile, fromContainer, server, context);

                    break;

                case Inventory inventory:

                    CloseContainer(inventory, fromContainer, server, context);

                    break;
            }
        }

        protected void CloseContainer(Tile toTile, Container fromContainer, Server server, CommandContext context)
        {
            foreach (var observer in server.Map.GetPlayers() )
            {
                if ( !observer.Tile.Position.IsNextTo(toTile.Position) )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value.IsChildOfParent(fromContainer) )
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

        protected void CloseContainer(Inventory toInventory, Container fromContainer, Server server, CommandContext context)
        {
            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != toInventory.Player)
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value.IsChildOfParent(fromContainer) )
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

            foreach (var observer in server.Map.GetPlayers() )
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

            foreach (var observer in server.Map.GetPlayers() )
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
    }
}