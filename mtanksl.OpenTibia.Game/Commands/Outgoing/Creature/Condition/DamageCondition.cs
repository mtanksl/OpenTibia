using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class DamageCondition : CreatureConditionBehaviour
    {
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

        private string key = Guid.NewGuid().ToString();

        public override async void Start()
        {
            base.Start();

            Creature creature = (Creature)GameObject;

            try
            {
                for (int i = 0; i < Damages.Length; i++)
                {
                    await Context.AddCommand(new CreatureAttackCreatureCommand(null, creature, new SimpleAttack(null, MagicEffectType, AnimatedTextColor, Damages[i], Damages[i] ) ) );

                    if (i != Damages.Length - 1)
                    {
                        await Promise.Delay(key, Interval);
                    }
                }

                Context.Server.GameObjectComponents.RemoveComponent(creature, this);
            }
            catch (PromiseCanceledException) { }
        }

        public override void Stop()
        {
            base.Stop();

            Creature creature = (Creature)GameObject;

            Context.Server.CancelQueueForExecution(key);
        }
    }
}