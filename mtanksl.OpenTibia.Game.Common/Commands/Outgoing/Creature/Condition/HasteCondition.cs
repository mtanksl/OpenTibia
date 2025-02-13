using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class HasteCondition : Condition
    {
        public HasteCondition(int? conditionSpeed, TimeSpan duration) : base(ConditionSpecialCondition.Haste)
        {
            ConditionSpeed = conditionSpeed;

            Duration = duration;
        }

        public int? ConditionSpeed { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(creature, ConditionSpeed) ).Then( () =>
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
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(creature, null) );
        }
    }
}