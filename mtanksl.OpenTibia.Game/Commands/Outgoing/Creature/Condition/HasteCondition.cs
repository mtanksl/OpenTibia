using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class HasteCondition : CreatureConditionBehaviour
    {
        public HasteCondition(ushort speed, TimeSpan duration) : base(ConditionSpecialCondition.Haste)
        {
            Speed = speed;

            Duration = duration;
        }

        public ushort Speed { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override async void Start()
        {
            base.Start();

            Creature creature = (Creature)GameObject;

            try
            {
                await Context.AddCommand(new CreatureUpdateSpeedCommand(creature, creature.BaseSpeed, Speed) );

                await Promise.Delay(key, Duration);

                Context.Server.GameObjectComponents.RemoveComponent(creature, this);
            }
            catch (PromiseCanceledException) { }
        }

        public override async void Stop()
        {
            base.Stop();

            Creature creature = (Creature)GameObject;

            await Context.AddCommand(new CreatureUpdateSpeedCommand(creature, creature.BaseSpeed, creature.BaseSpeed) );

            Context.Server.CancelQueueForExecution(key);
        }
    }
}