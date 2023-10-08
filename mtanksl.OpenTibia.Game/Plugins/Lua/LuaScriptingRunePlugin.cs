using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingRunePlugin : RunePlugin
    {
        private string fileName;

        public LuaScriptingRunePlugin(string fileName, Rune rune) : base(rune) 
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/runes/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );

            Rune.Condition = (player, target, tile, item) =>
            {
                return script.CallFunction("onusingrune", player, target, tile, item).Then(result =>
                {
                    return Promise.FromResult((bool)result[0]);
                } );
            };

            Rune.Callback = (player, target, tile, item) =>
            {
                return script.CallFunction("onuserune", player, target, tile, item);
            };
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}