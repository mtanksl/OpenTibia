using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenParentContainerCommand : Command
    {
        public ParseOpenParentContainerCommand(Player player, byte containerId)
        {
            Player = player;

            ContainerId = containerId;
        }

        public Player Player { get; set; }

        public byte ContainerId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Container container = Player.Client.ContainerCollection.GetContainer(ContainerId);

                if (container != null)
                {
                    Container parentContainer = container.Parent as Container;

                    if (parentContainer != null)
                    {
                        Player.Client.ContainerCollection.ReplaceContainer(parentContainer, ContainerId);

                        List<Item> items = new List<Item>();

                        foreach (var item in parentContainer.GetItems() )
                        {
                            items.Add(item);
                        }

                        context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, parentContainer.Metadata.TibiaId, parentContainer.Metadata.Name, parentContainer.Metadata.Capacity, parentContainer.Parent is Container, items) );

                        resolve(context);
                    }
                }
            } );
        }
    }
}