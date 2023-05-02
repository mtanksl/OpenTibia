using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class PlayerAttackAndFollowBehaviour : Behaviour
    {
        private enum State
        {
            None,

            Attack,

            Follow,

            AttackAndFollow
        }

        private TargetAction attackTargetAction = new MeleeAttackTargetAction(null, null, TimeSpan.FromMilliseconds(500) );

        private TargetAction walkTargetAction = new FollowWalkTargetAction();

        private Guid globalTick;

        private State state;

        private Creature target;

        public override void Start(Server server)
        {
            Player player = (Player)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target == null)
                {
                    return Promise.Completed;
                }

                if (target.IsDestroyed || !player.Tile.Position.CanHearSay(target.Tile.Position) )
                {
                    Stop();

                    Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost), new StopAttackAndFollowOutgoingPacket(0) );

                    return Promise.Completed;
                }
            
                List<Promise> promises = new List<Promise>();

                if (state == State.Follow || state == State.AttackAndFollow)
                {
                    promises.Add(walkTargetAction.Update(player, target) );
                }

                if (state == State.Attack || state == State.AttackAndFollow)
                {
                    promises.Add(attackTargetAction.Update(player, target) );
                }

                return Promise.WhenAll(promises.ToArray() );
            } );
        }

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

        public void StartFollow()
        {
            if (state == State.Attack)
            {
                state = State.AttackAndFollow;
            }
        }

        public void StopFollow()
        {
            if (state == State.AttackAndFollow)
            {
                state = State.Attack;
            }
        }

        public void Stop()
        {
            state = State.None;

            target = null;
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}