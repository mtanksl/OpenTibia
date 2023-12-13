using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingCreatureStepInPlugin : CreatureStepInPlugin
    {
        private string fileName;

        public LuaScriptingCreatureStepInPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/movements/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override Promise OnStepIn(Creature creature, Tile toTile)
        {
            return script.CallFunction("onstepin", creature, toTile);
        }

        public override void Stop()
        {
            script.Dispose();
        }       
    }
}