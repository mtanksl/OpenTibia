using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class MeleeAttackTargetAction : TargetAction
    {
        private int? min;

        private int? max;

        private TimeSpan cooldown;

        public MeleeAttackTargetAction(int? min, int? max, TimeSpan cooldown)
        {
            this.min = min;

            this.max = max;

            this.cooldown = cooldown;
        }

        private DateTime attackCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > attackCooldown)
            {
                if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
                {
                    attackCooldown = DateTime.UtcNow.Add(cooldown);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, new MeleeAttack(min, max) ) );
                }

                attackCooldown = DateTime.UtcNow.AddMilliseconds(500);
            }

            return Promise.Completed;
        }
    }
}