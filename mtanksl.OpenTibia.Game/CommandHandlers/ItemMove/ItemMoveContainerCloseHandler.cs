using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemMoveContainerCloseHandler : CommandHandler<ItemMoveCommand>
    {
        public override Promise Handle(Func<Promise> next, ItemMoveCommand command)
        {
            if (command.Item is Container container)
            {
                HashSet<Player> isNextFrom = new HashSet<Player>();

                switch (container.Parent)
                {
                    case Tile fromTile:

                        foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(fromTile.Position) )
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

                                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(fromTile.Position) )
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

                            case Safe fromSafe:

                                isNextFrom.Add(fromSafe.Player);

                                break;
                        }

                        break;
                }

                return next().Then( () =>
                {
                    HashSet<Player> isNextTo = new HashSet<Player>();

                    switch (command.Item.Parent)
                    {
                        case Tile toTile:

                            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(toTile.Position) )
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

                                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(toTile.Position) )
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

                                case Safe toSafe:

                                    isNextTo.Add(toSafe.Player);

                                    break;
                            }

                            break;
                    }

                    CloseContainer(container, isNextFrom, isNextTo);

                    UpdateContainer(container, isNextFrom, isNextTo);

                    return Promise.Completed;
                } );
            }

            return next();
        }

        private void CloseContainer(Container container, HashSet<Player> isNextFrom, HashSet<Player> isNextTo)
        {
            foreach (var observer in isNextFrom.Except(isNextTo) )
            {
                foreach (var pair in observer.Client.Containers.GetIndexedContainers() )
                {
                    if (pair.Value.IsContentOf(container) )
                    {
                        observer.Client.Containers.CloseContainer(pair.Key);

                        Context.AddPacket(observer, new CloseContainerOutgoingPacket(pair.Key) );
                    }
                }
            }
        }

        private void UpdateContainer(Container container, HashSet<Player> isNextFrom, HashSet<Player> isNextTo)
        {
            foreach (var observer in isNextFrom.Intersect(isNextTo) )
            {
                foreach (var pair in observer.Client.Containers.GetIndexedContainers() )
                {
                    if (pair.Value == container)
                    {
                        //TODO: FeatureFlag.ContainerPagination

                        Context.AddPacket(observer, new OpenContainerOutgoingPacket(pair.Key, container, container.Metadata.Name, container.Metadata.Capacity.Value, container.Parent is Container, true, false, 0, container.GetItems().ToList() ) );
                    }
                }
            }
        }
    }
}