using OpenTibia.Common.Objects;

namespace OpenTibia.Common.Events
{
    public class CreatureUpdateSpeedEventArgs : GameEventArgs
    {
        public CreatureUpdateSpeedEventArgs(Creature creature, ushort speed)
        {
            Creature = creature;

            Speed = speed;
        }

        public Creature Creature { get; set; }

        public ushort Speed { get; set; }
    }
}