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

        public Player GetTarget(Creature attacker, Player[] visiblePlayers)
        {
            foreach (var targetStrategy in Context.Current.Server.Randomization.Shuffle(targetStrategies) )
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