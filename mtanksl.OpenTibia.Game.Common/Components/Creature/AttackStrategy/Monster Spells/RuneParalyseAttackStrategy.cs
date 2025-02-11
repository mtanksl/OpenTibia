using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class RuneParalyseAttackStrategy : IAttackStrategy
    {
        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand(target, new SlowedCondition( (ushort)(target.BaseSpeed - 101), TimeSpan.FromSeconds(10) ) ) );
            } );
        }
    }
}