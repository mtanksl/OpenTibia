using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class ApproachWalkBehaviour : PeriodicBehaviour
    {
        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Approach_Walk_Behaviour_" + creature.Id;
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
                    int deltaY = creature.Tile.Position.Y - observer.Tile.Position.Y;

                    int deltaX = creature.Tile.Position.X - observer.Tile.Position.X;

                    HashSet<Direction> allowed = new HashSet<Direction>();

                    if (deltaY < 0)
                    {
                        allowed.Add(Direction.South);
                    }
                    else if (deltaY > 0)
                    {                     
                        allowed.Add(Direction.North);
                    }

                    if (deltaX < 0)
                    {
                        allowed.Add(Direction.East);
                    }
                    else if (deltaX > 0)
                    {
                        allowed.Add(Direction.West);
                    }

                    if ( !Walking(creature, allowed.ToArray() ) )
                    {
                        Walking(creature, new[] { Direction.East, Direction.North, Direction.West, Direction.South } );
                    }

                    break;
                }
            }

            bool Walking(Creature creature, Direction[] directions)
            {
                foreach (var direction in directions.Shuffle() )
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

                        return true;
                    }
                }

                return false;
            }
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(key);
        }
    }
}