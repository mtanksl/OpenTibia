using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class InvisibleMonsterAttackPlugin : MonsterAttackPlugin
    {
        public InvisibleMonsterAttackPlugin()
        {

        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            if ( !attacker.Invisible)
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(attacker, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(attacker, new InvisibleCondition(TimeSpan.FromSeconds(10) ) ) );
            } );
        }
    }    
}