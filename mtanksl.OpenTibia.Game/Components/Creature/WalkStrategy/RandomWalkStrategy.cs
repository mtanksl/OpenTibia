using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class RandomWalkStrategy : IWalkStrategy
    {
        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
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

            tile = null;

            return false;
        }
    }
}