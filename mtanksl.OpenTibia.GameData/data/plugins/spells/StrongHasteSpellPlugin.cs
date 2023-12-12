using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class StrongHasteSpellPlugin : SpellPlugin
    {
        public StrongHasteSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResult(true);
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            var speed = StrongHasteFormula(player.BaseSpeed);

            return Context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new HasteCondition(speed, new TimeSpan(0, 0, 22) ) ) );
            } );
        }
             
        public override void Stop()
        {
            
        }
    }
}