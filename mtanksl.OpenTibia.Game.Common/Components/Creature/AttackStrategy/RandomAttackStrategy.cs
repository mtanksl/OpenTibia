using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class RandomAttackStrategy : IAttackStrategy
    {
        private IAttackStrategy[] attackStrategies;

        public RandomAttackStrategy(params IAttackStrategy[] attackStrategies)
        {
            this.attackStrategies = attackStrategies;
        }

        private IAttackStrategy currentAttackStrategy;

        public async PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            foreach (var attackStrategy in Context.Current.Server.Randomization.Shuffle(attackStrategies) )
            {
                if (await attackStrategy.CanAttack(attacker, target) )
                {
                    this.currentAttackStrategy = attackStrategy;

                    return true;
                }
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return currentAttackStrategy.Attack(attacker, target);            
        }
    }
}