using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IMap
    {
        Tile GetTile(Position position);

        IEnumerable<Tile> GetTiles();
    }
}