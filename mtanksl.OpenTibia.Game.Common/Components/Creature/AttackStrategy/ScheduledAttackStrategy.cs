using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class ScheduledAttackStrategy : IAttackStrategy
    {
        private int ticks;

        private int interval;

        private double chance;

        private IAttackStrategy attackStrategy;

        public ScheduledAttackStrategy(int interval, double chance, IAttackStrategy attackStrategy)
        {
            this.ticks = interval;

            this.interval = interval;

            this.chance = chance;

            this.attackStrategy = attackStrategy;
        }

        private IAttackStrategy currentAttackStrategy;

        public async PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            currentAttackStrategy = null;

            ticks -= 1000;

            while (ticks <= 0)
            {
                ticks += interval;

                if (await attackStrategy.CanAttack(attacker, target) && currentAttackStrategy == null && Context.Current.Server.Randomization.HasProbability(chance / 100.0) )
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