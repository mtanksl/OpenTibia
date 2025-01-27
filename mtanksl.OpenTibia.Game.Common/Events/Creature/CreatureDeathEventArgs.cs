using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureDeathEventArgs : GameEventArgs
    {
        public CreatureDeathEventArgs(Creature creature, Creature killer, Creature mostDamage)
        {
            Creature = creature;

            Killer = killer;

            MostDamage = mostDamage;
        }

        public Creature Creature { get; }

        public Creature Killer { get; }

        public Creature MostDamage { get; }
    }
}