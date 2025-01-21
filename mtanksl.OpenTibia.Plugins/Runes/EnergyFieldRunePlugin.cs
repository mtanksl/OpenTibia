using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Runes
{
    public class EnergyFieldRunePlugin : RunePlugin
    {
        public EnergyFieldRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.BlockPathFinding)
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            Offset[] area = new Offset[]
            {
                new Offset(0, 0)
            };

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, toTile.Position, area, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1504, 1,

                new SimpleAttack(null, null, DamageType.Energy, 30, 30),

                new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) );
        }
    }
}