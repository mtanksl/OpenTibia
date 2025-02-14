using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
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

        private Guid creatureAppearEventArgs;

        private Guid creatureDisappearEventArgs;

        private HashSet<Player> near = new HashSet<Player>();

        private int ticks;

        private Guid globalTick;

        private string walkingKey = Guid.NewGuid().ToString();

        private DateTime nextWalk;

        private Creature target;

        public override void Start()
        {
            monster = (Monster)GameObject;

            creatureAppearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureAppearEventArgs>(monster, (context, e) => 
            {
                if (e.Creature is Player player && player.Rank != Rank.Gamemaster && player.Rank != Rank.AccountManager)
                {
                    if (near.Count == 0)
                    {
                        StartAttackThread();

                        StartFollowThread();
                    }

                    near.Add(player);
                }

                return Promise.Completed; 
            } );

            creatureDisappearEventArgs = Context.Server.GameObjectEventHandlers.Subscribe<CreatureDisappearEventArgs>(monster, (context, e) =>
            {
                if (e.Creature is Player player && player.Rank != Rank.Gamemaster && player.Rank != Rank.AccountManager)
                {
                    near.Remove(player);

                    if (near.Count == 0)
                    {
                        target = null;

                        StopAttackThread();

                        StopFollowThread();
                    }
                }

                return Promise.Completed;
            } );
        }

        private void StartAttackThread()
        {
            ticks = 1000;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(monster.Id), async (context, e) =>
            {
                ticks -= e.Ticks;

                while (ticks <= 0)
                {
                    ticks += 1000;

                    if (Math.Abs(monster.Tile.Position.X - monster.Spawn.Position.X) > Context.Server.Config.GameplayMonsterDeSpawnRadius || 
                        Math.Abs(monster.Tile.Position.Y - monster.Spawn.Position.Y) > Context.Server.Config.GameplayMonsterDeSpawnRadius || 
                        Math.Abs(monster.Tile.Position.Z - monster.Spawn.Position.Z) > Context.Server.Config.GameplayMonsterDeSpawnRange)
                    {
                        if (Context.Server.Config.GameplayMonsterRemoveOnDeSpawn)
                        {
                            await Context.AddCommand(new ShowMagicEffectCommand(monster, MagicEffectType.Puff) );

                            await Context.AddCommand(new CreatureDestroyCommand(monster) );

                            break;
                        }
                        else
                        {
                            await Context.AddCommand(new ShowMagicEffectCommand(monster, MagicEffectType.Puff) );

                            await Context.AddCommand(new CreatureMoveCommand(monster, monster.Spawn) );

                            await Context.AddCommand(new ShowMagicEffectCommand(monster, MagicEffectType.Teleport) );
                        }

                        target = null;
                    }

                    if (target == null || target.Tile == null || target.IsDestroyed || target.Tile.ProtectionZone || !monster.Tile.Position.CanHearSay(target.Tile.Position) || ( !monster.Metadata.ImmuneToInvisible && target.Stealth) || changeTargetStrategy.ShouldChange(1000, monster, target) )
                    {
                        Player[] players;

                        if (monster.Metadata.ImmuneToInvisible)
                        {
                            players = near
                                .Where(p => !p.Tile.ProtectionZone && 
                                            monster.Tile.Position.CanHearSay(p.Tile.Position) )
                                .ToArray();
                        }
                        else
                        {
                            players = near
                                .Where(p => !p.Stealth &&
                                            !p.Tile.ProtectionZone && 
                                            monster.Tile.Position.CanHearSay(p.Tile.Position) )
                                .ToArray();
                        }

                        if (players.Length > 0)
                        {
                            target = targetStrategy.GetTarget(1000, monster, players);
                        }
                        else
                        {
                            target = null;
                        }
                    }

                    if (target != null && await attackStrategy.CanAttack(1000, monster, target) )
                    {
                        await attackStrategy.Attack(monster, target);
                    }
                }
            } );
        }

        private void StartFollowThread()
        {
            Promise.Run(async () =>
            {
                while (true)
                {
                    await Promise.Delay(walkingKey, nextWalk - DateTime.UtcNow);

                    Tile toTile;

                    if ( (target == null ? idleWalkStrategy : walkStrategy).CanWalk(monster, target, out toTile) )
                    {
                        MoveDirection moveDirection = monster.Tile.Position.ToMoveDirection(toTile.Position).Value;

                        await Context.AddCommand(new CreatureMoveCommand(monster, toTile) );

                        int diagonalCost = (moveDirection == MoveDirection.NorthWest ||
                                            moveDirection == MoveDirection.NorthEast ||
                                            moveDirection == MoveDirection.SouthWest ||
                                            moveDirection == MoveDirection.SouthEast) ? 2 : 1;

                        nextWalk = DateTime.UtcNow.AddMilliseconds(diagonalCost * 1000 * toTile.Ground.Metadata.GroundSpeed / monster.Speed);
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
            Context.Server.GameObjectEventHandlers.Unsubscribe(creatureAppearEventArgs);

            Context.Server.GameObjectEventHandlers.Unsubscribe(creatureDisappearEventArgs);

            if (near.Count > 0)
            {
                near.Clear();
                                      
                target = null;

                StopAttackThread();

                StopFollowThread();
            }
        }
    }
}