using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerLoginPlugin : Plugin
    {
        public abstract Promise OnLogin(Player player, Tile toTile);
    }
}