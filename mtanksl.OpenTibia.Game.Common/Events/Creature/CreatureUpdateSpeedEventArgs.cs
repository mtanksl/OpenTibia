using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateSpeedEventArgs : GameEventArgs
    {
        public CreatureUpdateSpeedEventArgs(Creature creature, int? conditionSpeed)
        {
            Creature = creature;

            ConditionSpeed = conditionSpeed;
        }

        public Creature Creature { get; }

        public int? ConditionSpeed { get; }
    }
}