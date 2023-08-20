using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class DamageCondition : Condition
    {
        private DelayBehaviour delayBehaviour;

        public DamageCondition(SpecialCondition specialCondition, MagicEffectType? magicEffectType, AnimatedTextColor? animatedTextColor, int[] damages, TimeSpan interval) : base( (ConditionSpecialCondition)specialCondition)
        {
            SpecialCondition = specialCondition;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Damages = damages;

            Interval = interval;
        }

        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int[] Damages { get; set; }

        public TimeSpan Interval { get; set; }

        public override async Promise Start(Creature target)
        {
            for (int i = 0; i < Damages.Length; i++)
            {
                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(null, target, new SimpleAttack(null, MagicEffectType, AnimatedTextColor, Damages[i], Damages[i] ) ) );

                if (i < Damages.Length - 1)
                {
                    delayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent(target, new DelayBehaviour(Interval) );

                    await delayBehaviour.Promise;
                }
            }
        }

        public override void Stop()
        {
            if (delayBehaviour != null)
            {
                delayBehaviour.Stop();
            }
        }
    }
}