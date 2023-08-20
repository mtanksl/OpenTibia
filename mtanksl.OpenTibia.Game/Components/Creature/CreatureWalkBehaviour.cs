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

        private Creature target;

        public void Follow(Creature creature)
        {
            target = creature;
        }

        public void StopAttackAndFollow()
        {
            target = null;
        }

        private Guid globalTick;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            DateTime lastWalk = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target != null)
                {
                    if (target.Tile == null || target.IsDestroyed || !creature.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        StopAttackAndFollow();
                    }
                    else
                    {
                        if (DateTime.UtcNow > lastWalk)
                        {
                            Tile toTile;

                            if (walkStrategy.CanWalk(creature, target, out toTile) )
                            {
                                lastWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                                return Context.AddCommand(new CreatureWalkCommand(creature, toTile) );
                            }

                            lastWalk = DateTime.UtcNow.AddMilliseconds(500);
                        }
                    }
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