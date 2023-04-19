using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureRemoveConditionCommand : Command
    {
        public CreatureRemoveConditionCommand(Creature target, ConditionSpecialCondition combatSpecialCondition)
        {
            Target = target;

            CombatSpecialCondition = combatSpecialCondition;
        }

        public Creature Target { get; set; }

        public ConditionSpecialCondition CombatSpecialCondition { get; set; }

        public override Promise Execute()
        {
            CreatureConditionBehaviour creatureConditionBehaviour = Context.Server.Components.GetComponents<CreatureConditionBehaviour>(Target)
                .Where(c => c.Condition.ConditionSpecialCondition == CombatSpecialCondition)
                .FirstOrDefault();

            if (creatureConditionBehaviour != null)
            {
                Context.Server.Components.RemoveComponent(Target, creatureConditionBehaviour);
            }

            return Promise.Completed;
        }
    }
}