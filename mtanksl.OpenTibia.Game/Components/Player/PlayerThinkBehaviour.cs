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

        private DateTime nextAttack = DateTime.MinValue;

        private DateTime nextWalk = DateTime.MinValue;

        public override void Start()
        {
            Player player = (Player)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
            {
                if (target != null)
                {
                    if (target.Tile == null || target.IsDestroyed || target.Tile.ProtectionZone || player.Tile.ProtectionZone)
                    {
                        StopAttackAndFollow();

                        Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );
                    }
                    else
                    {
                        if ( !player.Tile.Position.CanHearSay(target.Tile.Position) )
                        {
                            StopAttackAndFollow();

                            Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost) );

                            Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );
                        }
                        else
                        {
                            if (state == State.Follow || state == State.AttackAndFollow)
                            {
                                if (DateTime.UtcNow >= nextWalk)
                                {
                                    if (walkStrategy != null)
                                    {
                                        Tile toTile;

                                        if (walkStrategy.CanWalk(player, target, out toTile) )
                                        {
                                            nextWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / player.Speed);

                                            await Context.AddCommand(new CreatureMoveCommand(player, toTile) );
                                        }
                                    }
                                }
                            }

                            if (state == State.Attack || state == State.AttackAndFollow)
                            {
                                if (DateTime.UtcNow >= nextAttack)
                                {
                                    if (attackStrategy != null)
                                    {
                                        if (attackStrategy.CanAttack(player, target) )
                                        {
                                            nextAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                                            await attackStrategy.Attack(player, target);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalTickEventArgs>(globalTick);
        }
    }
}