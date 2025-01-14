using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateSpeedEventArgs : GameEventArgs
    {
        public CreatureUpdateSpeedEventArgs(Creature creature, ushort baseSpeed, ushort speed)
        {
            Creature = creature;

            BaseSpeed = baseSpeed;

            Speed = speed;
        }

        public Creature Creature { get; }

        public ushort BaseSpeed { get; }

        public ushort Speed { get; }
    }
}