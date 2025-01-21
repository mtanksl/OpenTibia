using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class LightMagicMissileRunePlugin : RunePlugin
    {
        public LightMagicMissileRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.GetSkillLevel(Skill.MagicLevel), 0.4, 2, 0.81, 4);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new SimpleAttack(ProjectileType.EnergySmall, null, DamageType.Energy, formula.Min, formula.Max) ) );
        }
    }
}