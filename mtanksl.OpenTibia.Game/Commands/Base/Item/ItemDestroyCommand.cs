using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        public override void Execute(Server server, CommandContext context)
        {
            Position fromPosition = null;

            switch (Item.Container)
            {
                case Tile tile:

                    fromPosition = tile.Position;

                    new TileRemoveItemCommand(tile, tile.GetIndex(Item) ).Execute(server, context);

                    break;

                case Inventory inventory:

                    fromPosition = inventory.Player.Tile.Position;

                    new InventoryRemoveItemCommand(inventory, inventory.GetIndex(Item) ).Execute(server, context);

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

                    new ContainerRemoveItemCommand(container, container.GetIndex(Item) ).Execute(server, context);

                    break;
            }

            if (Item is Container backpack)
            {
                foreach (var observer in server.Map.GetPlayers() )
                {
                    if ( observer.Tile.Position.IsNextTo(fromPosition) )
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

            base.Execute(server, context);            
        }
    }
}
