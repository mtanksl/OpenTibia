using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerCloseCommand : Command
    {
        public ContainerCloseCommand(Player player, Container container)
        {
            Player = player;

            Container = container;
        }

        public Player Player { get; set; }

        public Container Container { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
            {
                if (pair.Value == Container)
                {
                    //Act

                    Player.Client.ContainerCollection.CloseContainer(pair.Key);

                    //Notify

                    context.Write(Player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                }
            }

            base.Execute(server, context);
        }
    }
}