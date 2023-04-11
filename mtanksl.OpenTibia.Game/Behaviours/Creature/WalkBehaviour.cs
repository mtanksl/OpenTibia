using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game.Components
{
    public class WalkBehaviour : PeriodicBehaviour
    {
        private IWalkStrategy walkStrategy;

        public WalkBehaviour(IWalkStrategy walkStrategy)
        {
            this.walkStrategy = walkStrategy;
        }

        private Creature creature;

        private string key;

        private Tile spawn;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Walk_Behaviour_" + creature.Id;
        }

        private bool running = false;

        public override void Update()
        {
            if (running)
            {
                return;
            }

            if (spawn == null)
            {
                spawn = creature.Tile;
            }

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.CanSee(observer.Tile.Position) )
                {
                    var toTile = walkStrategy.GetNext(Context, spawn, creature, observer);

                    if (toTile != null)
                    {
                        running = true;

                        Context.AddCommand(new CreatureUpdateParentCommand(creature, toTile) ).Then( () =>
                        {
                            return Promise.Delay(Context.Server, key, 1000 * toTile.Ground.Metadata.Speed / creature.Speed);

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