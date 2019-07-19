using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToTileCommand : MoveItemCommand
    {
        public MoveItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, Position toPosition)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ToPosition = toPosition;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public Position ToPosition { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange



            //Act



            //Notify


        }
    }
}