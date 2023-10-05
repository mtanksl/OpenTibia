using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class DrowningCondition : Condition
    {
        public DrowningCondition(int damage, TimeSpan interval) : base(ConditionSpecialCondition.Drowning)
        {
            Damage = damage;

            Interval = interval;
        }

        public int Damage { get; set; }

        public TimeSpan Interval { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override async Promise AddCondition(Creature creature)
        {
            while (true)
            {
                await Promise.Delay(key, Interval);

                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(null, creature, 
                    
                    new SimpleAttack(null, MagicEffectType.BlueRings, AnimatedTextColor.Crystal, Damage, Damage) ) );
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