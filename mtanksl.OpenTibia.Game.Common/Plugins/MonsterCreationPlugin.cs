using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class MonsterCreationPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnStart(Monster monster);

        public abstract PromiseResult<bool> OnStop(Monster monster);
    }
}