using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class RandomTargetStrategy : ITargetStrategy
    {
        public static readonly RandomTargetStrategy Instance = new RandomTargetStrategy();

        private RandomTargetStrategy()
        {
            
        }

        public Player GetTarget(Creature attacker, Player[] visiblePlayers)
        {
            Player[] targets = visiblePlayers
                .Where(p => !p.Tile.ProtectionZone &&
                            attacker.Tile.Position.CanHearSay(p.Tile.Position) )
                .ToArray();

            if (targets.Length > 0)
            {
                return Context.Current.Server.Randomization.Take(targets);
            }

            return null;
        }
    }
}