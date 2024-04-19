using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class HealingAttack : Attack
    {
        public HealingAttack(MagicEffectType? magicEffectType, int min, int max)
        {
            ShowMagicEffectType = magicEffectType;

            Min = min;

            Max = max;
        }

        public MagicEffectType? ShowMagicEffectType { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            return Context.Current.Server.Randomization.Take(Min, Max);
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Promise.Completed;
        }

        public override async Promise Hit(Creature attacker, Creature target, int damage)
        {
            if (ShowMagicEffectType != null)
            {
                await Context.Current.AddCommand(new ShowMagicEffectCommand(target, ShowMagicEffectType.Value) );
            }

            await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );

            await Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Poisoned) );

            await Context.Current.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Slowed) );
        }
    }
}