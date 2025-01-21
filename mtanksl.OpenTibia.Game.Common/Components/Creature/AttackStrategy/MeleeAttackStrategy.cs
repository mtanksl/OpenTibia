using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class MeleeAttackStrategy : IAttackStrategy
    {
        private DamageType damageType;

        private int min;

        private int max;

        public MeleeAttackStrategy(DamageType damageType, int min, int max)
        {
            this.damageType = damageType;

            this.min = min;

            this.max = max;
        }

        public PromiseResult<bool> CanAttack(Creature attacker, Creature target)
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
                
                new SimpleAttack(null, null, damageType, min, max) ) );            
        }
    }
}