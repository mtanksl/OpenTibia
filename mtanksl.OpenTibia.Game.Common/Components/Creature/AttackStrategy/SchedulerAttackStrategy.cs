using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class SchedulerAttackStrategy : IAttackStrategy
    {
        private int ticks;

        private int interval;

        private double chance;

        private IAttackStrategy attackStrategy;

        public SchedulerAttackStrategy(int interval, double chance, IAttackStrategy attackStrategy)
        {
            this.ticks = interval;

            this.interval = interval;

            this.chance = chance;

            this.attackStrategy = attackStrategy;
        }

        private IAttackStrategy currentAttackStrategy;

        public async PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            ticks -= 1000;

            while (ticks <= 0)
            {
                ticks += interval;

                if (await attackStrategy.CanAttack(attacker, target) && Context.Current.Server.Randomization.HasProbability(chance / 100.0) )
                {
                    currentAttackStrategy = attackStrategy;

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