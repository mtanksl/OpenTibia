using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class ApproachWalkStrategy : IWalkStrategy
    {
        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            if (target != null)
            {
                int deltaY = attacker.Tile.Position.Y - target.Tile.Position.Y;

                int deltaX = attacker.Tile.Position.X - target.Tile.Position.X;

                HashSet<Direction> directions = new HashSet<Direction>();

                if (deltaY < 0)
                {
                    directions.Add(Direction.South);
                }
                else if (deltaY > 0)
                {
                    directions.Add(Direction.North);
                }

                if (deltaX < 0)
                {
                    directions.Add(Direction.East);
                }
                else if (deltaX > 0)
                {
                    directions.Add(Direction.West);
                }

                if (directions.Count > 0)
                {
                    foreach (var direction in Context.Current.Server.Randomization.Shuffle(directions.ToArray() ) )
                    {
                        Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

                        if (toTile == null ||  toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) )
                        {

                        }
                        else
                        {
                            tile = toTile;

                            return true;
                        }
                    }
                }

                foreach (var direction in Context.Current.Server.Randomization.Shuffle(new[] { Direction.North, Direction.East, Direction.South, Direction.West } ) )
                {
                    Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

                    if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) )
                    {

                    }
                    else
                    {
                        tile = toTile;

                        return true;
                    }
                }
            }

            tile = null;

            return false;
        }
    }
}