using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerLoginPlugin : Plugin
    {
        public abstract Promise OnLogin(Player player, Tile toTile);
    }
}