using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class CombineRandomAttackStrategy : IAttackStrategy
    {
        private bool aggressive;

        private IAttackStrategy[] attackStrategies;

        public CombineRandomAttackStrategy(bool aggressive, params IAttackStrategy[] attackStrategies)
        {
            this.aggressive = aggressive;

            this.attackStrategies = attackStrategies;
        }

        private IAttackStrategy attackStrategy;

        public bool CanAttack(Creature attacker, Creature target)
        {
            if (aggressive)
            {
                Context.Current.Server.Randomization.Shuffle(attackStrategies);

                foreach (var attackStrategy in attackStrategies)
                {
                    if (attackStrategy.CanAttack(attacker, target) )
                    {
                        this.attackStrategy = attackStrategy;

                        return true;
                    }
                }

                return false;
            }
            else
            {
                this.attackStrategy = Context.Current.Server.Randomization.Take(attackStrategies);

                return attackStrategy.CanAttack(attacker, target);
            }
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return attackStrategy.Attack(attacker, target);            
        }
    }
}