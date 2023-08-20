using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkBehaviour : Behaviour
    {
        private IWalkStrategy walkStrategy;

        public CreatureWalkBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private Guid globalTick;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            DateTime lastCreatureWalk = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (DateTime.UtcNow > lastCreatureWalk)
                {
                    Tile toTile;

                    if (walkStrategy.CanWalk(creature, null, out toTile) )
                    {
                        lastCreatureWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                        return Context.AddCommand(new CreatureWalkCommand(creature, toTile) );
                    }

                    lastCreatureWalk = DateTime.UtcNow.AddMilliseconds(500);
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}