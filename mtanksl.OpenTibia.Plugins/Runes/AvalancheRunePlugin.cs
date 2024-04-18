using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class AvalancheRunePlugin : RunePlugin
    {
        public AvalancheRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || tile.NotWalkable)
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

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 1.2, 7, 2.8, 17);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, tile.Position, area, ProjectileType.Ice, MagicEffectType.IceArea,

                new SimpleAttack(null, null, AnimatedTextColor.Crystal, formula.Min, formula.Max) ) );
        } 
    }
}
