using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class CreatureStepOutPlugin : Plugin
    {
        public abstract Promise OnStepOut(Creature creature, Tile fromTile, Tile toTile);
    }
}