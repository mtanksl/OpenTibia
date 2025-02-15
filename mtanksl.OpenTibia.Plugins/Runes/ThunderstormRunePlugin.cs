using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class ThunderstormRunePlugin : RunePlugin
    {
        public ThunderstormRunePlugin(Rune rune) : base(rune)
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
                                                         new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
				                     new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
		         new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
		         new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
		         new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
				                     new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
							                             new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)
             };

            var formula = Formula.GenericFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.MagicLevel), 1, 6, 2.6, 16);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, toTile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage,

                new DamageAttack(null, null, DamageType.Energy, formula.Min, formula.Max) ) );
        }
    }
}
