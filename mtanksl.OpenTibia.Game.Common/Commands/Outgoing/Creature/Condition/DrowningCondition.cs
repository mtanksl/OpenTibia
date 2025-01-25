using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
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

        public override async Promise OnStart(Creature creature)
        {
            while (true)
            {
                await Promise.Delay(key, Interval);

                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(null, creature, 
                    
                    new DamageAttack(null, null, DamageType.Drown, Damage, Damage) ) );
            }
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }

        public override Promise OnStop(Creature creature)
        {
            return Promise.Completed;
        }
    }
}