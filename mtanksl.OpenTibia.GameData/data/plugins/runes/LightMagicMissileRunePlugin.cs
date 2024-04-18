using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class LightMagicMissileRunePlugin : RunePlugin
    {
        public LightMagicMissileRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 0.4, 2, 0.81, 4);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new SimpleAttack(ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
        }
    }
}