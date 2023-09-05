using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class MagicShieldCondition : Condition
    {
        public MagicShieldCondition(TimeSpan duration) : base(ConditionSpecialCondition.MagicShield)
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