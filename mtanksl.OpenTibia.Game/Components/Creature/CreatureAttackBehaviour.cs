using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureAttackBehaviour : Behaviour
    {
        private IAttackStrategy attackStrategy;

        public CreatureAttackBehaviour(IAttackStrategy attackStrategy, Creature target)
        {
            this.attackStrategy = attackStrategy;

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

            DateTime lastAttack = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (DateTime.UtcNow > lastAttack)
                {
                    if (target.Tile == null || target.IsDestroyed || !creature.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        Context.Server.GameObjectComponents.RemoveComponent(creature, this);
                    }
                    else
                    {                    
                        if (attackStrategy.CanAttack(creature, target) )
                        {
                            lastAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                            return attackStrategy.Attack(creature, target);
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