using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IMap
    {
        Tile GetTile(Position position);
    }
}