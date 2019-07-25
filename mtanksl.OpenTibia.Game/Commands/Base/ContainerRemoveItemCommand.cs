using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerRemoveItemCommand : Command
    {
        public ContainerRemoveItemCommand(Container container, byte index)
        {
            Container = container;

            Index = index;
        }

        public Container Container { get; set; }

        public byte Index { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Act

            Container.RemoveContent(Index);

            //Notify

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        context.Write(observer.Client.Connection, new ContainerRemoveOutgoingPacket(pair.Key, Index) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}