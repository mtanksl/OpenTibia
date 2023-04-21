using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Strategies
{
    public class DistanceAttackStrategy : IAttackStrategy
    {
        private ProjectileType projectileType;

        private int cooldownInMilliseconds;

        public DistanceAttackStrategy(ProjectileType projectileType, int cooldownInMilliseconds)
        {
            this.projectileType = projectileType;

            this.cooldownInMilliseconds = cooldownInMilliseconds;
        }

        public int CooldownInMilliseconds
        {
            get
            {
                return cooldownInMilliseconds;
            }
        }

        public Command GetNext(Server server, Creature attacker, Creature target)
        {
            if (server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return new CreatureAttackCreatureCommand(attacker, target, new DistanceAttack(projectileType) );
            }

            return null;
        }
    }    
}