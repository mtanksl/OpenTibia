using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Spells
{
    public class LightSpellPlugin : SpellPlugin
    {
        public LightSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new LightCondition(new Light(6, 215), new TimeSpan(0, 6, 10) ) ) );
            } );
        }
    }
}