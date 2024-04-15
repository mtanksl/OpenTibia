using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class ThunderstormRunePlugin : RunePlugin
    {
        public ThunderstormRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || tile.NotWalkable || tile.BlockPathFinding)
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
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

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 1, 6, 2.6, 16);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage,

                new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
        }
    }
}
