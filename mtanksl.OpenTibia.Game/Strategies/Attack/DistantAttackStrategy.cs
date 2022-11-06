using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Strategies
{
    public class DistantAttackStrategy : IAttackStrategy
    {
        private ProjectileType projectileType;

        private int cooldownInMilliseconds;

        private Func<Creature, Creature, int> formula;

        public DistantAttackStrategy(ProjectileType projectileType, int cooldownInMilliseconds, Func<Creature, Creature, int> formula)
        {
            this.projectileType = projectileType;

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

        public Command GetNext(Context context, Creature creature, Creature target)
        {
            return CombatCommand.TargetAttack(creature, target, projectileType, MagicEffectType.RedSpark, formula);
        }
    }    
}