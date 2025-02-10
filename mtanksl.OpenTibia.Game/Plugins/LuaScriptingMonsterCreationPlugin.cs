using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingMonsterCreationPlugin : MonsterCreationPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingMonsterCreationPlugin(string fileName, LuaTable parameters)
        {
            this.fileName = fileName;

            this.parameters = parameters;
        }

        public LuaScriptingMonsterCreationPlugin(ILuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/monsters/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/monsters/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override PromiseResult<bool> OnStart(Monster monster)
        {
            if (fileName != null)
            {
                return script.CallFunction("onstart", monster).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onstart"], monster).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }  
        }

        public override PromiseResult<bool> OnStop(Monster monster)
        {
            if (fileName != null)
            {
                return script.CallFunction("onstop", monster).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onstop"], monster).Then(result =>
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