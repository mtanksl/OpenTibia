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

        public Creature Creature { get; set; }

        public ushort Health { get; set; }
    }
}