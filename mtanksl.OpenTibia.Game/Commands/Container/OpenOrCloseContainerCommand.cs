using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class OpenOrCloseContainerCommand : Command
    {
        public OpenOrCloseContainerCommand(Player player, Container container)
        {
            Player = player;

            Container = container;
        }

        public Player Player { get; set; }

        public Container Container { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            bool open = true;

            foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    //Act

                    Player.Client.ContainerCollection.CloseContainer(pair.Key);

                    //Notify

                    context.Write(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    open = false;
                }
            }

            if (open)
            {
                //Act

                byte containerId = Player.Client.ContainerCollection.OpenContainer(Container);

                //Notify

                var items = new List<Item>();

                foreach (var item in Container.GetItems() )
                {
                    items.Add(item);
                }

                context.Write(Player.Client.Connection, new OpenContainerOutgoingPacket(containerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Container is Container, items) );
            }

            base.Execute(server, context);
        }
    }
}