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
                

                return Promise.Completed;
            } );
        }

        private void Close(Item parent)
        {
            if (parent is Container container)
            {
                foreach (var observer in container.GetPlayers() )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers())
                    {
                        if (pair.Value == container)
                        {
                            observer.Client.ContainerCollection.CloseContainer(pair.Key);

                            Context.AddPacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key));
                        }
                    }
                }
            }
        }
    }
}