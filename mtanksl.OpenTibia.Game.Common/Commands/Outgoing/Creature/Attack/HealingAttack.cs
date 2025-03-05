using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class HealingAttack : Attack
    {
        private int min;

        private int max;

        public HealingAttack(int min, int max)
        {
            this.min = min;

            this.max = max;
        }

        public override (int Damage, BlockType BlockType, HashSet<Item> RemoveCharges) Calculate(Creature attacker, Creature target)
        {
            return (Context.Current.Server.Randomization.Take(min, max), BlockType.None, null);
        }

        public override Promise NoDamage(Creature attacker, Creature target, BlockType blockType)
        {
            return Promise.Completed;
        }

        public override async Promise Damage(Creature attacker, Creature target, int damage)
        {
            await Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.BlueShimmer) );

            await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );

            await Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Poisoned) );

            await Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Slowed) );
        }
    }
}