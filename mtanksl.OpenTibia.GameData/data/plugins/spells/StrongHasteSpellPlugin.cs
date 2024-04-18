using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class StrongHasteSpellPlugin : SpellPlugin
    {
        public StrongHasteSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            var speed = StrongHasteFormula(player.BaseSpeed);

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new HasteCondition(speed, new TimeSpan(0, 0, 22) ) ) );
            } );
        }
    }
}