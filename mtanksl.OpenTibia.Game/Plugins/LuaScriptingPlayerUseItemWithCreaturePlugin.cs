using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerUseItemWithCreaturePlugin : PlayerUseItemWithCreaturePlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingPlayerUseItemWithCreaturePlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingPlayerUseItemWithCreaturePlugin(LuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/actions/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/actions/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override PromiseResult<bool> OnUseItemWithCreature(Player player, Item item, Creature toCreature)
        {
            if (fileName != null)
            {
                return script.CallFunction("onuseitemwithcreature", player, item, toCreature).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onuseitemwithcreature"], player, item, toCreature).Then(result =>
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