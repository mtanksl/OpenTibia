using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class RaidPlugin : Plugin
    {
        public RaidPlugin(Raid raid)
        {
            Raid = raid;
        }

        public Raid Raid { get; }

        public abstract Promise OnRaid();
    }
}