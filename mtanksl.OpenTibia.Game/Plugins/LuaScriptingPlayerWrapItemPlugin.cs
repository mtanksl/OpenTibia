﻿using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerWrapItemPlugin : PlayerWrapItemPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingPlayerWrapItemPlugin(string fileName, LuaTable parameters)
        {
            this.fileName = fileName;

            this.parameters = parameters;
        }

        public LuaScriptingPlayerWrapItemPlugin(ILuaScope script, LuaTable parameters)
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

        public override PromiseResult<bool> OnWrapItem(Player player, Item item)
        {
            if (fileName != null)
            {
                return script.CallFunction("onwrapitem", player, item).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onwrapitem"], player, item).Then(result =>
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