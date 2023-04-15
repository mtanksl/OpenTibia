using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game.Components
{
    public class CreatureWalkBehaviour : PeriodicBehaviour
    {
        private IWalkStrategy walkStrategy;

        public CreatureWalkBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private Creature creature;

        private string key;

        private Tile spawn;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "CreatureWalkBehaviour" + creature.Id;
        }

        private bool running = false;

        public override async Promise Update()
        {
            if (!running)
            {
                if (spawn == null)
                {
                    spawn = creature.Tile;
                }

                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (creature.Tile.Position.CanSee(observer.Tile.Position) )
                    {
                        Tile toTile = walkStrategy.GetNext(Context.Server, spawn, creature, observer);

                        if (toTile != null)
                        {
                            running = true;

                            await Context.AddCommand(new CreatureUpdateParentCommand(creature, toTile) );

                            await Promise.Delay(key, 1000 * toTile.Ground.Metadata.Speed / creature.Speed);

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