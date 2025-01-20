using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class WoundCleansingSpellPlugin : SpellPlugin
    {
        public WoundCleansingSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.GetSkillLevel(Skill.MagicLevel), 4, 25, 7.95, 51);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, player,

                new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
        }
    }
}