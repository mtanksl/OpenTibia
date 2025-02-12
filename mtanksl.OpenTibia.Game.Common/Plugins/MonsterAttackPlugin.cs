using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System.Collections.Generic;

namespace OpenTibia.Game.Plugins
{
    public abstract class MonsterAttackPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnAttacking(Monster attacker, Creature target);

        public abstract Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes);
    }
}