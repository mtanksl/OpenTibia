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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                switch (Item.Parent)
                {
                    case Tile tile:

                        context.AddCommand(new TileRemoveItemCommand(tile, Item) ).Then(ctx =>
                        {
                            Destroy(ctx, Item);

                            resolve(ctx);
                        } );
                  
                        break;

                    case Inventory inventory:

                        context.AddCommand(new InventoryRemoveItemCommand(inventory, Item) ).Then(ctx =>
                        {
                            Destroy(ctx, Item);

                            resolve(ctx);
                        } );
                   
                        break;

                    case Container container:

                        context.AddCommand(new ContainerRemoveItemCommand(container, Item) ).Then(ctx =>
                        {
                            Destroy(ctx, Item);

                            resolve(ctx);
                        } );

                        break;
                }
            } );            
        }

        private void Destroy(Context context, Item item)
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

                            context.AddPacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }

                foreach (var child in container.GetItems() )
                {
                    Destroy(context, child);
                }
            }

            context.Server.ItemFactory.Destroy(item);
        }
    }
}