using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ItemDestroyCommand : Command
    {
        public ItemDestroyCommand(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                switch (Item.Parent)
                {
                    case Tile tile:

                        Context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then( () =>
                        {
                            Destroy(Item);

                            resolve();
                        } );
                  
                        break;

                    case Inventory inventory:

                        Context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then( () =>
                        {
                            Destroy(Item);

                            resolve();
                        } );
                   
                        break;

                    case Container container:

                        Context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then( () =>
                        {
                            Destroy(Item);

                            resolve();
                        } );

                        break;
                }
            } );            
        }

        private void Destroy(Item item)
        {
            if (item is Container container)
            {
                foreach (var observer in container.GetPlayers().ToList() )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value == container)
                        {
                            observer.Client.ContainerCollection.CloseContainer(pair.Key);

                            Context.AddPacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }

                foreach (var child in container.GetItems() )
                {
                    Destroy(child);
                }
            }

            Context.Server.ItemFactory.Destroy(item);
        }
    }
}