using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class IntervalAndChanceTargetStrategy : ITargetStrategy
    {
        private int ticks;

        private int interval;

        private double chance;

        private ITargetStrategy targetStrategy;

        public IntervalAndChanceTargetStrategy(int interval, double chance, ITargetStrategy targetStrategy)
        {
            this.ticks = interval;

            this.interval = interval;

            this.chance = chance;

            this.targetStrategy = targetStrategy;
        }

        public Player GetTarget(int eticks, Creature attacker, Player[] players)
        {
            Player currentPlayer = null;

            ticks -= eticks;

            while (ticks <= 0)
            {
                ticks += interval;

                Player player = targetStrategy.GetTarget(eticks, attacker, players);

                if (player != null && currentPlayer == null && Context.Current.Server.Randomization.HasProbability(chance / 100.0) )
                {
                    currentPlayer = player;
                }
            }

            return currentPlayer;
        }
    }
}