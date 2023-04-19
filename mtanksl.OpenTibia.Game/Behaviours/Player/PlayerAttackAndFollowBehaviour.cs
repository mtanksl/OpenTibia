using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Game.Strategies;
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

        private IAttackStrategy attackStrategy;

        private IWalkStrategy walkStrategy;

        public PlayerAttackAndFollowBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy)
        {
            this.attackStrategy = attackStrategy;

            this.walkStrategy = walkStrategy;
        }

        private Player player;

        private Guid token;

        public override void Start(Server server)
        {
            player = (Player)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalCreatureThinkEventArgs>( (context, e) =>
            {
                return Update();
            } );
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

        public void Stop()
        {
            state = State.None;

            target = null;
        }

        private DateTime attackCooldown;

        private DateTime walkCooldown;

        private Promise Update()
        {
            if (target != null)
            {
                if (target.Tile == null)
                {
                    Stop();

                    Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),

                                                                new StopAttackAndFollowOutgoingPacket(0) );
                }
                else
                {
                    if ( !player.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        Stop();

                        Context.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),

                                                                    new StopAttackAndFollowOutgoingPacket(0) );
                    }
                    else
                    {
                        List<Promise> promises = new List<Promise>();

                        if (state == State.Follow || state == State.AttackAndFollow)
                        {
                            if (DateTime.UtcNow > walkCooldown)
                            {
                                Tile toTile = walkStrategy.GetNext(Context.Server, null, player, target);

                                if (toTile != null)
                                {
                                    walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / player.Speed);

                                    promises.Add(Context.AddCommand(new CreatureUpdateTileCommand(player, toTile) ) );
                                }
                            }
                        }

                        if (state == State.Attack || state == State.AttackAndFollow)
                        {
                            if (DateTime.UtcNow > attackCooldown)
                            {
                                Command command = attackStrategy.GetNext(Context.Server, player, target);

                                if (command != null)
                                {
                                    attackCooldown = DateTime.UtcNow.AddMilliseconds(attackStrategy.CooldownInMilliseconds);

                                    promises.Add(Context.AddCommand(command) );
                                }
                            }
                        }

                        return Promise.WhenAll(promises.ToArray() );
                    }
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