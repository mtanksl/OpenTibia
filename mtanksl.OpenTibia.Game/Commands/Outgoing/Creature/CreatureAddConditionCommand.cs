using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureAddConditionCommand : Command
    {
        public CreatureAddConditionCommand(Creature target, Condition condition)
        {
            Target = target;

            Condition = condition;
        }

        public Creature Target { get; set; }

        public Condition Condition { get; set; }

        public override Promise Execute()
        {
            CreatureConditionBehaviour creatureConditionBehaviour = Context.Server.Components.GetComponents<CreatureConditionBehaviour>(Target)
                .Where(c => c.Condition.ConditionSpecialCondition == Condition.ConditionSpecialCondition)
                .FirstOrDefault();

            if (creatureConditionBehaviour != null)
            {
                Context.Server.Components.RemoveComponent(Target, creatureConditionBehaviour);
            }

            Context.Server.Components.AddComponent(Target, new CreatureConditionBehaviour(Condition) );

            return Promise.Completed;
        }
    }
}