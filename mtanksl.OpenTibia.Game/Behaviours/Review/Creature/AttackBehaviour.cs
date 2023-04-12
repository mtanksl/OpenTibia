﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game.Components
{
    public class AttackBehaviour : PeriodicBehaviour
    {
        private IAttackStrategy attackStrategy;

        public AttackBehaviour(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Attack_Behaviour_" + creature.Id;
        }

        private bool running = false;

        public override void Update()
        {
            if (running)
            {
                return;
            }

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.CanHearSay(observer.Tile.Position) )
                {
                    var command = attackStrategy.GetNext(Context, creature, observer);

                    if (command != null)
                    {
                        running = true;

                        Context.AddCommand(command).Then( () =>
                        {
                            return Promise.Delay(Context.Server, key, attackStrategy.CooldownInMilliseconds);

                        } ).Then( () =>
                        {
                            running = false;

                            Update();
                        } );
                    }

                    break;
                }
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}