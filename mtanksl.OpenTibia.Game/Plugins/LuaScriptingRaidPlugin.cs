using NLua;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingRaidPlugin : RaidPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingRaidPlugin(string fileName, LuaTable parameters, Raid raid) : base(raid)
        {
            this.fileName = fileName;

            this.parameters = parameters;
        }

        public LuaScriptingRaidPlugin(ILuaScope script, LuaTable parameters, Raid raid) : base(raid)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/raids/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/raids/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnRaid()
        {
            if (fileName != null)
            {
                return script.CallFunction("onraid");
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onraid"] );
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