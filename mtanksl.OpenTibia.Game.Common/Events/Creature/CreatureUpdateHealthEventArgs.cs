using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateHealthEventArgs : GameEventArgs
    {
        public CreatureUpdateHealthEventArgs(Creature creature, ushort health)
        {
            Creature = creature;

            Health = health;
        }

        public Creature Creature { get; }

        public ushort Health { get; }
    }
}