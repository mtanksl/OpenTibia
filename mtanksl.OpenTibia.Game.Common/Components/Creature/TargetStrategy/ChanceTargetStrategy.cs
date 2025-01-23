using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class ChanceTargetStrategy : ITargetStrategy
    {
        private int chance;

        private ITargetStrategy targetStrategy;

        public ChanceTargetStrategy(int chance, ITargetStrategy targetStrategy)
        {
            this.chance = chance;

            this.targetStrategy = targetStrategy;
        }

        public Player GetTarget(Creature attacker, Player[] visiblePlayers)
        {
            if (Context.Current.Server.Randomization.HasProbability(chance / 100.0) )
            {
                Player player = targetStrategy.GetTarget(attacker, visiblePlayers);

                if (player != null)
                {
                    return player;
                }
            }

            return null;
        }
    }
}