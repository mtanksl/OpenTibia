using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class ServerStartupPlugin : Plugin
    {
        public abstract Promise OnStartup();
    }
}