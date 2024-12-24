using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class RandomChangeTargetStrategy : IChangeTargetStrategy
    {
        private double probability;

        public RandomChangeTargetStrategy(double probability)
        {
            this.probability = probability;
        }

        public bool ShouldChange(Creature attacker, Player target)
        {
            return Context.Current.Server.Randomization.HasProbability(probability);
        }
    }
}