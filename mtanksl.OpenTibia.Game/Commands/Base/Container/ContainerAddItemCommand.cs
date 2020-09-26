using OpenTibia.Common.Objects;
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

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            byte toIndex = Container.AddContent(Item);

            //Notify

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

            base.Execute(context);
        }
    }
}