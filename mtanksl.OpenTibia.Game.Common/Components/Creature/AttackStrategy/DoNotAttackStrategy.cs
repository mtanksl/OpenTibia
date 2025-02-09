using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class DoNotAttackStrategy : IAttackStrategy
    {
        public static readonly DoNotAttackStrategy Instance = new DoNotAttackStrategy();

        private DoNotAttackStrategy()
        {

        }

        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            return Promise.FromResultAsBooleanFalse;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return Promise.Completed;
        }
    }
}