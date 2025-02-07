using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureKillEventArgs : GameEventArgs
    {
        public CreatureKillEventArgs(Creature creature, Creature target)
        {
            Creature = creature;

            Target = target;
        }

        public Creature Creature { get; }

        public Creature Target { get; }
    }
}