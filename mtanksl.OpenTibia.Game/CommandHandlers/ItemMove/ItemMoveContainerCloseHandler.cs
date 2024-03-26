using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
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

                foreach (var player in container.GetPlayers() )
                {
                    isNextFrom.Add(player);
                }

                return next().Then( (Action)(() =>
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

                                case LockerCollection toSafe:

                                    isNextTo.Add((Player)toSafe.Player);

                                    break;
                            }

                            break;
                    }

                    CloseContainer(container, isNextFrom, isNextTo);

                    UpdateContainer(container, isNextFrom, isNextTo);
                }) );
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
                        Context.AddPacket(observer, new OpenContainerOutgoingPacket(pair.Key, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity.Value, container.Parent is Container, container.GetItems().ToList() ) );
                    }
                }
            }
        }
    }
}