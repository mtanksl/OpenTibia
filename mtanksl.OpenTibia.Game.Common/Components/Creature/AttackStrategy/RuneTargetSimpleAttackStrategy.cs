using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class RuneTargetSimpleAttackStrategy : IAttackStrategy
    {
        private ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        private int min;

        private int max;

        private Condition condition;

        public RuneTargetSimpleAttackStrategy(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max)

            : this(projectileType, magicEffectType, damageType, min, max, null)
        {

        }

        public RuneTargetSimpleAttackStrategy(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max, Condition condition)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.min = min;

            this.max = max;

            this.condition = condition;
        }

        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, 
                
                new DamageAttack(projectileType, magicEffectType, damageType, min, max),

                condition) );
        }
    }
}