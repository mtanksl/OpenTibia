using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DistanceAttackTargetAction : TargetAction
    {
        private ProjectileType projectileType;

        private int? min;

        private int? max;

        private TimeSpan cooldown;

        public DistanceAttackTargetAction(ProjectileType projectileType, int? min, int? max, TimeSpan cooldown)
        {
            this.projectileType = projectileType;

            this.min = min;

            this.max = max;

            this.cooldown = cooldown;
        }

        private DateTime attackCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > attackCooldown)
            {
                if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
                {
                    attackCooldown = DateTime.UtcNow.Add(cooldown);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, new DistanceAttack(projectileType, min, max) ) );
                }

                attackCooldown = DateTime.UtcNow.AddMilliseconds(500);
            }

            return Promise.Completed;
        }
    }
}