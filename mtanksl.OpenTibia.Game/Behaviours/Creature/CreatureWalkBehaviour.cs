using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Game.Strategies;
using System;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkBehaviour : Behaviour
    {
        private IChooseTargetStrategy chooseTargetStrategy;

        private IWalkStrategy walkStrategy;

        public CreatureWalkBehaviour(IChooseTargetStrategy chooseTargetStrategy, IWalkStrategy walkStrategy)
        {
            this.chooseTargetStrategy = chooseTargetStrategy;

            this.walkStrategy = walkStrategy;
        }

        public override bool IsUnique
        {
            get
            {
                return true;
            }
        }

        private Creature attacker;

        private Tile spawn;

        private Guid token;

        public override void Start(Server server)
        {
            attacker = (Creature)GameObject;

            token = Context.Server.EventHandlers.Subscribe<GlobalCreatureThinkEventArgs>( (context, e) =>
            {
                return Update();
            } );
        }

        private DateTime walkCooldown;

        private Promise Update()
        {
            if (DateTime.UtcNow > walkCooldown)
            {
                var target = chooseTargetStrategy.GetNext(Context.Server, attacker);

                if (target != null)
                {
                    if (spawn == null)
                    {
                        spawn = attacker.Tile;
                    }

                    Tile toTile = walkStrategy.GetNext(Context.Server, spawn, attacker, target);

                    if (toTile != null)
                    {
                        walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / attacker.Speed);

                       return Context.AddCommand(new CreatureUpdateTileCommand(attacker, toTile) );
                    }
                    else
                    {
                        walkCooldown = DateTime.UtcNow.AddSeconds(2);
                    }
                }
                else
                {
                    walkCooldown = DateTime.UtcNow.AddSeconds(2);
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