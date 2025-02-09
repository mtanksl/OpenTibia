using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class MeleeAttackStrategy : IAttackStrategy
    {
        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        private int min;

        private int max;

        public MeleeAttackStrategy(MagicEffectType? magicEffectType, DamageType damageType, int min, int max)
        {
            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.min = min;

            this.max = max;
        }

        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,
                
                new DamageAttack(null, magicEffectType, damageType, min, max) ) );            
        }
    }
}