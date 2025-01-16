using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerAdvanceLevelPlugin : Plugin
    {
        public abstract Promise OnAdvanceLevel(Player player, ushort fromLevel, ushort toLevel);
    }
}