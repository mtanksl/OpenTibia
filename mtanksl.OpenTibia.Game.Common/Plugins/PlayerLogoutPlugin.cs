using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerLogoutPlugin : Plugin
    {
        public abstract Promise OnLogout(Player player, Tile fromTile);
    }
}