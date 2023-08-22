using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureAddConditionCommand : Command
    {
        public CreatureAddConditionCommand(Creature target, CreatureConditionBehaviour condition)
        {
            Target = target;

            Condition = condition;
        }

        public Creature Target { get; set; }

        public CreatureConditionBehaviour Condition { get; set; }

        public override Promise Execute()
        {
            CreatureConditionBehaviour creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                .Where(c => c.ConditionSpecialCondition == Condition.ConditionSpecialCondition)
                .FirstOrDefault();

            if (creatureConditionBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
            }

            Context.Server.GameObjectComponents.AddComponent(Target, Condition, false);

            return Promise.Completed;
        }
    }
}