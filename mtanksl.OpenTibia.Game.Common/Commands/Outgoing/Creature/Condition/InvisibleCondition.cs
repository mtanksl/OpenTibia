using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class InvisibleCondition : Condition
    {
        public InvisibleCondition(TimeSpan duration) : base(ConditionSpecialCondition.Invisible)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise AddCondition(Creature creature)
        {
            return Promise.Delay(key, Duration);
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