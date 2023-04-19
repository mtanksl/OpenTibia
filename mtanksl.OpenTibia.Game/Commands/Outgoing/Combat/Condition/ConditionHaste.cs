using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionHaste : Condition
    {
        public ConditionHaste(ushort speed, int durationInMilliseconds) : base(ConditionSpecialCondition.Haste)
        {
            Speed = speed;

            DurationInMilliseconds = durationInMilliseconds;
        }

        public ushort Speed { get; set; }

        public int DurationInMilliseconds { get; set; }

        public override Promise Start(Creature target)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(target, target.BaseSpeed, Speed));

            } ).Then( () =>
            {
                return Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(Guid.NewGuid().ToString(), DurationInMilliseconds) ).Promise;
            } );
        }

        public override Promise Stop(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateSpeedCommand(target, target.BaseSpeed, target.BaseSpeed) );
        }
    }
}