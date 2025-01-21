using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class HealingAttack : Attack
    {
        private MagicEffectType? magicEffectType;

        public HealingAttack(MagicEffectType? magicEffectType, int min, int max) : base(DamageType.None, min, max)
        {
            this.magicEffectType = magicEffectType;
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