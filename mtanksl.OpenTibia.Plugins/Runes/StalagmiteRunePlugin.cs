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

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.MagicLevel, 0.81, 4, 1.59, 10);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new SimpleAttack(ProjectileType.Poison, MagicEffectType.GreenRings, AnimatedTextColor.Green, formula.Min, formula.Max) ) );
        }
    }
}