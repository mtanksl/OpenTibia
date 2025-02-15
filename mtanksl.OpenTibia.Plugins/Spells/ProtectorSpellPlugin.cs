using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Spells
{
    public class ProtectorSpellPlugin : SpellPlugin
    {
        public ProtectorSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            //TODO: Reduce all damage received by 15%, reduce all damage caused by 35%

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new SkillCondition(new[] { (Skill.Shield, (int)(2.2 * player.Skills.GetSkillLevel(Skill.Shield) ) ) }, new TimeSpan(0, 0, 10) ) ) );
            } );
        }
    }
}