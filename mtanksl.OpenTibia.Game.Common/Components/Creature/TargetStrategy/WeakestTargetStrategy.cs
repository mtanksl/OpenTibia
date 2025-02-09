using OpenTibia.Common.Objects;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class WeakestTargetStrategy : ITargetStrategy
    {
        public static readonly WeakestTargetStrategy Instance = new WeakestTargetStrategy();

        private WeakestTargetStrategy()
        {
            
        }

        public Player GetTarget(int ticks, Creature attacker, Player[] players)
        {
            return players
                .OrderBy(p => p.MaxHealth)
                    .ThenBy(p => p.Health)
                .FirstOrDefault();
        }
    }
}