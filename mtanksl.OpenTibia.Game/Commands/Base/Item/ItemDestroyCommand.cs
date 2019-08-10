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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            HashSet<Player> isNextFrom = new HashSet<Player>();

            switch (Item.Container)
            {
                case Tile tile:

                    foreach (var observer in server.Map.GetPlayers() )
                    {
                        if (observer.Tile.Position.IsNextTo(tile.Position) )
                        {
                            isNextFrom.Add(observer);
                        }
                    }

                    new TileRemoveItemCommand(tile, Item).Execute(server, context);

                    break;

                case Inventory inventory:

                    isNextFrom.Add(inventory.Player);

                    new InventoryRemoveItemCommand(inventory, Item).Execute(server, context);

                    break;

                case Container container:

                    switch (container.GetRootContainer() )
                    {
                        case Tile tile:

                            foreach (var observer in server.Map.GetPlayers() )
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

                    new ContainerRemoveItemCommand(container, Item).Execute(server, context);

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

                            context.Write(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }                           
                    }
                }
            }

            //Notify

            base.Execute(server, context);            
        }
    }
}