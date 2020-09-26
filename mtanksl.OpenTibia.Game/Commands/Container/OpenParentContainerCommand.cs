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
            //Arrange

            Container container = Player.Client.ContainerCollection.GetContainer(ContainerId);

            if (container != null)
            {
                Container parentContainer = container.Container as Container;

                if (parentContainer != null)
                {
                    //Act

                    Player.Client.ContainerCollection.ReplaceContainer(ContainerId, parentContainer);

                    //Notify

                    var items = new List<Item>();

                    foreach (var item in parentContainer.GetItems() )
                    {
                        items.Add(item);
                    }

                    context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, parentContainer.Metadata.TibiaId, parentContainer.Metadata.Name, parentContainer.Metadata.Capacity, parentContainer.Container is Container, items) );

                    base.Execute(context);
                }
            }
        }
    }
}