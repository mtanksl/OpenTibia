using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureDeathEventArgs : GameEventArgs
    {
        public CreatureDeathEventArgs(Creature creature)
        {
            Creature = creature;
        }

        public Creature Creature { get; }
    }
}