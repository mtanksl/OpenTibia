using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game.Components
{
    public class CreatureAttackBehaviour : PeriodicBehaviour
    {
        private IAttackStrategy attackStrategy;

        public CreatureAttackBehaviour(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "CreatureAttackBehaviour" + creature.Id;
        }

        private bool running = false;

        public override async Promise Update()
        {
            if (!running)
            {
                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (creature.Tile.Position.CanHearSay(observer.Tile.Position) )
                    {
                        Command command = attackStrategy.GetNext(Context.Server, creature, observer);

                        if (command != null)
                        {
                            running = true;

                            await Context.AddCommand(command);
                            
                            await Promise.Delay(key, attackStrategy.CooldownInMilliseconds);
                            
                            running = false;

                            await Update();
                        }

                        break;
                    }
                }
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}