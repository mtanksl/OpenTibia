using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ItemDestroyCommand : Command
    {
        public ItemDestroyCommand(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            HashSet<Player> isNextFrom = new HashSet<Player>();

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

            if (Item is Container bag)
            {
                foreach (var observer in isNextFrom)
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
            }

            base.OnCompleted(context);            
        }
    }
}