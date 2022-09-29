using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerRemoveItemCommand : Command
    {
        public ContainerRemoveItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            byte index = Container.GetIndex(Item);

            Container.RemoveContent(index);

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        context.AddPacket(observer.Client.Connection, new ContainerRemoveOutgoingPacket(pair.Key, index) );
                    }
                }
            }

            context.AddEvent(new ContainerRemoveItemEventArgs(Container, Item, index) );

            base.Execute(context);
        }
    }
}