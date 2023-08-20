using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureFocusBehaviour : Behaviour
    {
        public CreatureFocusBehaviour(Creature target)
        {
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

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target.Tile == null || target.IsDestroyed || !creature.Tile.Position.CanHearSay(target.Tile.Position) )
                {
                    Context.Server.GameObjectComponents.RemoveComponent(creature, this);
                }
                else
                {                    
                    var direction = creature.Tile.Position.ToDirection(target.Tile.Position);

                    if (direction != null && direction.Value != creature.Direction)
                    {
                        return Context.AddCommand(new CreatureUpdateDirectionCommand(creature, direction.Value) );
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