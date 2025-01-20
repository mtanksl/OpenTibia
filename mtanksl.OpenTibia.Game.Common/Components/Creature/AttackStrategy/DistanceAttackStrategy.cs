using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class DistanceAttackStrategy : IAttackStrategy
    {
        private ProjectileType projectileType;

        private int min;

        private int max;

        public DistanceAttackStrategy(ProjectileType projectileType, int min, int max)
        {
            this.projectileType = projectileType;

            this.min = min;

            this.max = max;
        }

        public PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, 
                
                new DistanceAttack(projectileType, min, max) ) );
        }
    }
}