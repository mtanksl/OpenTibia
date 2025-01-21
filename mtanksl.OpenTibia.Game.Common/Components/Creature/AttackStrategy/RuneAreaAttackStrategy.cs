using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class RuneAreaAttackStrategy : IAttackStrategy
    {
        private Offset[] area;

        private ProjectileType? projectileType;

        private MagicEffectType? magicEffectType;

        private ushort? openTibiaId;

        private byte? count;

        private DamageType damageType;

        private int min;

        private int max;

        private Condition condition;

        public RuneAreaAttackStrategy(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max)

            : this(area, projectileType, magicEffectType, damageType, min, max, null)
        {

        }

        public RuneAreaAttackStrategy(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, DamageType damageType, int min, int max, Condition condition)
        {
            this.area = area;

            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.min = min;

            this.max = max;

            this.condition = condition;
        }

        public RuneAreaAttackStrategy(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, DamageType damageType, int min, int max)
        
            : this(area, projectileType, magicEffectType, openTibiaId, count, damageType, min, max, null)
        {

        }

        public RuneAreaAttackStrategy(Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, DamageType damageType, int min, int max, Condition condition)
        {
            this.area = area;

            this.projectileType = projectileType;

            this.magicEffectType = magicEffectType;

            this.openTibiaId = openTibiaId;

            this.count = count;

            this.damageType = damageType;

            this.min = min;

            this.max = max;

            this.condition = condition;
        }

        public PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            if (openTibiaId != null && count != null)
            {
                return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, projectileType, magicEffectType, openTibiaId.Value, count.Value,
                
                    new SimpleAttack(null, null, damageType, min, max),
                
                    condition) );
            }

            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, projectileType, magicEffectType, 
                
                new SimpleAttack(null, null, damageType, min, max),
                
                condition) );
        }
    }
}