using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerSayPlugin : PlayerSayPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingPlayerSayPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingPlayerSayPlugin(LuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/talkactions/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/talkactions/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override PromiseResult<bool> OnSay(Player player, string message)
        {
            if (fileName != null)
            {
                return script.CallFunction("onsay", player, message).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onsay"], player, message).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
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