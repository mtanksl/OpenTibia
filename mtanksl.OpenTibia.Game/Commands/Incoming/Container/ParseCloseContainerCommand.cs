using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseContainerCommand : IncomingCommand
    {
        public ParseCloseContainerCommand(Player player, byte containerId)
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
                Player.Client.Containers.CloseContainer(ContainerId);

                Context.AddPacket(Player, new CloseContainerOutgoingPacket(ContainerId) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}