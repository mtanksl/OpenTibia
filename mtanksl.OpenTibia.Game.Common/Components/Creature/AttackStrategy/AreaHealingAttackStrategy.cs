using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class AreaHealingAttackStrategy : IAttackStrategy
    {
        private Offset[] area;

        private ProjectileType? projectileType;

        private int min;

        private int max;

        public AreaHealingAttackStrategy(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, int min, int max)
        {
            this.area = area;

            this.projectileType = projectileType;

            this.min = min;

            this.max = max;
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, projectileType, MagicEffectType.BlueShimmer,

                new HealingAttack(null, min, max) ) );
        }
    }
}