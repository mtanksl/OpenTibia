using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IMapGetTile
    {
        Tile GetTile(Position position);
    }
}