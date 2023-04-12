using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        public Command GetNext(Creature creature, Creature target)
        {
            Context context = Context.Current;

            if (creature.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return CombatCommand.TargetAttack(creature, target, null, MagicEffectType.RedSpark, formula);
            }

            return null;
        }
    }
}