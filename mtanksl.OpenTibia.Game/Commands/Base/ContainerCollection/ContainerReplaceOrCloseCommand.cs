using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ContainerReplaceOrCloseCommand : Command
    {
        public ContainerReplaceOrCloseCommand(Player player, byte containerId, Container container)
        {
            Player = player;

            ContainerId = containerId;

            Container = container;
        }

        public Player Player { get; set; }

        public byte ContainerId { get; set; }

        public Container Container { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            bool replace = true;

            foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    //Act

                    Player.Client.ContainerCollection.CloseContainer(pair.Key);

                    //Notify

                    context.AddPacket(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    replace = false;
                }
            }

            if (replace)
            {
                //Act

                Player.Client.ContainerCollection.ReplaceContainer(ContainerId, Container);

                //Notify

                var items = new List<Item>();

                foreach (var item in Container.GetItems() )
                {
                    items.Add(item);
                }

                context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Container is Container, items) );
            }

            base.Execute(context);
        }
    }
}