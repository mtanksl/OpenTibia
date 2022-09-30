using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class OpenParentContainerCommand : Command
    {
        public OpenParentContainerCommand(Player player, byte containerId)
        {
            Player = player;

            ContainerId = containerId;
        }

        public Player Player { get; set; }

        public byte ContainerId { get; set; }

        public override void Execute(Context context)
        {
            Container container = Player.Client.ContainerCollection.GetContainer(ContainerId);

            if (container != null)
            {
                Container parentContainer = container.Parent as Container;

                if (parentContainer != null)
                {
                    Player.Client.ContainerCollection.ReplaceContainer(parentContainer, ContainerId);

                    var items = new List<Item>();

                    foreach (var item in parentContainer.GetItems() )
                    {
                        items.Add(item);
                    }

                    context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, parentContainer.Metadata.TibiaId, parentContainer.Metadata.Name, parentContainer.Metadata.Capacity, parentContainer.Parent is Container, items) );

                    OnComplete(context);
                }
            }
        }
    }
}