using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class Condition
    {
        public Condition(ConditionSpecialCondition conditionSpecialCondition)
        {
            ConditionSpecialCondition = conditionSpecialCondition;
        }

        public ConditionSpecialCondition ConditionSpecialCondition { get; }

        public abstract Promise AddCondition(Creature creature);

        public abstract Promise RemoveCondition(Creature creature);

        public abstract void Cancel();
    }
}