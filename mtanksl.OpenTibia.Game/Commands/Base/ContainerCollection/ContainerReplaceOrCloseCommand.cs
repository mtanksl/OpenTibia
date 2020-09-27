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

        public override void Execute(Context context)
        {
            bool replace = true;

            foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    Player.Client.ContainerCollection.CloseContainer(pair.Key);

                    context.WritePacket(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );

                    replace = false;
                }
            }

            if (replace)
            {
                Player.Client.ContainerCollection.ReplaceContainer(ContainerId, Container);

                var items = new List<Item>();

                foreach (var item in Container.GetItems() )
                {
                    items.Add(item);
                }

                context.WritePacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, Container.Metadata.TibiaId, Container.Metadata.Name, Container.Metadata.Capacity, Container.Container is Container, items) );
            }

            base.OnCompleted(context);
        }
    }
}