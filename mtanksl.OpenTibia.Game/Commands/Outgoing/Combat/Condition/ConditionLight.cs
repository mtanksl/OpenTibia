using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionLight : Condition
    {
        public ConditionLight(Light light, int durationInMilliseconds) : base(ConditionSpecialCondition.Light)
        {
            Light = light;

            DurationInMilliseconds = durationInMilliseconds;
        }

        public Light Light { get; set; }

        public int DurationInMilliseconds { get; set; }

        public override Promise Start(Creature target)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateLightCommand(target, Light) );

            } ).Then( () =>
            {
                return Context.Current.Server.Components.AddComponent(target, new DelayBehaviour(Guid.NewGuid().ToString(), DurationInMilliseconds) ).Promise;
            } );
        }

        public override Promise Stop(Creature target)
        {
            return Context.Current.AddCommand(new CreatureUpdateLightCommand(target, Light.None) );
        }
    }
}