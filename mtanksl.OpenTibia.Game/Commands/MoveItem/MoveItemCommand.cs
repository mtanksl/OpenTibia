using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class MoveItemCommand : Command
    {
        protected void RemoveItem(Tile fromTile, Item fromItem, Server server, CommandContext context)
        {
            //Act

            byte fromIndex = (byte)fromTile.RemoveContent(fromItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(fromTile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex));
                }
            }
        }

        protected void RemoveItem(Inventory fromInventory, Item fromItem, Server server, CommandContext context)
        {
            //Act

            byte fromSlot = (byte)fromInventory.RemoveContent(fromItem);

            //Notify

            context.Write(fromInventory.Player.Client.Connection, new SlotRemoveOutgoingPacket( (Slot)fromSlot ) );
        }

        protected void RemoveItem(Container fromContainer, Item fromItem, Server server, CommandContext context)
        {
            //Act

            byte fromIndex = (byte)fromContainer.RemoveContent(fromItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                byte containerId;

                if (observer.Client.ContainerCollection.IsOpen(fromContainer, out containerId) )
                {
                    context.Write(observer.Client.Connection, new ContainerRemoveOutgoingPacket(containerId, fromIndex) );
                }
            }
        }
        
        protected void AddItem(Tile toTile, Item fromItem, Server server, CommandContext context)
        {
            //Act

            byte toIndex = (byte)toTile.AddContent(fromItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(toTile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toTile.Position, toIndex, fromItem));
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

            byte toIndex = (byte)toContainer.AddContent(fromItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                byte containerId;

                if (observer.Client.ContainerCollection.IsOpen(toContainer, out containerId) )
                {
                    context.Write(observer.Client.Connection, new ContainerAddOutgoingPacket(containerId, fromItem) );
                }
            }
        }
    }
}