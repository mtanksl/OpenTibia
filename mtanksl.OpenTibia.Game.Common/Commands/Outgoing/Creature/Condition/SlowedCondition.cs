using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class SlowedCondition : Condition
    {
        public SlowedCondition(ushort speed, TimeSpan duration) : base(ConditionSpecialCondition.Slowed)
        {
            Speed = speed;

            Duration = duration;
        }

        public ushort Speed { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(creature, Speed) ).Then( () =>
            {
                return Promise.Delay(key, Duration);
            } );
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }

        public override Promise OnStop(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(creature, creature.BaseSpeed) );
        }
    }
}