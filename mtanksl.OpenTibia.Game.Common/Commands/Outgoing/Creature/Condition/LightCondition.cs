using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class LightCondition : Condition
    {
        public LightCondition(Light light, TimeSpan duration) : base(ConditionSpecialCondition.Light)
        {
            Light = light;

            Duration = duration;
        }

        public Light Light { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateLightCommand(creature, Light) ).Then( () =>
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
            return Context.Current.AddCommand(new CreatureUpdateLightCommand(creature, Light.None) );
        }
    }
}