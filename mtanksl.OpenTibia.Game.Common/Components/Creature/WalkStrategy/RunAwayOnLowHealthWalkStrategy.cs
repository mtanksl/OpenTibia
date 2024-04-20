using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public class RunAwayOnLowHealthWalkStrategy : IWalkStrategy
    {
        private int health;

        private IWalkStrategy walkStrategy;

        public RunAwayOnLowHealthWalkStrategy(int health, IWalkStrategy walkStrategy)
        {
            this.health = health;

            this.walkStrategy = walkStrategy;
        }

        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            if (attacker.Health <= health)
            {
                return RunAwayWalkStrategy.Instance.CanWalk(attacker, target, out tile);
            }

            return walkStrategy.CanWalk(attacker, target, out tile);
        }
    }
}