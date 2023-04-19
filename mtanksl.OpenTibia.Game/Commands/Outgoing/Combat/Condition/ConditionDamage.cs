using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionDamage : Condition
    {
        public ConditionDamage(SpecialCondition specialCondition, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int[] damages, int intervalInMilliseconds) : base( (ConditionSpecialCondition)specialCondition)
        {
            SpecialCondition = specialCondition;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Damages = damages;

            IntervalInMilliseconds = intervalInMilliseconds;
        }

        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int[] Damages { get; set; }

        public int IntervalInMilliseconds { get; set; }

        public override async Promise Start(Creature target)
        {
            for (int i = 0; i < Damages.Length; i++)
            {
                await Context.Current.AddCommand(new CombatAddDamageCommand(null, target, (attacker, target) => Damages[i], null, MagicEffectType, AnimatedTextColor) );

                if (i < Damages.Length - 1)
                {
                    await Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(Guid.NewGuid().ToString(), IntervalInMilliseconds) ).Promise;
                }
            }
        }

        public override Promise Stop(Creature target)
        {
            return Promise.Completed;
        }
    }
}