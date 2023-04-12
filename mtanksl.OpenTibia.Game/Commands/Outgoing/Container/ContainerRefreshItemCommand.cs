using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerRefreshItemCommand : Command
    {
        public ContainerRefreshItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }
        
        public override Promise Execute()
        {
            byte index = Container.GetIndex(Item);

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        Context.AddPacket(observer.Client.Connection, new ContainerUpdateOutgoingPacket(pair.Key, index, Item) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}