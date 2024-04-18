using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class OutfitCondition : Condition
    {
        public OutfitCondition(Outfit outfit, TimeSpan duration) : base(ConditionSpecialCondition.Outfit)
        {
            Outfit = outfit;

            Duration = duration;
        }

        public Outfit Outfit { get; set; }

        public TimeSpan Duration { get; set; }

        private string key = Guid.NewGuid().ToString();

        public override Promise AddCondition(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, Outfit) ).Then( () =>
            {
                return Promise.Delay(key, Duration);
            } );
        }

        public override Promise RemoveCondition(Creature creature)
        {
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(creature, creature.BaseOutfit, creature.BaseOutfit) );
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }
    }
}