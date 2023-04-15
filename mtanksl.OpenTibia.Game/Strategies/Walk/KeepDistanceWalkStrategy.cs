using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Strategies
{
    public class KeepDistanceWalkStrategy : IWalkStrategy
    {
        private int radius;

        public KeepDistanceWalkStrategy(int radius)
        {
            this.radius = radius;
        }

        public Tile GetNext(Server server, Tile spawn, Creature attacker, Creature target)
        {
            int deltaY = attacker.Tile.Position.Y - target.Tile.Position.Y;

            int deltaX = attacker.Tile.Position.X - target.Tile.Position.X;

            HashSet<Direction> directions = new HashSet<Direction>();

            if (deltaY < 0)
            {
                if (-deltaY > radius)
                {
                    directions.Add(Direction.South);
                }
                else if (-deltaY < radius)
                {
                    directions.Add(Direction.North);
                }                       
            }
            else if (deltaY > 0)
            {               
                if (deltaY > radius)
                {
                    directions.Add(Direction.North);
                }
                else if (deltaY < radius)
                {
                    directions.Add(Direction.South);
                }
            }

            if (deltaX < 0)
            {
                if (-deltaX > radius)
                {
                    directions.Add(Direction.East);
                }
                else if (-deltaX < radius)
                {
                    directions.Add(Direction.West);
                }
            }
            else if (deltaX > 0)
            {
                if (deltaX > radius)
                {
                    directions.Add(Direction.West);
                }
                else if (deltaX < radius)
                {
                    directions.Add(Direction.East);
                }
            }

            if (directions.Count > 0)
            {
                // Try keeping distance

                foreach (var direction in server.Randomization.Shuffle(directions.ToArray() ) )
                {
                    Tile toTile = server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

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
                
            foreach (var direction in server.Randomization.Shuffle(new[] { Direction.North, Direction.East, Direction.South, Direction.West } ) )
            {
                Tile toTile = server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

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