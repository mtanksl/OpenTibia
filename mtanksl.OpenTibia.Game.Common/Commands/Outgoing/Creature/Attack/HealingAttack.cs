using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class HealingAttack : Attack
    {
        private MagicEffectType? magicEffectType;

        private int min;

        private int max;

        public HealingAttack(MagicEffectType? magicEffectType, int min, int max)
        {
            this.magicEffectType = magicEffectType;

            this.min = min;

            this.max = max;
        }

        public override int Calculate(Creature attacker, Creature target)
        {
            return Context.Current.Server.Randomization.Take(min, max);
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Promise.Completed;
        }

        public override async Promise Hit(Creature attacker, Creature target, int damage)
        {
            if (magicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, magicEffectType.Value) );
            }

            await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );

            await Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Poisoned) );

            await Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Slowed) );
        }
    }
}