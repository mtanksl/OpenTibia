using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class CombineComboRandomAttackStrategy : IAttackStrategy
    {
        private IAttackStrategy[] attackStrategies;

        public CombineComboRandomAttackStrategy(params IAttackStrategy[] attackStrategies)
        {
            this.attackStrategies = attackStrategies;
        }

        private List<IAttackStrategy> currentAttackStrategies = new List<IAttackStrategy>();

        public async PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            currentAttackStrategies.Clear();

            foreach (var attackStrategy in Context.Current.Server.Randomization.Shuffle(attackStrategies) )
            {
                if (await attackStrategy.CanAttack(ticks, attacker, target) )
                {
                    currentAttackStrategies.Add(attackStrategy);
                }
            }

            return currentAttackStrategies.Count > 0;
        }

        public async Promise Attack(Creature attacker, Creature target)
        {
            foreach (var currentAttackStrategy in currentAttackStrategies)
            {
                await currentAttackStrategy.Attack(attacker, target);            
            }
        }
    }
}