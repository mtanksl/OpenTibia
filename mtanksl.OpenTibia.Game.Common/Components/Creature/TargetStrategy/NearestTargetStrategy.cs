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

        public Player GetTarget(Creature attacker, Player[] visiblePlayers)
        {
            return visiblePlayers
                .Where(p => !p.Tile.ProtectionZone &&
                            attacker.Tile.Position.CanHearSay(p.Tile.Position) )
                .OrderBy(p => attacker.Tile.Position.ManhattanDistance(p.Tile.Position) )
                .FirstOrDefault();
        }
    }
}