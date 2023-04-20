using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Game.Strategies;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class CreatureAttackBehaviour : Behaviour
    {
        private IAttackStrategy attackStrategy;

        public CreatureAttackBehaviour(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        public override bool IsUnique
        {
            get
            {
                return true;
            }
        }

        private Creature creature;

        private Guid token;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalCreatureThinkEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private DateTime attackCooldown;

        private Promise Update()
        {
            if (DateTime.UtcNow > attackCooldown)
            {
                var target = Context.Server.GameObjects.GetPlayers()
                    .Where(p => creature.Tile.Position.CanHearSay(p.Tile.Position) )
                    .FirstOrDefault();

                if (target != null)
                {
                    Command command = attackStrategy.GetNext(Context.Server, creature, target);

                    if (command != null)
                    {
                        attackCooldown = DateTime.UtcNow.AddMilliseconds(attackStrategy.CooldownInMilliseconds);

                        return Context.AddCommand(command);
                    }
                    else
                    {
                        attackCooldown = DateTime.UtcNow.AddSeconds(2);
                    }
                }
                else
                {
                    attackCooldown = DateTime.UtcNow.AddSeconds(2);
                }
            }

            return Promise.Completed;
        }

        public override void Stop(Server server)
        {
            Context.Server.EventHandlers.Unsubscribe<GlobalCreatureThinkEventArgs>(token);
        }
    }
}