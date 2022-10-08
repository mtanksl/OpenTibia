using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System;

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

        public override Promise Execute(Context context)
        {
            bool replace = true;

            foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    Player.Client.ContainerCollection.CloseContainer(pair.Key);

                    context.AddPacket(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    replace = false;
                }
            }

            if (replace)
            {
                Player.Client.ContainerCollection.ReplaceContainer(Container, ContainerId);

                List<Item> items = new List<Item>();

                foreach (var item in Container.GetItems() )
                {
                    items.Add(item);
                }

                context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Parent is Container, items) );
            }

            return Promise.FromResult(context);
        }
    }
}