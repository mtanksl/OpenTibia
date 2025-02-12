using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public abstract class BaseDistanceMonsterAttackPlugin : MonsterAttackPlugin
    {
        private ProjectileType projectileType;

        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        public BaseDistanceMonsterAttackPlugin(ProjectileType projectileType, MagicEffectType? magicEffectType, DamageType damageType)
        {
            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;
        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, 
                
                new DamageAttack(projectileType, magicEffectType, damageType, min, max) ) );
        }
    }
}