using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface ITileRemoveItemScript : IScript
    {
        void OnTileRemoveItem(Item item, Tile fromTile, byte fromIndex, Server server, CommandContext context);
    }
}