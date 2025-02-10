using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class NpcCreationPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnStart(Npc npc);

        public abstract PromiseResult<bool> OnStop(Npc npc);
    }
}