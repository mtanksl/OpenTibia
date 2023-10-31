﻿using OpenTibia.Common.Objects;
using OpenTibia.Game;
using OpenTibia.Game.Commands;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerSayPlugin : PlayerSayPlugin
    {
        private string fileName;

        public LuaScriptingPlayerSayPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/talkactions/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> OnSay(Player player, string message)
        {
            return script.CallFunction("onsay", player, message).Then(result =>
            {
                return Promise.FromResult ( (bool)result[0] );
            } );
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}