using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingCreatureStepOutPlugin : CreatureStepOutPlugin
    {
        private string fileName;

        public LuaScriptingCreatureStepOutPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/movements/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override Promise OnStepOut(Creature creature, Tile fromTile)
        {
            return script.CallFunction("onstepout", creature, fromTile);
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}