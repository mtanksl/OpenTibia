using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ItemMoveCommand : Command
    {
        public ItemMoveCommand(Player player, Item fromItem, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            FromItem = fromItem;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item FromItem { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            if ( !server.Scripts.ItemMoveScripts.Any(script => script.OnItemMove(Player, FromItem, ToContainer, ToIndex, Count, server, context) ) )
            {
                //Arrange

                IContainer fromContainer = FromItem.Container;

                byte fromIndex = fromContainer.GetIndex(FromItem);

                //Act

                Position fromPosition = null;

                switch (fromContainer)
                {
                    case Tile tile:

                        fromPosition = tile.Position;

                        new TileRemoveItemCommand(tile, fromIndex).Execute(server, context);

                        break;

                    case Inventory inventory:

                        fromPosition = inventory.Player.Tile.Position;

                        new InventoryRemoveItemCommand(inventory, fromIndex).Execute(server, context);

                        break;

                    case Container container:

                        switch (container.GetRootContainer() )
                        {
                            case Tile fromTile:

                                fromPosition = fromTile.Position;

                                break;

                            case Inventory fromInventory:

                                fromPosition = fromInventory.Player.Tile.Position;

                                break;
                        }

                        new ContainerRemoveItemCommand(container, fromIndex).Execute(server, context);

                        break;
                }

                Position toPosition = null;

                Player toPlayer = null;

                switch (ToContainer)
                {
                    case Tile tile:

                        toPosition = tile.Position;

                        new TileAddItemCommand(tile, FromItem).Execute(server, context);

                        break;

                    case Inventory inventory:

                        toPosition = null;

                        toPlayer = inventory.Player;

                        new InventoryAddItemCommand(inventory, ToIndex, FromItem).Execute(server, context);

                        break;

                    case Container container:

                        switch (container.GetRootContainer() )
                        {
                            case Tile tile:

                                toPosition = tile.Position;

                                break;

                            case Inventory inventory:

                                toPosition = null;

                                toPlayer = inventory.Player;

                                break;
                        }

                        new ContainerAddItemCommand(container, FromItem).Execute(server, context);

                        break;
                }

                //Notify

                if (FromItem is Container backpack)
                {
                    foreach (var observer in server.Map.GetPlayers() )
                    {
                        if ( observer.Tile.Position.IsNextTo(fromPosition) )
                        {
                            if (toPosition == null ? observer == toPlayer : observer.Tile.Position.IsNextTo(toPosition) )
                            {
                                if ( ( (fromContainer is Container) && !(ToContainer is Container) ) || ( !(fromContainer is Container) && (ToContainer is Container) ) )
                                {
                                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                                    {
                                        if (pair.Value.IsChild(backpack) )
                                        {
                                            //Act

                                            observer.Client.ContainerCollection.ReplaceContainer(pair.Key, backpack);

                                            //Notify

                                            var items = new List<Item>();

                                            foreach (var item in backpack.GetItems() )
                                            {
                                                items.Add(item);
                                            }

                                            context.Write(observer.Client.Connection, new OpenContainerOutgoingPacket(pair.Key, backpack.Metadata.TibiaId, backpack.Metadata.Name, backpack.Metadata.Capacity, backpack.Container is Container, items) );
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                                {
                                    if (pair.Value.IsChild(backpack) )
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
                }                
            }

            base.Execute(server, context);
        }
    }
}