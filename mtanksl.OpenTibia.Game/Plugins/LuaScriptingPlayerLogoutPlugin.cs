using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerLogoutPlugin : PlayerLogoutPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingPlayerLogoutPlugin(string fileName, LuaTable parameters)
        {
            this.fileName = fileName;

            this.parameters = parameters;
        }

        public LuaScriptingPlayerLogoutPlugin(ILuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/creaturescripts/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/creaturescripts/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnLogout(Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("onlogout", player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onlogout"], player);
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