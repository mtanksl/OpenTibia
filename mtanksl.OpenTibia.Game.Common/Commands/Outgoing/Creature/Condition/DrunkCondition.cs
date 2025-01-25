using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class DrunkCondition : Condition
    {
        public DrunkCondition(TimeSpan duration) : base(ConditionSpecialCondition.Drunk)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Promise.Delay(key, Duration);
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