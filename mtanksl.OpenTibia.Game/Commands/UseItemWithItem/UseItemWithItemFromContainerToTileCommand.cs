using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromContainerToTileCommand : Command
    {
        public UseItemWithItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, Position toPosition)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToPosition = toPosition;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public Position ToPosition { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act

            

            //Notify

            
        }
    }
}