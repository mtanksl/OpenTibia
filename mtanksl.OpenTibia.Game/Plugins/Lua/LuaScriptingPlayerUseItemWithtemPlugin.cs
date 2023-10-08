using OpenTibia.Common.Objects;
using OpenTibia.Game;
using OpenTibia.Game.Commands;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerUseItemWithtemPlugin : PlayerUseItemWithItemPlugin
    {
        private string fileName;

        public LuaScriptingPlayerUseItemWithtemPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/actions/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> OnUseItemWithItem(Player player, Item item, Item toItem)
        {
            return script.CallFunction("onuseitemwithitem", player, item, toItem).Then(result =>
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