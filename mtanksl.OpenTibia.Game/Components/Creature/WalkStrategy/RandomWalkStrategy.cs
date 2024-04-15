using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Components
{
    public class RandomWalkStrategy : IWalkStrategy
    {
        public static readonly RandomWalkStrategy Instance = new RandomWalkStrategy(3);
        
        private int radius;

        public RandomWalkStrategy(int radius)
        {
            this.radius = radius;
        }

        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            Direction[] randomDirections = new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West };

            foreach (var direction in Context.Current.Server.Randomization.Shuffle(randomDirections) )
            {
                Tile toTile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

                if (toTile == null || toTile.Ground == null || toTile.NotWalkable || toTile.BlockPathFinding || toTile.Block || (attacker is Monster && toTile.ProtectionZone) || Math.Abs(toTile.Position.X - attacker.Spawn.Position.X) > radius || Math.Abs(toTile.Position.Y - attacker.Spawn.Position.Y) > radius)
                {

                }
                else
                {
                    tile = toTile;

                    return true;
                }
            }

            tile = null;

            return false;
        }
    }
}