using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
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

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>(async (context, e) =>
            {
                if (e.Index == monster.Id % 10)
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

                                    await Context.Current.AddCommand(new CreatureMoveCommand(monster, toTile));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.UtcNow >= nextAttack)
                        {
                            if (attackStrategy != null)
                            {
                                if (attackStrategy.CanAttack(monster, target) )
                                {
                                    nextAttack = DateTime.UtcNow.Add(attackStrategy.Cooldown);

                                    await attackStrategy.Attack(monster, target);
                                }
                            }
                        }

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