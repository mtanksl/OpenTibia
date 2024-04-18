using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class GreatLightSpellPlugin : SpellPlugin
    {
        public GreatLightSpellPlugin(Spell spell) : base(spell)
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
                            
                    new LightCondition(new Light(8, 215), new TimeSpan(0, 11, 35) ) ) );
            } );
        }
    }
}