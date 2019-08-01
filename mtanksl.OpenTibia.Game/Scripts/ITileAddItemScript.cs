using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface ITileAddItemScript : IScript
    {
        void OnTileAddItem(Item item, Tile toTile, byte toIndex, Server server, CommandContext context);
    }
}