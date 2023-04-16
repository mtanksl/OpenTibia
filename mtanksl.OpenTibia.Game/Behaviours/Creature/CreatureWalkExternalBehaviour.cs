using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkExternalBehaviour : ThinkBehaviour
    {
        private IWalkStrategy walkStrategy;

        public CreatureWalkExternalBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private Creature creature;

        private Tile spawn;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;
        }

        private DateTime walkCooldown;

        public override Promise Update()
        {
            if (DateTime.UtcNow > walkCooldown)
            {
                var target = Context.Server.GameObjects.GetPlayers()
                    .Where(p => creature.Tile.Position.CanHearSay(p.Tile.Position) )
                    .FirstOrDefault();

                if (target != null)
                {
                    Tile toTile = walkStrategy.GetNext(Context.Server, spawn, creature, target);

                    if (toTile != null)
                    {
                        walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                       return Context.AddCommand(new CreatureUpdateParentCommand(creature, toTile) );
                    }
                    else
                    {
                        walkCooldown = DateTime.UtcNow.AddSeconds(2);
                    }
                }
                else
                {
                    walkCooldown = DateTime.UtcNow.AddSeconds(2);
                }
            }

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            
        }
    }
}