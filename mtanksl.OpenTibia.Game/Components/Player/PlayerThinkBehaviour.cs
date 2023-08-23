using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class PlayerThinkBehaviour : Behaviour
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

        public PlayerThinkBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy)
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

        public void StopAttackAndFollow()
        {
            state = State.None;

            target = null;
        }

        private Guid globalTick;

        public override void Start()
        {
            Player player = (Player)GameObject;

            DateTime lastAttack = DateTime.MinValue;

            DateTime lastWalk = DateTime.MinValue;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target != null)
                {
                    if (target.Tile == null || target.IsDestroyed)
                    {
                        StopAttackAndFollow();
                           
                        Context.AddPacket(player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );
                    }
                    else
                    {
                        if ( !player.Tile.Position.CanHearSay(target.Tile.Position) )
                        {
                            StopAttackAndFollow();

                            Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),
                                                                        new StopAttackAndFollowOutgoingPacket(0) );
                        }
                        else
                        {
                            List<Promise> promises = new List<Promise>();

                            if (state == State.Follow || state == State.AttackAndFollow)
                            {
                                if (DateTime.UtcNow >= lastWalk)
                                {
                                    Tile toTile;

                                    if (walkStrategy.CanWalk(player, target, out toTile) )
                                    {
                                        lastWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / player.Speed);

                                        promises.Add(Context.AddCommand(new CreatureWalkCommand(player, toTile) ) );
                                    }
                                }
                            }

                            if (state == State.Attack || state == State.AttackAndFollow)
                            {
                                if (DateTime.UtcNow >= lastAttack)
                                {
                                    if (attackStrategy.CanAttack(player, target) )
                                    {
                                        lastAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                                        promises.Add(attackStrategy.Attack(player, target) );
                                    }
                                }
                            }

                            return Promise.WhenAll(promises.ToArray() );
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