using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class CreatureDeathPlugin : Plugin
    {
        public abstract Promise OnDeath(Creature creature, Tile fromTile);
    }
}