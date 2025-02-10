using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingItemCreationPlugin : ItemCreationPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingItemCreationPlugin(string fileName, LuaTable parameters)
        {
            this.fileName = fileName;

            this.parameters = parameters;
        }

        public LuaScriptingItemCreationPlugin(ILuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/items/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/items/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override PromiseResult<bool> OnStart(Item item)
        {
            if (fileName != null)
            {
                return script.CallFunction("onstart", item).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onstart"], item).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }  
        }

        public override PromiseResult<bool> OnStop(Item item)
        {
            if (fileName != null)
            {
                return script.CallFunction("onstop", item).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onstop"], item).Then(result =>
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