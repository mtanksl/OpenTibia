using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Strategies
{
    public class RunAwayWalkStrategy : IWalkStrategy
    {
        public Tile GetNext(Tile spawn, Creature creature, Creature target)
        {
            Context context = Context.Current;

            int deltaY = creature.Tile.Position.Y - target.Tile.Position.Y;

            int deltaX = creature.Tile.Position.X - target.Tile.Position.X;

            HashSet<Direction> directions = new HashSet<Direction>();

            if (deltaY < 0)
            {
                directions.Add(Direction.North);
            }
            else if (deltaY > 0)
            {
                directions.Add(Direction.South);
            }

            if (deltaX < 0)
            {
                directions.Add(Direction.West);
            }
            else if (deltaX > 0)
            {
                directions.Add(Direction.East);
            }

            if (directions.Count > 0)
            {
                // Try running away

                foreach (var direction in context.Server.Randomization.Shuffle(directions.ToArray() ) )
                {
                    Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                    if (toTile == null || 
                        
                        toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || 
                        
                        toTile.GetCreatures().Any(c => c.Block) )
                    {

                    }
                    else
                    {
                        return toTile;
                    }
                }
            }

            // Otherwise, random walk

            foreach (var direction in context.Server.Randomization.Shuffle(new[] { Direction.North, Direction.East, Direction.South, Direction.West } ) )
            {
                Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                if (toTile == null || 
                    
                    toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || 
                    
                    toTile.GetCreatures().Any(c => c.Block) )
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