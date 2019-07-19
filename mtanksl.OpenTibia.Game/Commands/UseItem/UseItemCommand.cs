using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemCommand : Command
    {
        protected void OpenOrCloseContainer(Player player, Container container, Server server, CommandContext context)
        {
            byte containerId;

            if (player.Client.ContainerCollection.IsOpen(container, out containerId) )
            {
                //Act

                containerId = player.Client.ContainerCollection.Close(container);

                //Notify

                context.Write(player.Client.Connection, new CloseContainerOutgoingPacket(containerId) );
            }
            else
            {
                //Act

                containerId = player.Client.ContainerCollection.Open(container);

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