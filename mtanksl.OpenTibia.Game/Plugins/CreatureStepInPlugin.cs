using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public abstract class CreatureStepInPlugin : Plugin
    {
        public abstract Promise OnStepIn(Creature creature, Tile toTile);
    }
}