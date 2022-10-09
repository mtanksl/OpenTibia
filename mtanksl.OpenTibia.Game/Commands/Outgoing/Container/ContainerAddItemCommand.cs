using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ContainerAddItemCommand : CommandResult<byte>
    {
        public ContainerAddItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }

        public override PromiseResult<byte> Execute(Context context)
        {
            return PromiseResult<byte>.Run(resolve =>
            {
                byte index = Container.AddContent(Item);

                foreach (var observer in Container.GetPlayers() )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value == Container)
                        {
                            context.AddPacket(observer.Client.Connection, new ContainerAddOutgoingPacket(pair.Key, Item) );
                        }
                    }
                }

                resolve(context, index);
            } );
        }
    }
}