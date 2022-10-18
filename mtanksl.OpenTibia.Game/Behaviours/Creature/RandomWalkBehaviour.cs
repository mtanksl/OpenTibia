using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class RandomWalkBehaviour : PeriodicBehaviour
    {
        public RandomWalkBehaviour(int radius)
        {
            this.radius = radius;
        }

        private int radius;

        private Tile spawn;

        private Creature creature;

        private string key;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;

            key = "Random_Walk_Behaviour_" + creature.Id;
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
                    Walking(creature, spawn, radius, new[] { Direction.East, Direction.North, Direction.West, Direction.South } );

                    break;
                }
            }

            bool Walking(Creature creature, Tile spawn, int radius, Direction[] directions)
            {
                foreach (var direction in directions.Shuffle() )
                {
                    Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                    if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) || Math.Abs(toTile.Position.X - spawn.Position.X) > radius || Math.Abs(toTile.Position.Y - spawn.Position.Y) > radius)
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