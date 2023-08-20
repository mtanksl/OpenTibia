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
                    target = Context.Server.Map.GetObservers(monster.Tile.Position).OfType<Player>().Where(p => p.Vocation != Vocation.Gamemaster && monster.Tile.Position.CanHearSay(p.Tile.Position) ).FirstOrDefault();
                }

                if (target != null)
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