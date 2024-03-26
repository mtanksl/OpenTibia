using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ItemTransformContainerCloseHandler : CommandResultHandler<Item, ItemTransformCommand>
    {  
        public override PromiseResult<Item> Handle(Func<PromiseResult<Item>> next, ItemTransformCommand command)
        {
            if (command.FromItem is Container)
            {
                return next().Then( (item) =>
                {
                    if (item is Container container)
                    {
                        CloseContainer(container);
                    }
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