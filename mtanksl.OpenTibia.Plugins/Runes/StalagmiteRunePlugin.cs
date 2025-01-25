using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class StalagmiteRunePlugin : RunePlugin
    {
        public StalagmiteRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.GetSkillLevel(Skill.MagicLevel), 0.81, 4, 1.59, 10);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DamageAttack(ProjectileType.Poison, null, DamageType.Earth, formula.Min, formula.Max) ) );
        }
    }
}