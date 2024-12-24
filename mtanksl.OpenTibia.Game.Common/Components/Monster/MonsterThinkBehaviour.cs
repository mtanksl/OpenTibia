using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class MonsterThinkBehaviour : Behaviour
    {
        private IAttackStrategy attackStrategy;

        private IWalkStrategy walkStrategy;

        private IChangeTargetStrategy changeTargetStrategy;

        private ITargetStrategy targetStrategy;

        public MonsterThinkBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy, IChangeTargetStrategy changeTargetStrategy, ITargetStrategy targetStrategy)
        {
            this.attackStrategy = attackStrategy;

            this.walkStrategy = walkStrategy;

            this.changeTargetStrategy = changeTargetStrategy;

            this.targetStrategy = targetStrategy;
        }

        private Monster attacker;

        private Player target;

        private bool hasVisiblePlayers;

        private void CheckTarget()
        {
            if (target == null || target.Tile == null || target.IsDestroyed || target.Tile.ProtectionZone || !attacker.Tile.Position.CanHearSay(target.Tile.Position) || changeTargetStrategy.ShouldChange(attacker, target) )
            {
                Player[] visiblePlayers = Context.Server.Map.GetObserversOfTypePlayer(attacker.Tile.Position)
                    .Where(p => p.Rank != Rank.Gamemaster &&
                                p.Rank != Rank.AccountManager &&
                                attacker.Tile.Position.CanSee(p.Tile.Position) )
                    .ToArray();

                if (visiblePlayers.Length > 0)
                {
                    target = targetStrategy.GetTarget(attacker, visiblePlayers);

                    hasVisiblePlayers = true;
                }
                else
                {
                    target = null;

                    hasVisiblePlayers = false;
                }
            }
        }

        private Player currentTarget;

        private string attackingKey = Guid.NewGuid().ToString();

        private string followingKey = Guid.NewGuid().ToString();

        private void StartAttackAndFollow()
        {
            currentTarget = target;

            if (attackStrategy != null)
            {
                Promise.Run(async () =>
                {
                    while (true)
                    {
                        if (currentTarget.Tile == null || currentTarget.IsDestroyed || currentTarget.Tile.ProtectionZone || !attacker.Tile.Position.CanHearSay(currentTarget.Tile.Position) )
                        {
                            StopAttackAndFollow();

                            break;
                        }

                        if (attackStrategy.CanAttack(attacker, currentTarget) )
                        {
                            await attackStrategy.Attack(attacker, currentTarget);
                        }

                        await Promise.Delay(attackingKey, TimeSpan.FromSeconds(2) );
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
                        if (currentTarget.Tile == null || currentTarget.IsDestroyed || currentTarget.Tile.ProtectionZone || !attacker.Tile.Position.CanHearSay(currentTarget.Tile.Position) )
                        {
                            StopAttackAndFollow();

                            break;
                        }

                        Tile toTile;

                        if (walkStrategy.CanWalk(attacker, currentTarget, out toTile) )
                        {
                            MoveDirection moveDirection = attacker.Tile.Position.ToMoveDirection(toTile.Position).Value;

                            await Context.Current.AddCommand(new CreatureMoveCommand(attacker, toTile) );

                            int diagonalCost = (moveDirection == MoveDirection.NorthWest || moveDirection == MoveDirection.NorthEast || moveDirection == MoveDirection.SouthWest || moveDirection == MoveDirection.SouthEast) ? 2 : 1;

                            await Promise.Delay(followingKey, TimeSpan.FromMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.Speed / attacker.Speed) );
                        }
                        else
                        {
                            await Promise.Delay(followingKey, TimeSpan.FromSeconds(1) );
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

        private void StopAttackAndFollow()
        {
            Context.Server.CancelQueueForExecution(attackingKey);

            Context.Server.CancelQueueForExecution(followingKey);

            currentTarget = null;
        }

        private Guid globalTick;

        public override void Start()
        {
            attacker = (Monster)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[attacker.Id % GlobalTickEventArgs.Instance.Length], async (context, e) =>
            {
                if (Math.Abs(attacker.Tile.Position.X - attacker.Spawn.Position.X) > Context.Server.Config.GameplayMonsterDeSpawnRadius || Math.Abs(attacker.Tile.Position.Y - attacker.Spawn.Position.Y) > Context.Server.Config.GameplayMonsterDeSpawnRadius || Math.Abs(attacker.Tile.Position.Z - attacker.Spawn.Position.Z) > Context.Server.Config.GameplayMonsterDeSpawnRange)
                {
                    await Context.AddCommand(new ShowMagicEffectCommand(attacker, MagicEffectType.Puff) );

                    await Context.AddCommand(new CreatureDestroyCommand(attacker) );
                }
                else
                {
                    CheckTarget();

                    if (target == null && currentTarget != null)
                    {
                        StopAttackAndFollow();                        
                    }
                    else if (target != null && currentTarget == null)
                    {
                        StartAttackAndFollow();
                    }
                    else if (target != null && currentTarget != null)
                    {
                        if (target != currentTarget)
                        {
                            StopAttackAndFollow();

                            StartAttackAndFollow();
                        }
                    }

                    if (target == null && hasVisiblePlayers)
                    {
                        Tile toTile;

                        if (RandomWalkStrategy.Instance.CanWalk(attacker, null, out toTile) )
                        {
                            await Context.Current.AddCommand(new CreatureMoveCommand(attacker, toTile) );
                        }
                    }
                }
            } );            
        }

        public override void Stop()
        {
            StopAttackAndFollow();

            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}