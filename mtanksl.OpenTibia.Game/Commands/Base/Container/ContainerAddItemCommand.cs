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

        public override void Execute(Server server, CommandContext context)
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
                        context.Write(observer.Client.Connection, new ContainerAddOutgoingPacket(pair.Key, Item) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}