using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemContainerCloseHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (command.Item is Container && !command.Data.ContainsKey("MoveItemContainerCloseHandler") )
            {
                command.Data.Add("MoveItemContainerCloseHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            context.AddCommand(command, ctx =>
            {
                HashSet<Player> isNextFrom = new HashSet<Player>();

                switch (command.FromContainer)
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

                                foreach (var observer in context.Server.GameObjects.GetPlayers())
                                {
                                    if (observer.Tile.Position.IsNextTo(fromTile.Position))
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

                HashSet<Player> isNextTo = new HashSet<Player>();

                switch (command.ToContainer)
                {
                    case Tile toTile:

                        foreach (var observer in context.Server.GameObjects.GetPlayers() )
                        {
                            if (observer.Tile.Position.IsNextTo(toTile.Position) )
                            {
                                isNextTo.Add(observer);
                            }
                        }

                        break;

                    case Inventory toInventory:

                        isNextTo.Add(toInventory.Player);

                        break;

                    case Container toContainer:

                        switch (toContainer.Root() )
                        {
                            case Tile toTile:

                                foreach (var observer in context.Server.GameObjects.GetPlayers())
                                {
                                    if (observer.Tile.Position.IsNextTo(toTile.Position))
                                    {
                                        isNextTo.Add(observer);
                                    }
                                }

                                break;

                            case Inventory toInventory:

                                isNextTo.Add(toInventory.Player);

                                break;
                        }

                        break;
                }

                Container container = (Container)command.Item;

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
                            observer.Client.ContainerCollection.ReplaceContainer(container, pair.Key);

                            var items = new List<Item>();

                            foreach (var item in container.GetItems() )
                            {
                                items.Add(item);
                            }

                            context.AddPacket(observer.Client.Connection, new OpenContainerOutgoingPacket(pair.Key, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity, container.Parent is Container, items) );
                        }                           
                    }
                }

                base.Handle(ctx, command);
            } );
        }
    }
}