using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class SpellBeamAttackStrategy : IAttackStrategy
    {
        private Offset[] area;

        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        private int min;

        private int max;

        private Condition condition;

        public SpellBeamAttackStrategy(Offset[] area, MagicEffectType? magicEffectType, DamageType damageType, int min, int max)

            :this(area, magicEffectType, damageType, min, max, null)
        {
            
        }

        public SpellBeamAttackStrategy(Offset[] area, MagicEffectType? magicEffectType, DamageType damageType, int min, int max, Condition condition)
        {
            this.area = area;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.min = min;

            this.max = max;

            this.condition = condition;
        }

        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, true, attacker.Tile.Position, area, null, magicEffectType, 
                        
                new DamageAttack(null, null, damageType, min, max),
                
                condition) );
        }
    }
}