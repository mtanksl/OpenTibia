using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class OutfitCondition : Condition
    {
        public OutfitCondition(Outfit conditionOutfit, TimeSpan duration) : base(ConditionSpecialCondition.Outfit)
        {
            ConditionOutfit = conditionOutfit;

            Duration = duration;
        }

        public Outfit ConditionOutfit { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise OnStart(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, ConditionOutfit, creature.Swimming, creature.Stealth) ).Then( () =>
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
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, null, creature.Swimming, creature.Stealth) );
        }
    }
}