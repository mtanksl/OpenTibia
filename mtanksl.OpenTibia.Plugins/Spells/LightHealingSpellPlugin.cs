using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class LightHealingSpellPlugin : SpellPlugin
    {
        public LightHealingSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.MagicLevel), 1.4, 8, 1.795, 11);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, player, 
                        
                new HealingAttack(formula.Min, formula.Max) ) );
        }
    }
}