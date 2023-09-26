using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class DistanceAttackStrategy : IAttackStrategy
    {
        private ProjectileType projectileType;

        private int min;

        private int max;

        private TimeSpan cooldown;

        public DistanceAttackStrategy(ProjectileType projectileType, int min, int max, TimeSpan cooldown)
        {
            this.projectileType = projectileType;

            this.min = min;

            this.max = max;

            this.cooldown = cooldown;
        }

        public TimeSpan Cooldown
        {
            get
            {
                return cooldown;
            }
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {            
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, new DistanceAttack(projectileType, min, max) ) );
        }
    }
}