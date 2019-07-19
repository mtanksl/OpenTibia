using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromInventoryToContainerCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromInventoryToContainerCommand(Player player, byte fromSlot, byte toContainerId, byte toContainerIndex)
        {
            Player = player;

            FromSlot = fromSlot;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

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