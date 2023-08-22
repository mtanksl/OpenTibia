using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class LightCondition : CreatureConditionBehaviour
    {
        public LightCondition(Light light, TimeSpan duration) : base(ConditionSpecialCondition.Light)
        {
            Light = light;

            Duration = duration;
        }

        public Light Light { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override async void Start()
        {
            base.Start();

            Creature creature = (Creature)GameObject;
            
            try
            {
                await Context.AddCommand(new CreatureUpdateLightCommand(creature, Light) );

                await Promise.Delay(key, Duration);

                Context.Server.GameObjectComponents.RemoveComponent(creature, this);
            }
            catch (PromiseCanceledException) { }
        }

        public override async void Stop()
        {
            base.Stop();

            Creature creature = (Creature)GameObject;

            await Context.AddCommand(new CreatureUpdateLightCommand(creature, Light.None) );

            Context.Server.CancelQueueForExecution(key);
        }
    }
}