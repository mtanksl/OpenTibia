using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Components
{
    public class SpellHasteAttackStrategy : IAttackStrategy
    {
        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            var speed = Formula.HasteFormula(attacker.BaseSpeed);

            return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                    new HasteCondition(speed, new TimeSpan(0, 0, 33) ) ) );
            } );  
        }
    }
}