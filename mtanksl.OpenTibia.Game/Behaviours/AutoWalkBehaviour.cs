using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class AutoWalkBehaviour : PeriodicBehaviour
    {
        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "AutoWalk" + creature.Id;
        }

        private bool running = false;

        public override void Update(Context context)
        {
            if (running)
            {
                return;
            }

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.CanSee(observer.Tile.Position) )
                {
                    foreach (var direction in new[] { Direction.East, Direction.North, Direction.West, Direction.South }.Shuffle() )
                    {
                        Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                        if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) )
                        {

                        }
                        else
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

                            break;
                        }
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