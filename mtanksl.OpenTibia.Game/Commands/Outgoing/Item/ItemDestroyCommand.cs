using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

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
            switch (Item.Parent)
            {
                case Tile tile:

                    return Context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then( () =>
                    {
                        Destroy(Item);

                        return Promise.Completed;
                    } );

                case Inventory inventory:

                    return Context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then( () =>
                    {
                        Destroy(Item);

                        return Promise.Completed;
                    } );

                case Container container:

                    return Context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then( () =>
                    {
                        Destroy(Item);

                        return Promise.Completed;
                    } );
            }

            return Promise.Completed;
        }

        private void Destroy(Item item)
        {
            if (item is Container container)
            {
                foreach (var child in container.GetItems() )
                {
                    Destroy(child);
                }

                foreach (var observer in container.GetPlayers() )
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
            }

            Context.Server.ItemFactory.Destroy(item);
        }
    }
}