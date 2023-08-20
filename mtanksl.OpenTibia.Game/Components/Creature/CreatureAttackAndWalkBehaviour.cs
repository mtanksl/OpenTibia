using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureAttackAndWalkBehaviour : Behaviour
    {
        private enum State
        {
            None,

            Attack,

            Follow,

            AttackAndFollow
        }

        private IAttackStrategy attackStrategy;

        private IWalkStrategy walkStrategy;

        public CreatureAttackAndWalkBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy)
        {
            this.attackStrategy = attackStrategy;

            this.walkStrategy = walkStrategy;
        }

        private State state;

        private Creature target;

        public void Attack(Creature creature)
        {
            state = State.Attack;

            target = creature;
        }

        public void Follow(Creature creature)
        {
            state = State.Follow;

            target = creature;
        }

        public void AttackAndFollow(Creature creature)
        {
            state = State.AttackAndFollow;

            target = creature;
        }

        public void StopAttackAndFollow()
        {
            state = State.None;

            target = null;
        }

        private Guid globalTick;

        public override void Start()
        {
            Creature creature = (Creature)GameObject;

            DateTime lastAttack = DateTime.MinValue;

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
                        if (DateTime.UtcNow > lastAttack)
                        {
                            if (attackStrategy.CanAttack(creature, target) )
                            {
                                lastAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                                return attackStrategy.Attack(creature, target);
                            }

                            lastAttack = DateTime.UtcNow.AddMilliseconds(500);
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