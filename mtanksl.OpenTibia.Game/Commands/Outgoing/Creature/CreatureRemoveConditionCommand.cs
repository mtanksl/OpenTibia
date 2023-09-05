using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureRemoveConditionCommand : Command
    {
        public CreatureRemoveConditionCommand(Creature target, ConditionSpecialCondition conditionSpecialCondition)
        {
            Target = target;

            ConditionSpecialCondition = conditionSpecialCondition;
        }

        public Creature Target { get; set; }

        public ConditionSpecialCondition ConditionSpecialCondition { get; set; }

        public override Promise Execute()
        {
            CreatureConditionBehaviour creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition)
                .FirstOrDefault();

            if (creatureConditionBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
            }

            return Promise.Completed;
        }
    }
}