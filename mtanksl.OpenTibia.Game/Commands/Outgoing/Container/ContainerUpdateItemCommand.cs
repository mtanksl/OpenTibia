using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerUpdateItemCommand : Command
    {
        public ContainerUpdateItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }
        
        public override void Execute(Context context)
        {
            byte index = Container.GetIndex(Item);

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        context.AddPacket(observer.Client.Connection, new ContainerUpdateOutgoingPacket(pair.Key, index, Item) );
                    }
                }
            }

            OnComplete(context);
        }
    }
}