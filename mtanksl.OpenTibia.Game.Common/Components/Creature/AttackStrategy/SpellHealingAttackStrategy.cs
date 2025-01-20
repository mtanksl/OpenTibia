using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class SpellHealingAttackStrategy : IAttackStrategy
    {
        private int min;

        private int max;

        public SpellHealingAttackStrategy(int min, int max)
        {
            this.min = min;

            this.max = max;
        }

        public PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker,

                new HealingAttack(MagicEffectType.BlueShimmer, min, max) ) );            
        }
    }
}