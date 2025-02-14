using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.Spells
{
    public class FoodSpellPlugin : SpellPlugin
    {
        private readonly List<ushort> foods;

        public FoodSpellPlugin(Spell spell) : base(spell)
        {
            foods = Context.Server.Values.GetUInt16List("values.items.foods");
        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, Context.Server.Randomization.Take(foods), 1, Context.Server.Randomization.Take(1, 2) ) );
            } );
        }
     }
}