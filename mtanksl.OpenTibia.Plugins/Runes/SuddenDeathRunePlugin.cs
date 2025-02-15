using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class SuddenDeathRunePlugin : RunePlugin
    {
        public SuddenDeathRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.MagicLevel), 4.605, 28, 7.395, 46);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DamageAttack(ProjectileType.SuddenDeath, MagicEffectType.MortArea, DamageType.Death, formula.Min, formula.Max) ) );
        }
    }
}