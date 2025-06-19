using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public abstract class BaseRuneTargetMonsterAttackPlugin : MonsterAttackPlugin
    {
        private ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        private Condition condition;

        public BaseRuneTargetMonsterAttackPlugin(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType)

            : this(projectileType, magicEffectType, damageType, null)
        {

        }

        public BaseRuneTargetMonsterAttackPlugin(ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, Condition condition)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.condition = condition;
        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            if (Context.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {            
            return Context.AddCommand(new CreatureAttackCreatureCommand(attacker, target, 
                
                new DamageAttack(projectileType, magicEffectType, damageType, min, max, true),

                condition) );
        }
    }
}