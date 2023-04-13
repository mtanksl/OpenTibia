using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Strategies
{
    public class CloseAttackStrategy : IAttackStrategy
    {
        private int cooldownInMilliseconds;

        private Func<Creature, Creature, int> formula;

        public CloseAttackStrategy(int cooldownInMilliseconds, Func<Creature, Creature, int> formula)
        {
            this.cooldownInMilliseconds = cooldownInMilliseconds;

            this.formula = formula;
        }

        public int CooldownInMilliseconds
        {
            get
            {
                return cooldownInMilliseconds;
            }
        }

        public Command GetNext(Server server, Creature creature, Creature target)
        {
            if (creature.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return new CombatAttackCreatureWithMeleeCommand(creature, target, formula);
            }

            return null;
        }
    }
}