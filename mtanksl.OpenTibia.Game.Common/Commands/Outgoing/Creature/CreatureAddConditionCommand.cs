using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
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
            CreatureConditionBehaviour creatureConditionBehaviour;

            if (Condition.ConditionSpecialCondition == ConditionSpecialCondition.Haste)
            {
                creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                    .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.Slowed)
                    .FirstOrDefault();

                if (creatureConditionBehaviour != null)
                {
                    Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
                }
            }
            else if (Condition.ConditionSpecialCondition == ConditionSpecialCondition.Slowed)
            {
                creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                    .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.Haste)
                    .FirstOrDefault();

                if (creatureConditionBehaviour != null)
                {
                    Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
                }
            }
            else if (Condition.ConditionSpecialCondition == ConditionSpecialCondition.LogoutBlock)
            {
                creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                    .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.ProtectionZoneBlock)
                    .FirstOrDefault();

                if (creatureConditionBehaviour != null)
                {
                    Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
                }
            }
            else if (Condition.ConditionSpecialCondition == ConditionSpecialCondition.ProtectionZoneBlock)
            {
                creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                    .Where(c => c.Condition.ConditionSpecialCondition == ConditionSpecialCondition.LogoutBlock)
                    .FirstOrDefault();

                if (creatureConditionBehaviour != null)
                {
                    Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
                }
            }          

            creatureConditionBehaviour = Context.Server.GameObjectComponents.GetComponents<CreatureConditionBehaviour>(Target)
                .Where(c => c.Condition.ConditionSpecialCondition == Condition.ConditionSpecialCondition)
                .FirstOrDefault();

            if (creatureConditionBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Target, creatureConditionBehaviour);
            }

            Context.Server.GameObjectComponents.AddComponent(Target, new CreatureConditionBehaviour(Condition), false);
                        
            return Promise.Completed;
        }
    }
}