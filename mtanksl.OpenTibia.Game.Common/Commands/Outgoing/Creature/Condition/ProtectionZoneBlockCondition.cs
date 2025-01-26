using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class ProtectionZoneBlockCondition : Condition
    {
        public ProtectionZoneBlockCondition(TimeSpan duration) : base(ConditionSpecialCondition.ProtectionZoneBlock)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Promise.Delay(key, Duration).Then( () =>
            {
                Context.Current.Server.Combats.CleanUp( (Player)creature);

                return Promise.Completed;
            } );
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