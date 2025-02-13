using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class StealthCondition : Condition
    {
        public StealthCondition(TimeSpan duration) : base(ConditionSpecialCondition.Invisible)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, creature.ConditionOutfit, creature.Swimming, true) ).Then( () =>
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
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, creature.ConditionOutfit, creature.Swimming, false) );
        }
    }
}