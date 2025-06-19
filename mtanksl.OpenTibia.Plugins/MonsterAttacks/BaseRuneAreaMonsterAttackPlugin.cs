using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public abstract class BaseRuneAreaMonsterAttackPlugin : MonsterAttackPlugin
    {
        private Offset[] area;

        private ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        private ushort? openTibiaId;

        private byte? count;

        private DamageType damageType;

        private Condition condition;

        public BaseRuneAreaMonsterAttackPlugin(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType)

            : this(area, projectileType, magicEffectType, damageType, null)
        {

        }

        public BaseRuneAreaMonsterAttackPlugin(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, Condition condition)
        {
            this.area = area;

            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.condition = condition;
        }

        public BaseRuneAreaMonsterAttackPlugin(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, DamageType damageType)
        
            : this(area, projectileType, magicEffectType, openTibiaId, count, damageType, null)
        {

        }

        public BaseRuneAreaMonsterAttackPlugin(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, DamageType damageType, Condition condition)
        {
            this.area = area;

            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.openTibiaId = openTibiaId;

            this.count = count;

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
            if (openTibiaId != null && count != null)
            {
                return Context.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, projectileType, magicEffectType, openTibiaId.Value, count.Value,
                
                    new DamageAttack(null, null, damageType, min, max, true),
                
                    condition) );
            }

            return Context.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, projectileType, magicEffectType, 
                
                new DamageAttack(null, null, damageType, min, max, true),
                
                condition) );
        }
    }
}