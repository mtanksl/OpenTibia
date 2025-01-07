using NLua;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingServerSavePlugin : ServerSavePlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingServerSavePlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingServerSavePlugin(ILuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/globalevents/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/globalevents/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnSave()
        {
            if (fileName != null)
            {
                return script.CallFunction("onsave");
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onsave"] );
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