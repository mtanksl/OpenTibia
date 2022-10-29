using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System;
using System.Linq;

namespace OpenTibia.Game.Strategies
{
    public class RandomWalkStrategy : IWalkStrategy
    {
        private int radius;

        public RandomWalkStrategy(int radius)
        {
            this.radius = radius;
        }

        public Tile GetNext(Context context, Tile spawn, Creature creature, Creature target)
        {
            foreach (var direction in context.Server.Randomization.Shuffle(new[] { Direction.North, Direction.East, Direction.South, Direction.West } ) )
            {
                Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) || Math.Abs(toTile.Position.X - spawn.Position.X) > radius || Math.Abs(toTile.Position.Y - spawn.Position.Y) > radius)
                {

                }
                else
                {
                    return toTile;
                }
            }

            return null;
        }
    }
}