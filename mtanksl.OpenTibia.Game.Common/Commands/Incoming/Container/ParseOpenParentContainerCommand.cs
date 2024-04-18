using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenParentContainerCommand : IncomingCommand
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

                    Context.AddPacket(Player, new OpenContainerOutgoingPacket(ContainerId, parentContainer.Metadata.TibiaId, parentContainer.Metadata.Name, parentContainer.Metadata.Capacity.Value, parentContainer.Parent is Container, parentContainer.GetItems().ToList() ) );

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}