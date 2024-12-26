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
        private IWalkStrategy idleWalkStrategy;
        private IChangeTargetStrategy changeTargetStrategy;
        private ITargetStrategy targetStrategy;

        public MonsterThinkBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy, IWalkStrategy idleWalkStrategy, IChangeTargetStrategy changeTargetStrategy, ITargetStrategy targetStrategy)
        {
            this.attackStrategy = attackStrategy;
            this.walkStrategy = walkStrategy;
            this.idleWalkStrategy = idleWalkStrategy;
            this.changeTargetStrategy = changeTargetStrategy;
            this.targetStrategy = targetStrategy;
        }

        private Monster monster;

        private Player target;

        private bool hasVisiblePlayers;

        private Player current;

        private string attackingKey = Guid.NewGuid().ToString();

        private string followingKey = Guid.NewGuid().ToString();

        private DateTime nextAttack = DateTime.MinValue;

        private DateTime nextWalk = DateTime.MinValue;

        private void StartThreads()
        {
            current = target;

            if (attackStrategy != null)
            {
                Promise.Run(async () =>
                {
                    while (true)
                    {
                        await Promise.Delay(attackingKey, nextAttack - DateTime.UtcNow);

                        if (current.Tile == null || 
                            current.IsDestroyed || 
                            current.Tile.ProtectionZone || 
                            !monster.Tile.Position.CanHearSay(current.Tile.Position) )
                        {
                            StopThreads();

                            break;
                        }

                        if (attackStrategy.CanAttack(monster, current) )
                        {
                            await attackStrategy.Attack(monster, current);

                            nextAttack = DateTime.UtcNow.AddSeconds(2);
                        }
                        else
                        {
                            nextAttack = DateTime.UtcNow.AddSeconds(1);
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
                        await Promise.Delay(followingKey, nextWalk - DateTime.UtcNow);

                        if (current.Tile == null || 
                            current.IsDestroyed || 
                            current.Tile.ProtectionZone || 
                            !monster.Tile.Position.CanHearSay(current.Tile.Position) )
                        {
                            StopThreads();

                            break;
                        }

                        Tile toTile;

                        if (walkStrategy.CanWalk(monster, current, out toTile) )
                        {
                            MoveDirection moveDirection = monster.Tile.Position.ToMoveDirection(toTile.Position).Value;

                            await Context.Current.AddCommand(new CreatureMoveCommand(monster, toTile) );

                            int diagonalCost = (moveDirection == MoveDirection.NorthWest || 
                                                moveDirection == MoveDirection.NorthEast || 
                                                moveDirection == MoveDirection.SouthWest || 
                                                moveDirection == MoveDirection.SouthEast) ? 2 : 1;

                            nextWalk = DateTime.UtcNow.AddMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.Speed / monster.Speed);
                        }
                        else
                        {
                            nextWalk = DateTime.UtcNow.AddSeconds(1);
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
            monster = (Monster)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[monster.Id % GlobalTickEventArgs.Instance.Length], async (context, e) =>
            {
                if (Math.Abs(monster.Tile.Position.X - monster.Spawn.Position.X) > Context.Server.Config.GameplayMonsterDeSpawnRadius || 
                    Math.Abs(monster.Tile.Position.Y - monster.Spawn.Position.Y) > Context.Server.Config.GameplayMonsterDeSpawnRadius ||
                    Math.Abs(monster.Tile.Position.Z - monster.Spawn.Position.Z) > Context.Server.Config.GameplayMonsterDeSpawnRange)
                {
                    await Context.AddCommand(new ShowMagicEffectCommand(monster, MagicEffectType.Puff) );

                    await Context.AddCommand(new CreatureDestroyCommand(monster) );
                }
                else
                {
                    if (target == null || 
                        target.Tile == null || 
                        target.IsDestroyed || 
                        target.Tile.ProtectionZone ||
                        !monster.Tile.Position.CanHearSay(target.Tile.Position) || 
                        changeTargetStrategy.ShouldChange(monster, target) )
                    {
                        Player[] visiblePlayers = Context.Server.Map.GetObserversOfTypePlayer(monster.Tile.Position)
                            .Where(p => p.Rank != Rank.Gamemaster &&
                                        p.Rank != Rank.AccountManager &&
                                        monster.Tile.Position.CanSee(p.Tile.Position) )
                            .ToArray();

                        if (visiblePlayers.Length > 0)
                        {
                            target = targetStrategy.GetTarget(monster, visiblePlayers);

                            hasVisiblePlayers = true;
                        }
                        else
                        {
                            target = null;

                            hasVisiblePlayers = false;
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

                    if (hasVisiblePlayers && target == null && idleWalkStrategy != null)
                    {
                        Tile toTile;

                        if (idleWalkStrategy.CanWalk(monster, null, out toTile) )
                        {
                            await Context.Current.AddCommand(new CreatureMoveCommand(monster, toTile) );
                        }
                    }
                }
            } );            
        }

        public override void Stop()
        {
            StopThreads();

            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}