using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class DamageCondition : Condition
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

        public override async Promise AddCondition(Creature creature)
        {
            for (int i = 0; i < Damages.Length; i++)
            {
                await Promise.Delay(key, Interval);

                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(null, creature, 
                    
                    new SimpleAttack(null, MagicEffectType, AnimatedTextColor, Damages[i], Damages[i] ) ) );
            }
        }

        public override Promise RemoveCondition(Creature creature)
        {
            return Promise.Completed;  
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }   
    }
}