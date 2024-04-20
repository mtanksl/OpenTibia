using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class ApproachWalkStrategy : IWalkStrategy
    {
        public static readonly ApproachWalkStrategy Instance = new ApproachWalkStrategy();

        private ApproachWalkStrategy()
        {
            
        }

        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            int deltaY = attacker.Tile.Position.Y - target.Tile.Position.Y;

            int deltaX = attacker.Tile.Position.X - target.Tile.Position.X;

            HashSet<Direction> randomDirections = new HashSet<Direction>() { Direction.North, Direction.East, Direction.South, Direction.West };

            HashSet<Direction> directions = new HashSet<Direction>();

            if (deltaY < 0)
            {
                directions.Add(Direction.South);

                randomDirections.Remove(Direction.South);
            }
            else if (deltaY > 0)
            {
                directions.Add(Direction.North);

                randomDirections.Remove(Direction.North);
            }

            if (deltaX < 0)
            {
                directions.Add(Direction.East);

                randomDirections.Remove(Direction.East);
            }
            else if (deltaX > 0)
            {
                directions.Add(Direction.West);

                randomDirections.Remove(Direction.West);
            }

            foreach (var collection in new[] { directions, randomDirections } )
            {
                if (collection.Count > 0)
                {
                    foreach (var direction in Context.Current.Server.Randomization.Shuffle(collection.ToArray() ) )
                    {
                        Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

                        if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.BlockPathFinding || toTile.Block || (attacker is Monster && toTile.ProtectionZone) )
                        {

                        }
                        else
                        {
                            tile = toTile;

                            return true;
                        }
                    }
                }
            }

            tile = null;

            return false;
        }
    }
}