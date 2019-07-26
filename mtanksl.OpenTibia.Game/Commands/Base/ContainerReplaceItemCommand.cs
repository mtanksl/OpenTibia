using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerReplaceItemCommand : Command
    {
        public ContainerReplaceItemCommand(Container container, byte index, Item item)
        {
            Container = container;

            Index = index;

            Item = item;
        }

        public Container Container { get; set; }

        public byte Index { get; set; }

        public Item Item { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            Container.ReplaceContent(Index, Item);

            //Notify

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        context.Write(observer.Client.Connection, new ContainerUpdateOutgoingPacket(pair.Key, Index, Item) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}