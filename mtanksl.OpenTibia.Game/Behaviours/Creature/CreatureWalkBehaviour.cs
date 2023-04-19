using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Game.Strategies;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkBehaviour : Behaviour
    {
        private IWalkStrategy walkStrategy;

        public CreatureWalkBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private Creature creature;

        private Tile spawn;

        private Guid token;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalCreatureThinkEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private DateTime walkCooldown;

        private Promise Update()
        {
            if (DateTime.UtcNow > walkCooldown)
            {
                var target = Context.Server.GameObjects.GetPlayers()
                    .Where(p => creature.Tile.Position.CanHearSay(p.Tile.Position) )
                    .FirstOrDefault();

                if (target != null)
                {
                    if (spawn == null)
                    {
                        spawn = creature.Tile;
                    }

                    Tile toTile = walkStrategy.GetNext(Context.Server, spawn, creature, target);

                    if (toTile != null)
                    {
                        walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                       return Context.AddCommand(new CreatureUpdateTileCommand(creature, toTile) );
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
            Context.Server.EventHandlers.Unsubscribe<GlobalCreatureThinkEventArgs>(token);
        }
    }
}