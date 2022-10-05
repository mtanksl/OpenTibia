using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemMoveToInventoryContainerCloseHandler : CommandHandler<ItemUpdateParentToInventoryCommand>
    {
        public override bool CanHandle(Context context, ItemUpdateParentToInventoryCommand command)
        {
            if (command.Item is Container && !command.Data.ContainsKey("ItemMoveToInventoryContainerCloseHandler") )
            {
                command.Data.Add("ItemMoveToInventoryContainerCloseHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemUpdateParentToInventoryCommand command)
        {
            HashSet<Player> isNextFrom = new HashSet<Player>();

            HashSet<Player> isNextTo = new HashSet<Player>();

            Container container = (Container)command.Item;

            switch (container.Parent)
            {
                case Tile fromTile:

                    foreach (var observer in context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer.Tile.Position.IsNextTo(fromTile.Position) )
                        {
                            isNextFrom.Add(observer);
                        }                            
                    }

                    break;

                case Inventory fromInventory:

                    isNextFrom.Add(fromInventory.Player);

                    break;

                case Container fromContainer:

                    switch (fromContainer.Root() )
                    {
                        case Tile fromTile:

                            foreach (var observer in context.Server.GameObjects.GetPlayers() )
                            {
                                if (observer.Tile.Position.IsNextTo(fromTile.Position) )
                                {
                                    isNextFrom.Add(observer);
                                }
                            }

                            break;

                        case Inventory fromInventory:

                            isNextFrom.Add(fromInventory.Player);

                            break;
                    }

                    break;
            }

            isNextTo.Add(command.ToInventory.Player);

            context.AddCommand(command).Then(ctx =>
            {
                foreach (var observer in isNextFrom.Except(isNextTo) )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value.IsContentOf(container) )
                        {
                            observer.Client.ContainerCollection.CloseContainer(pair.Key);

                            context.AddPacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }

                foreach (var observer in isNextFrom.Intersect(isNextTo) )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value == container)
                        {
                            var items = new List<Item>();

                            foreach (var item in container.GetItems() )
                            {
                                items.Add(item);
                            }

                            context.AddPacket(observer.Client.Connection, new OpenContainerOutgoingPacket(pair.Key, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity, container.Parent is Container, items) );
                        }                           
                    }
                }

                OnComplete(ctx);
            } );
        }
    }
}