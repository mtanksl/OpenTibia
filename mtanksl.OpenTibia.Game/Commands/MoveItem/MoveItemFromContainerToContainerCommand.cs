using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToContainerCommand : MoveItemCommand
    {
        public MoveItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, byte toContainerId, byte toContainerIndex)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act

            

            //Notify

            
        }
    }
}