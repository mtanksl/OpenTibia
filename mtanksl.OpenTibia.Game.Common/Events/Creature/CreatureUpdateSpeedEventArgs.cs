using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateSpeedEventArgs : GameEventArgs
    {
        public CreatureUpdateSpeedEventArgs(Creature creature, int conditionSpeed, int itemSpeed)
        {
            Creature = creature;

            ConditionSpeed = conditionSpeed;

            ItemSpeed = itemSpeed;
        }

        public Creature Creature { get; }

        public int ConditionSpeed { get; }

        public int ItemSpeed { get; set; }
    }
}