using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class AttackAction : Action
    {
        private IAttackStrategy attackStrategy;

        private int cooldownInMilliseconds;

        public AttackAction(IAttackStrategy attackStrategy, int cooldownInMilliseconds)
        {
            this.attackStrategy = attackStrategy;

            this.cooldownInMilliseconds = cooldownInMilliseconds;
        }

        private DateTime attackCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > attackCooldown)
            {
                Command command = attackStrategy.GetNext(Context.Current.Server, attacker, target);

                if (command != null)
                {
                    attackCooldown = DateTime.UtcNow.AddMilliseconds(cooldownInMilliseconds);

                    return Context.Current.AddCommand(command);
                }

                attackCooldown = DateTime.UtcNow.AddSeconds(2);
            }

            return Promise.Completed;
        }
    }
}