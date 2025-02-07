using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class CreatureKillPlugin : Plugin
    {
        public abstract Promise OnKill(Creature creature, Creature target);
    }
}