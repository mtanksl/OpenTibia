using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : CommandResult<Item>
    {
        public ItemTransformCommand(Item fromItem, ushort openTibiaId, byte count)
        {
            FromItem = fromItem;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Item FromItem { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override PromiseResult<Item> Execute()
        {
            return PromiseResult<Item>.Run( (resolve, reject) =>
            {
                Item toItem = context.Server.ItemFactory.Create(OpenTibiaId, Count);

                if (toItem != null)
                {
                    switch (FromItem.Parent)
                    {
                        case Tile tile:

                            context.AddCommand(new TileReplaceItemCommand(tile, FromItem, toItem) ).Then(ctx =>
                            {
                                Destroy(ctx, FromItem);

                                resolve(ctx, toItem);
                            } );
                  
                            break;

                        case Inventory inventory:

                            context.AddCommand(new InventoryReplaceItemCommand(inventory, FromItem, toItem) ).Then(ctx =>
                            {
                                Destroy(ctx, FromItem);

                                resolve(ctx, toItem);
                            } );
                   
                            break;

                        case Container container:

                            context.AddCommand(new ContainerReplaceItemCommand(container, FromItem, toItem) ).Then(ctx =>
                            {
                                Destroy(ctx, FromItem);

                                resolve(ctx, toItem);
                            } );

                            break;                    
                    }
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