﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Spells
{
    public class SharpshooterSpellPlugin : SpellPlugin
    {
        public SharpshooterSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            //TODO: Reduce speed by 70%, disable healing, support and supply spells

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new SkillCondition(new[] { (Skill.Distance, (int)(1.4 * player.Skills.GetSkillLevel(Skill.Distance) ) ) }, new TimeSpan(0, 0, 10) ) ) );
            } );
        }
    }
}