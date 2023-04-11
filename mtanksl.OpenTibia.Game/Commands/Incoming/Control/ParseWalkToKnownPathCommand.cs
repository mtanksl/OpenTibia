using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkToKnownPathCommand : Command
    {
        public ParseWalkToKnownPathCommand(Player player, MoveDirection[] moveDirections)
        {
            Player = player;

            MoveDirections = moveDirections;
        }

        public Player Player { get; set; }

        public MoveDirection[] MoveDirections { get; set; }

        private int index;

        public override Promise Execute()
        {
            if (index < MoveDirections.Length)
            {
                return Context.AddCommand(new ParseWalkCommand(Player, MoveDirections[index++] ) ).Then( () =>
                {
                    return Execute();
                } ); 
            }

            return Promise.Completed;
        }
    }
}