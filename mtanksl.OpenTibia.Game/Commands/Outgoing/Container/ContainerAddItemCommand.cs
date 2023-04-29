using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerAddItemCommand : Command
    {
        public ContainerAddItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            byte index = Container.AddContent(Item);

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        Context.AddPacket(observer.Client.Connection, new ContainerAddOutgoingPacket(pair.Key, Item) );
                    }
                }

                Context.AddEvent(observer, new ContainerAddItemEventArgs(Container, Item, index) );
            }

            Context.AddEvent(new ContainerAddItemEventArgs(Container, Item, index) );

            return Promise.Completed;
        }
    }
}