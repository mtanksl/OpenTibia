using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class RandomTargetStrategy : ITargetStrategy
    {
        public static readonly RandomTargetStrategy Instance = new RandomTargetStrategy();

        private RandomTargetStrategy()
        {
            
        }

        public Player GetTarget(int ticks, Creature attacker, Player[] players)
        {
            return Context.Current.Server.Randomization.Take(players);
        }
    }
}