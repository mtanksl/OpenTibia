using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class ServerRecordPlugin : Plugin
    {
        public abstract Promise OnRecord(uint count);
    }
}