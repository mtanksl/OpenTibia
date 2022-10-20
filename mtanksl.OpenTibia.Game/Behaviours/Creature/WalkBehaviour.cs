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

        public override void Update(Context context)
        {
            if (running)
            {
                return;
            }

            if (spawn == null)
            {
                spawn = creature.Tile;
            }

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.CanSee(observer.Tile.Position) )
                {
                    var toTile = walkStrategy.GetNext(context, spawn, creature, observer);

                    if (toTile != null)
                    {
                        running = true;

                        context.AddCommand(new CreatureUpdateParentCommand(creature, toTile) ).Then(ctx =>
                        {
                            return Promise.Delay(ctx.Server, key, 1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                        } ).Then(ctx =>
                        {
                            running = false;

                            Update(ctx);
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