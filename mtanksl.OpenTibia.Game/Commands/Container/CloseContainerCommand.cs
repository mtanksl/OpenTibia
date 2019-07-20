using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CloseContainerCommand : Command
    {
        public CloseContainerCommand(Player player, byte containerId)
        {
            Player = player;

            ContainerId = containerId;
        }

        public Player Player { get; set; }

        public byte ContainerId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container container = Player.Client.ContainerCollection.GetContainer(ContainerId);

            if (container != null)
            {
                //Act

                byte containerId = Player.Client.ContainerCollection.CloseContainer(container);

                //Notify

                context.Write(Player.Client.Connection, new CloseContainerOutgoingPacket(containerId) );
            }

            base.Execute(server, context);
        }
    }
}