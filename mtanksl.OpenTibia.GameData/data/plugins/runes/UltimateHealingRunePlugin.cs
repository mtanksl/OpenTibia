using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class UltimateHealingRunePlugin : RunePlugin
    {
        public UltimateHealingRunePlugin(Rune rune) : base(rune)
        {

        }

        public override void Start()
        {
            
        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            return Promise.FromResult(true);
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 7.22, 44, 12.79, 79);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
        }

        public override void Stop()
        {
            
        }
    }
}