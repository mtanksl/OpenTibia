using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class ProtectionZoneBlockCondition : Condition
    {
        public ProtectionZoneBlockCondition() : base(ConditionSpecialCondition.ProtectionZoneBlock)
        {

        }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Promise.Delay(key, TimeSpan.FromMinutes(15) );
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