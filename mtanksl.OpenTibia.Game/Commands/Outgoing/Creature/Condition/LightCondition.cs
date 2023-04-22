using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class LightCondition : Condition
    {
        private DelayBehaviour delayBehaviour;

        public LightCondition(Light light, TimeSpan duration) : base(ConditionSpecialCondition.Light)
        {
            Light = light;

            Duration = duration;
        }

        public Light Light { get; set; }

        public TimeSpan Duration { get; set; }

        public override Promise Update(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateLightCommand(target, Light) ).Then( () =>
            {
                delayBehaviour = Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(Duration) );

                return delayBehaviour.Promise;

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand(target, Light.None) );
            } );
        }

        public override void Stop(Server server)
        {
            if (delayBehaviour != null)
            {
                delayBehaviour.Stop(server);
            }
        }
    }
}