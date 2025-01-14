using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateDirectionEventArgs : GameEventArgs
    {
        public CreatureUpdateDirectionEventArgs(Creature creature, Direction direction)
        {
            Creature = creature;

            Direction = direction;
        }

        public Creature Creature { get; }

        public Direction Direction { get; }
    }
}