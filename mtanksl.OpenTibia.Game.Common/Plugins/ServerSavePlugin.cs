using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class ServerSavePlugin : Plugin
    {
        public abstract Promise OnSave();
    }
}