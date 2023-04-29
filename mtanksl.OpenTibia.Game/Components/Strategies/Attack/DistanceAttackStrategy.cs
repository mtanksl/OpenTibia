using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class DistanceAttackStrategy : IAttackStrategy
    {
        private ProjectileType projectileType;

        private int? min;

        private int? max;

        public DistanceAttackStrategy(ProjectileType projectileType, int? min, int? max)
        {
            this.projectileType = projectileType;

            this.min = min;

            this.max = max;
        }

        public Command GetNext(Server server, Creature attacker, Creature target)
        {
            if (server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return new CreatureAttackCreatureCommand(attacker, target, new DistanceAttack(projectileType, min, max) );
            }

            return null;
        }
    }    
}