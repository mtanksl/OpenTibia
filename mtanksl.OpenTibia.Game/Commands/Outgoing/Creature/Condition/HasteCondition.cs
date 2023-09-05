using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class HasteCondition : Condition
    {
        public HasteCondition(ushort speed, TimeSpan duration) : base(ConditionSpecialCondition.Haste)
        {
            Speed = speed;

            Duration = duration;
        }

        public ushort Speed { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise AddCondition(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(creature, creature.BaseSpeed, Speed) ).Then( () =>
            {
                return Promise.Delay(key, Duration);
            } );
        }

        public override Promise RemoveCondition(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(creature, creature.BaseSpeed, creature.BaseSpeed) );
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }
    }
}