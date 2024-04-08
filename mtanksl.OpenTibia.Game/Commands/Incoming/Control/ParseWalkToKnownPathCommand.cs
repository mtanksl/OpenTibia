using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkToKnownPathCommand : IncomingCommand
    {
        public ParseWalkToKnownPathCommand(Player player, MoveDirection[] moveDirections)
        {
            Player = player;

            MoveDirections = moveDirections;
        }

        public Player Player { get; set; }

        public MoveDirection[] MoveDirections { get; set; }

        public override async Promise Execute()
        {
            foreach (var moveDirection in MoveDirections)
            {
                await Context.AddCommand(new ParseWalkCommand(Player, moveDirection) );
            }
        }
    }
}