using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Runes
{
    public class ParalyseRunePlugin : RunePlugin
    {
        public ParalyseRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(target, new SlowedCondition( (ushort)(target.BaseSpeed - 101), TimeSpan.FromSeconds(10) ) ) );
            } );
        }
    }
}