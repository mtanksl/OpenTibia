using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class SelfHealingMonsterAttackPlugin : MonsterAttackPlugin
    {
        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            return Context.AddCommand(new CreatureAttackCreatureCommand(attacker, attacker,

                new HealingAttack(min, max) ) );
        }
    }
}