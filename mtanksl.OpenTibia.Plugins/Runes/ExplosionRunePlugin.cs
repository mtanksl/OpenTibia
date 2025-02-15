using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class ExplosionRunePlugin : RunePlugin
    {
        public ExplosionRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            if (toTile == null || toTile.Ground == null || toTile.NotWalkable)
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            Offset[] area = new Offset[]
            {
                                    new Offset(0, -1),
                new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                                    new Offset(0, 1)
            };

            var formula = Formula.GenericFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.MagicLevel), 0, 0, 4.8, 0);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, toTile.Position, area, ProjectileType.Explosion, MagicEffectType.ExplosionArea,

                new DamageAttack(null, null, DamageType.Physical, formula.Min, formula.Max) ) );
        }
    }
}
