using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ContainerReplaceOrCloseCommand : Command
    {
        public ContainerReplaceOrCloseCommand(Player player, Container container, byte containerId)
        {
            Player = player;

            ContainerId = containerId;

            Container = container;
        }

        public Player Player { get; set; }

        public Container Container { get; set; }

        public byte ContainerId { get; set; }

        public override Promise Execute()
        {
            bool replace = true;

            foreach (var pair in Player.Client.Containers.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    Player.Client.Containers.CloseContainer(pair.Key);

                    Context.AddPacket(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    replace = false;
                }
            }

            if (replace)
            {
                Player.Client.Containers.ReplaceContainer(Container, ContainerId);

                List<Item> items = new List<Item>();

                foreach (var item in Container.GetItems() )
                {
                    items.Add(item);
                }

                Context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Parent is Container, items) );
            }

            return Promise.Completed;
        }
    }
}