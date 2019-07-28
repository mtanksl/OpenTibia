using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ContainerReplaceCommand : Command
    {
        public ContainerReplaceCommand(Player player, byte containerId, Container container)
        {
            Player = player;

            ContainerId = containerId;

            Container = container;
        }

        public Player Player { get; set; }

        public byte ContainerId { get; set; }

        public Container Container { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            Player.Client.ContainerCollection.ReplaceContainer(ContainerId, Container);

            //Notify

            var items = new List<Item>();

            foreach (var item in Container.GetItems() )
            {
                items.Add(item);
            }

            context.Write(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Container is Container, items) );

            base.Execute(server, context);
        }
    }
}