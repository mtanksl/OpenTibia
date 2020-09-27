using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ItemMoveCommand : Command
    {
        public ItemMoveCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            if ( !context.Server.Scripts.ItemMoveScripts.Any(script => script.OnItemMove(Player, Item, ToContainer, ToIndex, Count, context) ) )
            {
                HashSet<Player> isNextFrom = new HashSet<Player>();

                HashSet<Player> isNextTo = new HashSet<Player>();

                switch (Item.Container)
                {
                    case Tile tile:

                        foreach (var observer in context.Server.GameObjects.GetPlayers() )
                        {
                            if (observer.Tile.Position.IsNextTo(tile.Position) )
                            {
                                isNextFrom.Add(observer);
                            }
                        }

                        new TileRemoveItemCommand(tile, Item).Execute(context);

                        break;

                    case Inventory inventory:

                        isNextFrom.Add(inventory.Player);

                        new InventoryRemoveItemCommand(inventory, Item).Execute(context);

                        break;

                    case Container container:

                        switch (container.GetRootContainer() )
                        {
                            case Tile tile:

                                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                                {
                                    if (observer.Tile.Position.IsNextTo(tile.Position) )
                                    {
                                        isNextFrom.Add(observer);
                                    }
                                }

                                break;

                            case Inventory inventory:

                                isNextFrom.Add(inventory.Player);

                                break;
                        }

                        new ContainerRemoveItemCommand(container, Item).Execute(context);

                        break;
                }

                switch (ToContainer)
                {
                    case Tile tile:

                        foreach (var observer in context.Server.GameObjects.GetPlayers() )
                        {
                            if (observer.Tile.Position.IsNextTo(tile.Position) )
                            {
                                isNextTo.Add(observer);
                            }
                        }

                        new TileAddItemCommand(tile, Item).Execute(context);

                        break;

                    case Inventory inventory:

                        isNextTo.Add(inventory.Player);

                        new InventoryAddItemCommand(inventory, ToIndex, Item).Execute(context);

                        break;

                    case Container container:

                        switch (container.GetRootContainer() )
                        {
                            case Tile tile:

                                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                                {
                                    if (observer.Tile.Position.IsNextTo(tile.Position) )
                                    {
                                        isNextTo.Add(observer);
                                    }
                                }

                                break;

                            case Inventory inventory:

                                isNextTo.Add(inventory.Player);

                                break;
                        }

                        new ContainerAddItemCommand(container, Item).Execute(context);

                        break;
                }

                if (Item is Container bag)
                {
                    foreach (var observer in isNextFrom.Except(isNextTo) )
                    {
                        foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                        {
                            if (pair.Value.IsChild(Item) )
                            {
                                observer.Client.ContainerCollection.CloseContainer(pair.Key);

                                context.WritePacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                            }                           
                        }
                    }

                    foreach (var observer in isNextFrom.Intersect(isNextTo) )
                    {
                        foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                        {
                            if (pair.Value == bag)
                            {
                                observer.Client.ContainerCollection.ReplaceContainer(pair.Key, bag);

                                var items = new List<Item>();

                                foreach (var item in bag.GetItems() )
                                {
                                    items.Add(item);
                                }

                                context.WritePacket(observer.Client.Connection, new OpenContainerOutgoingPacket(pair.Key, bag.Metadata.TibiaId, bag.Metadata.Name, bag.Metadata.Capacity, bag.Container is Container, items) );
                            }                           
                        }
                    }
                }
            }

            base.OnCompleted(context);
        }
    }
}