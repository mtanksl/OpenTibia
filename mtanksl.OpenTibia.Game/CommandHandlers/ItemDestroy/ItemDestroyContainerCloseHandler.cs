using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemDestroyContainerCloseHandler : CommandHandler<ItemDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, ItemDestroyCommand command)
        {
            return next().Then( () =>
            {
                Close(command.Item);

                return Promise.Completed;
            } );
        }

        private void Close(Item item)
        {
            if (item is Container container)
            {
                foreach (var observer in container.GetPlayers() )
                {
                    foreach (var pair in observer.Client.Containers.GetIndexedContainers() )
                    {
                        if (pair.Value == container)
                        {
                            observer.Client.Containers.CloseContainer(pair.Key);

                            Context.AddPacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                }

                foreach (var child in container.GetItems() )
                {
                    Close(child);
                }
            }
        }
    }
}