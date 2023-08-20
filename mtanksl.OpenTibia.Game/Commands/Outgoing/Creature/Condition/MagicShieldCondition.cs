using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class MagicShieldCondition : Condition
    {
        private DelayBehaviour delayBehaviour;

        public MagicShieldCondition(TimeSpan duration) : base(ConditionSpecialCondition.MagicShield)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        public override Promise Start(Creature target)
        {
            delayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent(target, new DelayBehaviour(Duration) );

            return delayBehaviour.Promise;
        }

        public override void Stop()
        {
            if (delayBehaviour != null)
            {
                delayBehaviour.Stop();
            }            
        }
    }
}