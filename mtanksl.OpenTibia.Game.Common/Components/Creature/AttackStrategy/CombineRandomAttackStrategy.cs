using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class CombineRandomAttackStrategy : IAttackStrategy
    {
        private IAttackStrategy[] attackStrategies;

        public CombineRandomAttackStrategy(params IAttackStrategy[] attackStrategies)
        {
            this.attackStrategies = attackStrategies;
        }

        private IAttackStrategy currentAttackStrategy;

        public async PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            currentAttackStrategy = null;

            foreach (var attackStrategy in Context.Current.Server.Randomization.Shuffle(attackStrategies) )
            {
                if (await attackStrategy.CanAttack(ticks, attacker, target) && currentAttackStrategy == null)
                {
                    currentAttackStrategy = attackStrategy;
                }
            }

            return currentAttackStrategy != null;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return currentAttackStrategy.Attack(attacker, target);            
        }
    }
}