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
        private static List<ushort> foods = new List<ushort>() { 2681 /* Grape */, 2689 /* Bread */, 2690 /* Roll */, 8368 /* Cheese*/, 2674 /* Red apple*/, 2666 /* Meat */, 2671 /* Ham */ };

        public FoodSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            int value = Context.Server.Randomization.Take(0, foods.Count - 1);

            ushort openTibiaId = foods[value];

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, openTibiaId, 1, Context.Server.Randomization.Take(1, 2) ) );
            } );
        }
     }
}