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

        public Player GetTarget(Creature attacker, Player[] visiblePlayers)
        {
            Player weakest = visiblePlayers
                .Where(p => !p.Tile.ProtectionZone &&
                            attacker.Tile.Position.CanHearSay(p.Tile.Position) )
                .OrderBy(p => p.MaxHealth)
                    .ThenBy(p => p.Health)
                .FirstOrDefault();

            if (weakest != null)
            {
                return weakest;
            }
               
            return null;
        }
    }
}