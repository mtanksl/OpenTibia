using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

        private Player player;

        private Creature target;

        private State state;

        private Creature current;

        private string attackingKey = Guid.NewGuid().ToString();

        private string followingKey = Guid.NewGuid().ToString();

        public void Attack(Creature creature)
        {
            target = creature;

            state = State.Attack;
        }

        public void Follow(Creature creature)
        {
            target = creature;

            state = State.Follow;
        }

        public void AttackAndFollow(Creature creature)
        {
            target = creature;

            state = State.AttackAndFollow;
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
            target = null;

            state = State.None;
        }

        private void StartThreads()
        {
            current = target;

            if (attackStrategy != null)
            {
                Promise.Run(async () =>
                {
                    while (true)
                    {
                        if (current.Tile == null ||
                            current.IsDestroyed ||
                            current.Tile.ProtectionZone ||
                            player.Tile.ProtectionZone)
                        {
                            StopThreads();

                            StopAttackAndFollow();

                            Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );

                            break;
                        }
                        else
                        {
                            if ( !player.Tile.Position.CanHearSay(current.Tile.Position) )
                            {
                                StopThreads();

                                StopAttackAndFollow();

                                Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost) );

                                Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );
                            
                                break;
                            }
                        }

                        if ( (state == State.Attack || state == State.AttackAndFollow) && attackStrategy.CanAttack(player, current) )
                        {
                            await attackStrategy.Attack(player, current);

                            await Promise.Delay(attackingKey, TimeSpan.FromSeconds(1) );
                        }
                        else
                        {
                            await Promise.Delay(attackingKey, TimeSpan.FromMilliseconds(100) );
                        }
                    }

                } ).Catch( (ex) =>
                {
                    if (ex is PromiseCanceledException)
                    {
                        //
                    }
                    else
                    {
                        Context.Server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                    }
                } );
            }

            if (walkStrategy != null)
            {
                Promise.Run(async () =>
                {
                    while (true)
                    {
                        if (current.Tile == null ||
                            current.IsDestroyed ||
                            current.Tile.ProtectionZone ||
                            player.Tile.ProtectionZone)
                        {
                            StopThreads();

                            StopAttackAndFollow();

                            Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );

                            break;
                        }
                        else
                        {
                            if ( !player.Tile.Position.CanHearSay(current.Tile.Position) )
                            {
                                StopThreads();

                                StopAttackAndFollow();

                                Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost) );

                                Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );
                            
                                break;
                            }
                        }

                        Tile toTile;

                        if ( (state == State.Follow || state == State.AttackAndFollow) && walkStrategy.CanWalk(player, current, out toTile) )
                        {
                            MoveDirection moveDirection = player.Tile.Position.ToMoveDirection(toTile.Position).Value;

                            await Context.Current.AddCommand(new CreatureMoveCommand(player, toTile) );

                            int diagonalCost = (moveDirection == MoveDirection.NorthWest || 
                                                moveDirection == MoveDirection.NorthEast || 
                                                moveDirection == MoveDirection.SouthWest || 
                                                moveDirection == MoveDirection.SouthEast) ? 2 : 1;

                            await Promise.Delay(followingKey, TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.Speed / player.Speed) );
                        }
                        else
                        {
                            await Promise.Delay(followingKey, TimeSpan.FromMilliseconds(500) );
                        }
                    }

                } ).Catch( (ex) =>
                {
                    if (ex is PromiseCanceledException)
                    {
                        //
                    }
                    else
                    {
                        Context.Server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                    }
                } );
            }            
        }

        private void StopThreads()
        {
            Context.Server.CancelQueueForExecution(attackingKey);

            Context.Server.CancelQueueForExecution(followingKey);

            current = null;
        }

        private Guid globalTick;

        public override void Start()
        {
            player = (Player)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[player.Id % GlobalTickEventArgs.Instance.Length], (context, e) =>
            {
                if (target != null)
                {
                    if (target.Tile == null || 
                        target.IsDestroyed || 
                        target.Tile.ProtectionZone ||
                        player.Tile.ProtectionZone)
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
                    }
                }

                if (target == null && current != null)
                {
                    StopThreads();
                }
                else if (target != null && current == null)
                {
                    StartThreads();
                }
                else if (target != null && current != null)
                {
                    if (target != current)
                    {
                        StopThreads();

                        StartThreads();
                    }
                }

                return Promise.Completed;
            } );
        }

        public override void Stop()
        {
            StopThreads();

            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}