using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class FoodSpellPlugin : SpellPlugin
    {
        private static List<ushort> foods = new List<ushort>() { 2681 /* Grape */, 2689 /* Bread */, 2690 /* Roll */, 8368 /* Cheese*/, 2674 /* Red apple*/, 2666 /* Meat */, 2671 /* Ham */ };

        public FoodSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            ushort openTibiaId = foods[Context.Server.Randomization.Take(0, foods.Count - 1) ];

            int count = Context.Server.Randomization.Take(1, 2);

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, openTibiaId, 1, count) );
            } );
        }
             
        public override void Stop()
        {
            
        }
     }
}