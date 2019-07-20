using OpenTibia.Common.Objects;

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

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act
                       
            //Notify

            base.Execute(server, context);
        }
    }
}