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
            byte index = (byte)Container.AddContent(Item);

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.Containers.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        Context.AddPacket(observer, new ContainerAddOutgoingPacket(pair.Key, Item) );
                    }
                }
            }

            Context.AddEvent(new ContainerAddItemEventArgs(Container, Item, index) );

            return Promise.Completed;
        }
    }
}