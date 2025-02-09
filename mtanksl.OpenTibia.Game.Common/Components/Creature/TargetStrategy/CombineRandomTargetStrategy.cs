using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class CombineRandomTargetStrategy : ITargetStrategy
    {
        private ITargetStrategy[] targetStrategies;

        public CombineRandomTargetStrategy(params ITargetStrategy[] targetStrategies)
        {
            this.targetStrategies = targetStrategies;
        }

        public Player GetTarget(int ticks, Creature attacker, Player[] players)
        {
            Player currentPlayer = null;

            foreach (var targetStrategy in Context.Current.Server.Randomization.Shuffle(targetStrategies) )
            {
                Player player = targetStrategy.GetTarget(ticks, attacker, players);

                if (player != null && currentPlayer == null)
                {
                    currentPlayer = player;
                }
            }

            return currentPlayer;
        }
    }
}