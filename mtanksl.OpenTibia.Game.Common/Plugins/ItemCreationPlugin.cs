using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class ItemCreationPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnStart(Item item);

        public abstract PromiseResult<bool> OnStop(Item item);
    }
}