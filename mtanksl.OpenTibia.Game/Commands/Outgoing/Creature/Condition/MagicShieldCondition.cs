using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class MagicShieldCondition : CreatureConditionBehaviour
    {
        public MagicShieldCondition(TimeSpan duration) : base(ConditionSpecialCondition.MagicShield)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override async void Start()
        {
            base.Start();

            Creature creature = (Creature)GameObject;

            try
            {
                await Promise.Delay(key, Duration);

                Context.Server.GameObjectComponents.RemoveComponent(creature, this);
            }
            catch (PromiseCanceledException) { }
        }

        public override void Stop()
        {
            base.Stop();

            Creature creature = (Creature)GameObject;

            Context.Server.CancelQueueForExecution(key);
        }
    }
}