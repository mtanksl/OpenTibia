using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Spells
{
    public class BloodRageSpellPlugin : SpellPlugin
    {
        public BloodRageSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            //TODO: Disable defense formula, increase all damage caused by 15%

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new SkillCondition(new[] { (Skill.Fist, (int)(1.35 * player.Skills.GetSkillLevel(Skill.Fist) ) ),
                                               (Skill.Axe, (int)(1.35 * player.Skills.GetSkillLevel(Skill.Axe) ) ),
                                               (Skill.Club, (int)(1.35 * player.Skills.GetSkillLevel(Skill.Club) ) ),
                                               (Skill.Sword, (int)(1.35 * player.Skills.GetSkillLevel(Skill.Sword) ) ) }, new TimeSpan(0, 0, 10) ) ) );
            } );
        }
    }
}