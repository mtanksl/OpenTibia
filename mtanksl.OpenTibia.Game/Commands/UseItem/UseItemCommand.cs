using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemCommand : Command
    {
        protected void OpenOrCloseContainer(Player player, Container container, Server server, CommandContext context)
        {
            bool open = true;

            foreach (var pair in player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == container)
                {
                    //Act

                    player.Client.ContainerCollection.CloseContainer(pair.Key);

                    //Notify

                    context.Write(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    open = false;
                }
            }

            if (open)
            {
                //Act

                byte containerId = player.Client.ContainerCollection.OpenContainer(container);

                //Notify

                var items = new List<Item>();

                foreach (var item in container.GetItems() )
                {
                    items.Add(item);
                }

                context.Write(player.Client.Connection, new OpenContainerOutgoingPacket(containerId, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity, container.Container is Container, items) );
            }
        }

        protected void ReplaceOrCloseContainer(Player player, byte containerId, Container container, Server server, CommandContext context)
        {
            bool replace = true;

            foreach (var pair in player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == container)
                {
                    //Act

                    player.Client.ContainerCollection.CloseContainer(pair.Key);

                    //Notify

                    context.Write(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    replace = false;
                }
            }

            if (replace)
            {
                //Act

                player.Client.ContainerCollection.OpenContainer(containerId, container);

                //Notify

                var items = new List<Item>();

                foreach (var item in container.GetItems() )
                {
                    items.Add(item);
                }

                context.Write(player.Client.Connection, new OpenContainerOutgoingPacket(containerId, container.Metadata.TibiaId, container.Metadata.Name, container.Metadata.Capacity, container.Container is Container, items) );
            }
        }
    }
}