MoveItemCommandusing OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromContainerToInventoryCommand(Player player, byte fromContainerId, byte fromContainerIndex, byte toSlot)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToSlot = toSlot;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public byte ToSlot { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act

            

            //Notify

            
        }
    }
}