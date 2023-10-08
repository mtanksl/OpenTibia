using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace mtanksl.OpenTibia.GameData.Plugins.Runes
{
    public class LightMagicMissileRunePlugin : RunePlugin
    {
        public LightMagicMissileRunePlugin(Rune rune) : base(rune)
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
            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 15, 5);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new SimpleAttack(ProjectileType.EnergySmall, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
        }

        public override void Stop()
        {
            
        }
    }
}