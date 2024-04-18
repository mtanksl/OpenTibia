using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemDestroyContainerCloseHandler : CommandHandler<ItemDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, ItemDestroyCommand command)
        {
            if (command.Item is Container container)
            {
                return next().Then( () =>
                {
                    CloseContainer(container);
                } );
            }

            return next();
        }

        private void CloseContainer(Container container)
        {
            foreach (var observer in container.GetPlayers() )
            {
                foreach (var pair in observer.Client.Containers.GetIndexedContainers() )
                {
                    if (pair.Value == container)
                    {
                        observer.Client.Containers.CloseContainer(pair.Key);

                        Context.AddPacket(observer, new CloseContainerOutgoingPacket(pair.Key) );
                    }
                }
            }

            foreach (var child in container.GetItems() )
            {
                if (child is Container container2)
                {
                    CloseContainer(container2);
                }
            }
        }
    }
}