using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class ServerShutdownPlugin : Plugin
    {
        public abstract Promise OnShutdown();
    }
}