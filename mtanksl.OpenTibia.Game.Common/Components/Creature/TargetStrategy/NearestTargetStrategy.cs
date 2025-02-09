using OpenTibia.Common.Objects;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class NearestTargetStrategy : ITargetStrategy
    {
        public static readonly NearestTargetStrategy Instance = new NearestTargetStrategy();

        private NearestTargetStrategy()
        {
            
        }

        public Player GetTarget(int ticks, Creature attacker, Player[] players)
        {
            return players
                .OrderBy(p => attacker.Tile.Position.ManhattanDistance(p.Tile.Position) )
                .FirstOrDefault();
        }
    }
}