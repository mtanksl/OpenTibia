using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class CancelInvisibilityAttack : Attack
    {
        public override (int Damage, BlockType BlockType, HashSet<Item> RemoveCharges) Calculate(Creature attacker, Creature target)
        {
            return (0, BlockType.None, null);
        }

        public override async Promise NoDamage(Creature attacker, Creature target, BlockType blockType)
        {
            if (target is Monster monster)
            {
                if (monster.Invisible)
                {
                    await Context.Current.AddCommand(new CreatureRemoveConditionCommand(monster, ConditionSpecialCondition.Invisible) );
                }
            }
        }

        public override Promise Damage(Creature attacker, Creature target, int damage)
        {
            return Promise.Completed;
        }
    }
}