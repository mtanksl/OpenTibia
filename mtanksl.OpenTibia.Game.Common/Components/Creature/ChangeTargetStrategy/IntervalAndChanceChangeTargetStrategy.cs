using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class IntervalAndChanceChangeTargetStrategy : IChangeTargetStrategy
    {
        private int ticks;

        private int interval;

        private double chance;

        private IChangeTargetStrategy changeTargetStrategy;

        public IntervalAndChanceChangeTargetStrategy(int interval, double chance, IChangeTargetStrategy changeTargetStrategy)
        {
            this.ticks = interval;

            this.interval = interval;

            this.chance = chance;

            this.changeTargetStrategy = changeTargetStrategy;
        }

        public bool ShouldChange(int eticks, Creature attacker, Player target)
        {
            bool currentResult = false;

            ticks -= eticks;

            while (ticks <= 0)
            {
                ticks += interval;

                bool result = changeTargetStrategy.ShouldChange(eticks, attacker, target);

                if (result != false && currentResult == false && Context.Current.Server.Randomization.HasProbability(chance / 100.0) )
                {
                    currentResult = result;
                }
            }

            return currentResult;
        }
    }
}