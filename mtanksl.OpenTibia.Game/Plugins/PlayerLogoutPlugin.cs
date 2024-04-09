using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerLogoutPlugin : Plugin
    {
        public abstract Promise OnLogout(Player player, Tile fromTile);
    }
}