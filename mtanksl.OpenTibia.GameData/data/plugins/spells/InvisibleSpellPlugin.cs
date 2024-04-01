using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class InvisibleSpellPlugin : SpellPlugin
    {
        public InvisibleSpellPlugin(Spell spell) : base(spell)
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
                            
                    new InvisibleCondition(new TimeSpan(0, 3, 20) ) ) );
            } );
        }
    }
}