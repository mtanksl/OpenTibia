using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Runes
{
    public class FireWallRunePlugin : RunePlugin
    {
        public FireWallRunePlugin(Rune rune) : base(rune)
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
                new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
            };

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, toTile.Position, area, ProjectileType.Fire, MagicEffectType.FirePlume, 1500, 1,

                new SimpleAttack(null, MagicEffectType.FirePlume, DamageType.Fire, 20, 20),

                new DamageCondition(SpecialCondition.Burning, MagicEffectType.FirePlume, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) );
        }
    }
}
