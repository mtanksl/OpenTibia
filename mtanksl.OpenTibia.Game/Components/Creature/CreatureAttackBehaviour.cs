using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureAttackBehaviour : Behaviour
    {
        private IAttackStrategy attackStrategy;

        public CreatureAttackBehaviour(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        private Creature target;

        public void Attack(Creature creature)
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

            DateTime lastCreatureAttack = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target != null)
                {
                    if (target.Tile == null || target.IsDestroyed)
                    {
                        StopAttackAndFollow();
                    }
                    else
                    {
                        if (DateTime.UtcNow > lastCreatureAttack)
                        {
                            if (attackStrategy.CanAttack(creature, target) )
                            {
                                lastCreatureAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                                return attackStrategy.Attack(creature, target);
                            }

                            lastCreatureAttack = DateTime.UtcNow.AddMilliseconds(500);
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