using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerReplaceItemCommand : Command
    {
        public ContainerReplaceItemCommand(Container container, Item fromItem, Item toItem)
        {
            Container = container;

            FromItem = fromItem;

            ToItem = toItem;
        }

        public Container Container { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }
        
        public override void Execute(Server server, Context context)
        {
            //Arrange

            byte index = Container.GetIndex(FromItem);

            //Act

            Container.ReplaceContent(index, ToItem);

            //Notify

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        context.Write(observer.Client.Connection, new ContainerUpdateOutgoingPacket(pair.Key, index, ToItem) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}