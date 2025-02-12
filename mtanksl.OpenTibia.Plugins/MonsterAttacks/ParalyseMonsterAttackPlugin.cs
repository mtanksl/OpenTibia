using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class ParalyseMonsterAttackPlugin : MonsterAttackPlugin
    {
        public ParalyseMonsterAttackPlugin()
        {
            
        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand(target, new SlowedCondition( (ushort)(target.BaseSpeed - 101), TimeSpan.FromSeconds(10) ) ) );
            } );
        }        
    }
}