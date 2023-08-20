using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkBehaviour : Behaviour
    {
        private IWalkStrategy walkStrategy;

        public CreatureWalkBehaviour(IWalkStrategy walkStrategy, Creature target)
        {
            this.walkStrategy = walkStrategy;

            this.target = target;
        }

        private Creature target;

        public Creature Target
        {
            get
            {
                return target;
            }
        }

        private Guid globalTick;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            DateTime lastWalk = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (DateTime.UtcNow > lastWalk)
                {
                    if (target != null && (target.Tile == null || target.IsDestroyed || !creature.Tile.Position.CanHearSay(target.Tile.Position) ) )
                    {
                        Context.Server.GameObjectComponents.RemoveComponent(creature, this);
                    }
                    else
                    {                    
                        Tile toTile;

                        if (walkStrategy.CanWalk(creature, target, out toTile) )
                        {
                            lastWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                            return Context.AddCommand(new CreatureWalkCommand(creature, toTile) );
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