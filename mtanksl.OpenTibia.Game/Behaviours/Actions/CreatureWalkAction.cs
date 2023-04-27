using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkAction : BehaviourAction
    {
        private IWalkStrategy walkStrategy;

        public CreatureWalkAction(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private Tile spawn;

        private DateTime walkCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > walkCooldown)
            {
                if (spawn == null)
                {
                    spawn = attacker.Tile;
                }

                Tile toTile = walkStrategy.GetNext(Context.Current.Server, spawn, attacker, target);

                if (toTile != null)
                {
                    walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / attacker.Speed);

                    return Context.Current.AddCommand(new CreatureWalkCommand(attacker, toTile) );
                }

                walkCooldown = DateTime.UtcNow.AddSeconds(2);
            }

            return Promise.Completed;
        }
    }
}