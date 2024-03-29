using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingCreatureStepInPlugin : CreatureStepInPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingCreatureStepInPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingCreatureStepInPlugin(LuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath(fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/movements/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/lib.lua"));
            }
        }

        public override Promise OnStepIn(Creature creature, Tile toTile)
        {
            if (fileName != null)
            {
                return script.CallFunction("onstepin", creature, toTile);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onstepin"], creature, toTile);
            }           
        }

        public override void Stop()
        {
            if (fileName != null)
            {
                script.Dispose();
            }
        }       
    }
}