using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionMagicShield : Condition
    {
        public ConditionMagicShield(int durationInMilliseconds) : base(ConditionSpecialCondition.MagicShield)
        {
            DurationInMilliseconds = durationInMilliseconds;
        }

        public int DurationInMilliseconds { get; set; }

        public override Promise Start(Creature target)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(Guid.NewGuid().ToString(), DurationInMilliseconds) ).Promise;
            } );
        }

        public override Promise Stop(Creature target)
        {
            return Promise.Completed;
        }
    }
}