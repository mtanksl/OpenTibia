using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class Condition
    {
        public Condition(ConditionSpecialCondition conditionSpecialCondition)
        {
            ConditionSpecialCondition = conditionSpecialCondition;
        }

        public ConditionSpecialCondition ConditionSpecialCondition { get; set; }

        public abstract Promise Start(Server server, Creature target);

        public abstract void Stop(Server server);
    }
}