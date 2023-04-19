using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionOutfit : Condition
    {
        public ConditionOutfit(Outfit outfit, int durationInMilliseconds) : base(ConditionSpecialCondition.Outfit)
        {
            Outfit = outfit;

            DurationInMilliseconds = durationInMilliseconds;
        }

        public Outfit Outfit { get; set; }

        public int DurationInMilliseconds { get; set; }

        public override Promise Start(Creature target)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(target, target.BaseOutfit, Outfit) );

            } ).Then( () =>
            {
                return Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(Guid.NewGuid().ToString(), DurationInMilliseconds) ).Promise;
            } );
        }

        public override Promise Stop(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateOutfitCommand(target, target.BaseOutfit, target.BaseOutfit) );
        }
    }
}