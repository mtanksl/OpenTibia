using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class MonsterThinkBehaviour : Behaviour
    {
        private IAttackStrategy attackStrategy;

        private IWalkStrategy walkStrategy;

        public MonsterThinkBehaviour(IAttackStrategy attackStrategy, IWalkStrategy walkStrategy)
        {
            this.attackStrategy = attackStrategy;

            this.walkStrategy = walkStrategy;
        }

        private Guid globalTick;

        private Player target = null;

        private bool hasVisiblePlayers = false;

        private DateTime nextAttack = DateTime.MinValue;

        private DateTime nextWalk = DateTime.MinValue;

        public override void Start()
        {
            Monster monster = (Monster)GameObject;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance[monster.Id % 10], async (context, e) =>
            {
                if (Math.Abs(monster.Tile.Position.X - monster.Spawn.Position.X) > Context.Server.Config.GameplayMonsterDeSpawnRadius || Math.Abs(monster.Tile.Position.Y - monster.Spawn.Position.Y) > Context.Server.Config.GameplayMonsterDeSpawnRadius || Math.Abs(monster.Tile.Position.Z - monster.Spawn.Position.Z) > Context.Server.Config.GameplayMonsterDeSpawnRange)
                {
                    await Context.AddCommand(new ShowMagicEffectCommand(monster, MagicEffectType.Puff) );

                    await Context.AddCommand(new CreatureDestroyCommand(monster) );
                }
                else
                {
                    if (target == null || target.Tile == null || target.IsDestroyed || target.Tile.ProtectionZone || !monster.Tile.Position.CanHearSay(target.Tile.Position) )
                    {
                        Player[] visiblePlayers = Context.Server.Map.GetObserversOfTypePlayer(monster.Tile.Position)
                            .Where(p => p.Rank != Rank.Gamemaster && 
                                        monster.Tile.Position.CanSee(p.Tile.Position) )
                            .ToArray();

                        if (visiblePlayers.Length > 0)
                        {
                            Player[] targets = visiblePlayers
                                .Where(p => !p.Tile.ProtectionZone &&
                                            monster.Tile.Position.CanHearSay(p.Tile.Position) )
                                .ToArray();

                            if (targets.Length > 0)
                            {
                                target = Context.Server.Randomization.Take(targets);
                            }
                            else
                            {
                                target = null;
                            }
                            
                            hasVisiblePlayers = true;
                        }
                        else
                        {
                            target = null;

                            hasVisiblePlayers = false;
                        }
                    }

                    if (target == null)
                    {
                        if (hasVisiblePlayers)
                        {
                            if (DateTime.UtcNow >= nextWalk)
                            {                            
                                Tile toTile;

                                if (RandomWalkStrategy.Instance.CanWalk(monster, null, out toTile) )
                                {
                                    nextWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / monster.Speed);

                                    await Context.Current.AddCommand(new CreatureMoveCommand(monster, toTile) );
                                }
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.UtcNow >= nextWalk)
                        {
                            if (walkStrategy != null)
                            {
                                Tile toTile;

                                if (walkStrategy.CanWalk(monster, target, out toTile) )
                                {
                                    nextWalk = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / monster.Speed);

                                    await Context.Current.AddCommand(new CreatureMoveCommand(monster, toTile) );
                                }
                            }
                        }

                        if (DateTime.UtcNow >= nextAttack)
                        {
                            if (attackStrategy != null)
                            {
                                if (attackStrategy.CanAttack(monster, target) )
                                {
                                    nextAttack = DateTime.UtcNow.Add(TimeSpan.FromSeconds(2) );

                                    await attackStrategy.Attack(monster, target);
                                }
                            }
                        }
                    }
                }                
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}