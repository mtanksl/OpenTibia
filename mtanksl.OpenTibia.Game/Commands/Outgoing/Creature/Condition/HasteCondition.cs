using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class HasteCondition : Condition
    {
        private DelayBehaviour delayBehaviour;

        public HasteCondition(ushort speed, TimeSpan duration) : base(ConditionSpecialCondition.Haste)
        {
            Speed = speed;

            Duration = duration;
        }

        public ushort Speed { get; set; }

        public TimeSpan Duration { get; set; }

        public override Promise Start(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(target, target.BaseSpeed, Speed) ).Then( () =>
            {
                delayBehaviour = Context.Current.Server.GameObjectComponents.AddComponent(target, new DelayBehaviour(Duration) );

                return delayBehaviour.Promise;

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(target, target.BaseSpeed, target.BaseSpeed) );
            } );
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