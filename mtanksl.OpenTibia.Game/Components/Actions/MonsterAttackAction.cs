using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class MonsterAttackAction : BehaviourAction
    {
        private IAttackStrategy attackStrategy;

        private TimeSpan cooldown;

        public MonsterAttackAction(IAttackStrategy attackStrategy, TimeSpan cooldown)
        {
            this.attackStrategy = attackStrategy;

            this.cooldown = cooldown;
        }

        private DateTime attackCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > attackCooldown)
            {
                Command command = attackStrategy.GetNext(Context.Current.Server, attacker, target);

                if (command != null)
                {
                    attackCooldown = DateTime.UtcNow.Add(cooldown);

                    return Context.Current.AddCommand(command);
                }

                attackCooldown = DateTime.UtcNow.AddSeconds(2);
            }

            return Promise.Completed;
        }
    }
}