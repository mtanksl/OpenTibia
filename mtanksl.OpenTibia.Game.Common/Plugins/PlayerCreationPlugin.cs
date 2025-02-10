using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerCreationPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnStart(Player player);

        public abstract PromiseResult<bool> OnStop(Player player);
    }
}