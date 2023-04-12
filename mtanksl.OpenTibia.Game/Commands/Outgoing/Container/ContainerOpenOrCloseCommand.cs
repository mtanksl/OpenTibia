using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ContainerOpenOrCloseCommand : Command
    {
        public ContainerOpenOrCloseCommand(Player player, Container container)
        {
            Player = player;

            Container = container;
        }

        public Player Player { get; set; }

        public Container Container { get; set; }

        public override Promise Execute()
        {
            bool open = true;

            foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    Player.Client.ContainerCollection.CloseContainer(pair.Key);

                    Context.AddPacket(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    open = false;
                }
            }

            if (open)
            {
                byte containerId = Player.Client.ContainerCollection.OpenContainer(Container);

                List<Item> items = new List<Item>();

                foreach (var item in Container.GetItems() )
                {
                    items.Add(item);
                }

                Context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(containerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Parent is Container, items) );
            }

            return Promise.Completed;
        }
    }
}