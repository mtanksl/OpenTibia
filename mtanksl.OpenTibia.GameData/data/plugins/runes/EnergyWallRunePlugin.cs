using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class EnergyWallRunePlugin : RunePlugin
    {
        public EnergyWallRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || !tile.CanUseWith)
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            Offset[] area = new Offset[]
            {
                new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
            };

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, tile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1504, 1,

                new SimpleAttack(null, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, 30, 30),

                new DamageCondition(SpecialCondition.Electrified, MagicEffectType.EnergyDamage, AnimatedTextColor.LightBlue, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) );
        }
    }
}
