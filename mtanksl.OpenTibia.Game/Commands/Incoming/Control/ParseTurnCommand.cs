using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseTurnCommand : Command
    {
        public ParseTurnCommand(Player player, Direction direction)
        {
            Player = player;

            Direction = direction;
        }

        public Player Player { get; set; }

        public Direction Direction { get; set; }

        public override Promise Execute(Context context)
        {
            return context.AddCommand(new CreatureUpdateDirectionCommand(Player, Direction) );
        }
    }
}