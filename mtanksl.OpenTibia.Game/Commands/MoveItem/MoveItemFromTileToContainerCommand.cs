using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToContainerCommand : Command
    {
        public MoveItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, byte toContainerId, byte toContainerIndex)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ToContainerId = ToContainerId;

            ToContainerIndex = ToContainerIndex;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

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