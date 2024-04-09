using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingCreatureStepOutPlugin : CreatureStepOutPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingCreatureStepOutPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingCreatureStepOutPlugin(LuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/movements/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/movements/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnStepOut(Creature creature, Tile fromTile, Tile toTile)
        {
            if (fileName != null)
            {
                return script.CallFunction("onstepout", creature, fromTile, toTile);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onstepout"], creature, fromTile, toTile);
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