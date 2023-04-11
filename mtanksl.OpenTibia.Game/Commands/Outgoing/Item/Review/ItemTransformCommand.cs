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
            return Promise.Run<Item>( (resolve, reject) =>
            {
                Item toItem = Context.Server.ItemFactory.Create(OpenTibiaId, Count);

                if (toItem != null)
                {
                    switch (FromItem.Parent)
                    {
                        case Tile tile:

                            Context.AddCommand(new TileReplaceItemCommand(tile, FromItem, toItem) ).Then( () =>
                            {
                                Destroy(FromItem);

                                resolve(toItem);
                            } );
                  
                            break;

                        case Inventory inventory:

                            Context.AddCommand(new InventoryReplaceItemCommand(inventory, FromItem, toItem) ).Then( () =>
                            {
                                Destroy(FromItem);

                                resolve(toItem);
                            } );
                   
                            break;

                        case Container container:

                            Context.AddCommand(new ContainerReplaceItemCommand(container, FromItem, toItem) ).Then( () =>
                            {
                                Destroy(FromItem);

                                resolve(toItem);
                            } );

                            break;                    
                    }
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