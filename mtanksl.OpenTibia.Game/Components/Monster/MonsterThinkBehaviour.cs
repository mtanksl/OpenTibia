using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
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

        public override void Start()
        {
            Monster monster = (Monster)GameObject;

            Player target = null;

            globalTick = Context.Server.EventHandlers.Subscribe<GlobalTickEventArgs>( (context, e) =>
            {
                if (target == null || target.Tile == null || target.IsDestroyed || !monster.Tile.Position.CanHearSay(target.Tile.Position) )
                {
                    Player[] players = Context.Server.Map.GetObserversOfTypePlayer(monster.Tile.Position)
                    
                        .Where(p => monster.Tile.Position.CanHearSay(p.Tile.Position) )

                        .Where(p => p.Vocation != Vocation.Gamemaster)
                    
                        .ToArray();

                    if (players.Length > 0)
                    {
                        target = Context.Server.Randomization.Take(players);
                    }
                    else
                    {
                        target = null;
                    }
                }

                if (target == null)
                {
                    if (attackStrategy != null)
                    {
                        CreatureAttackBehaviour creatureAttackBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureAttackBehaviour>(monster);

                        if (creatureAttackBehaviour != null)
                        {
                            Context.Server.GameObjectComponents.RemoveComponent(monster, creatureAttackBehaviour);
                        }
                    }

                    if (walkStrategy != null)
                    {
                        CreatureWalkBehaviour creatureWalkBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureWalkBehaviour>(monster);

                        if (creatureWalkBehaviour != null)
                        {
                            Context.Server.GameObjectComponents.RemoveComponent(monster, creatureWalkBehaviour);
                        }
                    }
                }
                else
                {
                    if (attackStrategy != null)
                    {
                        CreatureAttackBehaviour creatureAttackBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureAttackBehaviour>(monster);

                        if (creatureAttackBehaviour == null || creatureAttackBehaviour.Target != target)
                        {
                            Context.Server.GameObjectComponents.AddComponent(monster, new CreatureAttackBehaviour(attackStrategy, target) );
                        }
                    }

                    if (walkStrategy != null)
                    {
                        CreatureWalkBehaviour creatureWalkBehaviour = Context.Server.GameObjectComponents.GetComponent<CreatureWalkBehaviour>(monster);

                        if (creatureWalkBehaviour == null || creatureWalkBehaviour.Target != target)
                        {
                            Context.Server.GameObjectComponents.AddComponent(monster, new CreatureWalkBehaviour(walkStrategy, target) );
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