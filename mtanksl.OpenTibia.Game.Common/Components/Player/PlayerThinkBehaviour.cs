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
        private IAttackStrategy attackStrategy;
        private IWalkStrategy walkStrategy;

        public PlayerThinkBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy)
        {
            this.attackStrategy = attackStrategy;
            this.walkStrategy = walkStrategy;
        }

        public Creature Target
        {
            get
            {
                return target;
            }
        }

        public void Attack(Creature creature)
        {
            target = creature;

            if ( !attacking)
            {
                attacking = true;

                StartAttackThread();
            }

            if (following)
            {
                following = false;

                StopFollowThread();
            }
        }

        public void Follow(Creature creature)
        {            
            target = creature;

            if (attacking)
            {
                attacking = false;

                StopAttackThread();
            }

            if ( !following)
            {
                following = true;

                StartFollowThread();
            }
        }

        public void AttackAndFollow(Creature creature)
        {
            target = creature;

            if ( !attacking)
            {
                attacking = true;

                StartAttackThread();
            }

            if ( !following)
            {
                following = true;

                StartFollowThread();
            }
        }

        public void StartFollow()
        {
            if (attacking && !following)
            {
                following = true;

                StartFollowThread();
            }
        }

        public void StopFollow()
        {
            if (attacking && following)
            {
                following = false;

                StopFollowThread();
            }
        }

        public void StopAttackAndFollow()
        {
            target = null;

            if (attacking)
            {
                attacking = false;

                StopAttackThread();
            }

            if (following)
            {
                following = false;

                StopFollowThread();
            }
        }

        private Player player;

        private int ticks;

        private Guid globalTick;
                
        private string walkingKey = Guid.NewGuid().ToString();

        private DateTime nextWalk;

        private Creature target;

        public override void Start()
        {
            player = (Player)GameObject;            
        }

        private bool attacking;

        private void StartAttackThread()
        {
            ticks = 1000;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(player.Id), async (context, e) =>
            {
                ticks -= e.Ticks;

                while (ticks <= 0)
                {
                    ticks += 1000;

                    if (target == null || target.Tile == null || target.IsDestroyed || target.Tile.ProtectionZone || player.Tile.ProtectionZone)
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
                            if (await attackStrategy.CanAttack(1000, player, target) )
                            {
                                await attackStrategy.Attack(player, target);
                            }
                        }
                    }
                }
            } );
        }

        private bool following;

        private void StartFollowThread()
        {
            Promise.Run(async () =>
            {
                while (true)
                {
                    await Promise.Delay(walkingKey, nextWalk - DateTime.UtcNow);

                    if (target == null || target.Tile == null || target.IsDestroyed || target.Tile.ProtectionZone || player.Tile.ProtectionZone)
                    {
                        StopAttackAndFollow();

                        Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );

                        break;
                    }
                    else
                    {
                        if ( !player.Tile.Position.CanHearSay(target.Tile.Position) )
                        {
                            StopAttackAndFollow();

                            Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost) );

                            Context.AddPacket(player, new StopAttackAndFollowOutgoingPacket(0) );
                        
                            break;
                        }
                        else
                        {
                            Tile toTile;

                            if (walkStrategy.CanWalk(player, target, out toTile) )
                            {
                                MoveDirection moveDirection = player.Tile.Position.ToMoveDirection(toTile.Position).Value;

                                await Context.AddCommand(new CreatureMoveCommand(player, toTile) );

                                int diagonalCost = (moveDirection == MoveDirection.NorthWest ||
                                                    moveDirection == MoveDirection.NorthEast ||
                                                    moveDirection == MoveDirection.SouthWest ||
                                                    moveDirection == MoveDirection.SouthEast) ? 2 : 1;

                                nextWalk = DateTime.UtcNow.AddMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.GroundSpeed / player.Speed);
                            }
                            else
                            {
                                nextWalk = DateTime.UtcNow.AddMilliseconds(500);
                            }
                        }
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

        private void StopAttackThread()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }

        private void StopFollowThread()
        {
            Context.Server.CancelQueueForExecution(walkingKey);
        }

        public override void Stop()
        {
            StopAttackAndFollow();
        }
    }
}