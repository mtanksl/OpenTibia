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
        
        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                byte index = Container.GetIndex(FromItem);

                Container.ReplaceContent(index, ToItem);

                foreach (var observer in Container.GetPlayers() )
                {
                    foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value == Container)
                        {
                            Context.AddPacket(observer.Client.Connection, new ContainerUpdateOutgoingPacket(pair.Key, index, ToItem) );
                        }
                    }
                }

                resolve();
            } );
        }
    }
}