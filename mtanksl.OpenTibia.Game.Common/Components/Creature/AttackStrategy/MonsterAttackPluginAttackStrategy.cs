using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class MonsterAttackPluginAttackStrategy : IAttackStrategy
    {
        private string name;

        private int min;

        private int max;

        private Dictionary<string, string> attributes;

        public MonsterAttackPluginAttackStrategy(string name, int min, int max, Dictionary<string, string> attributes)
        {
            this.name = name;

            this.min = min;

            this.max = max;

            this.attributes = attributes;
        }

        private MonsterAttackPlugin monsterAttackPlugin;

        public PromiseResult<bool> CanAttack(int ticks, Creature attacker, Creature target)
        {
            monsterAttackPlugin = Context.Current.Server.Plugins.GetMonsterAttackPlugin(name);

            if (monsterAttackPlugin != null)
            {
                return monsterAttackPlugin.OnAttacking( (Monster)attacker, target);
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            return monsterAttackPlugin.OnAttack( (Monster)attacker, target, min, max, attributes);
        }
    }
}