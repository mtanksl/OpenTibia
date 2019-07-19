using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromContainerCommand : UseItemCommand
    {
        public UseItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act



            //Notify

            
        }
    }
}