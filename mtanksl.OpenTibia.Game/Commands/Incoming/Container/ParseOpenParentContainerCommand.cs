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

        public override Promise Execute()
        {
            Container container = Player.Client.Containers.GetContainer(ContainerId);

            if (container != null)
            {
                if (container.Parent is Container parentContainer)
                {
                    Player.Client.Containers.ReplaceContainer(parentContainer, ContainerId);

                    List<Item> items = new List<Item>();

                    foreach (var item in parentContainer.GetItems() )
                    {
                        items.Add(item);
                    }

                    Context.AddPacket(Player.Client.Connection, new OpenContainerOutgoingPacket(ContainerId, parentContainer.Metadata.TibiaId, parentContainer.Metadata.Name, parentContainer.Metadata.Capacity.Value, parentContainer.Parent is Container, items) );

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}