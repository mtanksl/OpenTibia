using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerMoveCreaturePlugin : Plugin
    {
        public abstract PromiseResult<bool> OnMoveCreature(Player player, Creature creature, Tile toTile);
    }
}