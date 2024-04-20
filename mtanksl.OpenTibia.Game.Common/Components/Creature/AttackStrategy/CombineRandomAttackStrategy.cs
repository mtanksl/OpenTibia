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

        private IAttackStrategy attackStrategy;

        public bool CanAttack(Creature attacker, Creature target)
        {
            attackStrategy = Context.Current.Server.Randomization.Take(attackStrategies);

            return attackStrategy.CanAttack(attacker, target);
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return attackStrategy.Attack(attacker, target);            
        }
    }
}