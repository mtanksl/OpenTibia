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
            if (target is Monster monster && monster.Metadata.ImmuneToParalyse)
            {
                return Context.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.GreenShimmer) );
            }
            else
            {
                return Context.AddCommand(new ShowMagicEffectCommand(target, MagicEffectType.GreenShimmer) ).Then( () =>
                {
                    return Context.AddCommand(new CreatureAddConditionCommand(target, new SlowedCondition(-101, TimeSpan.FromSeconds(10) ) ) );
                } );
            }
        }
    }
}