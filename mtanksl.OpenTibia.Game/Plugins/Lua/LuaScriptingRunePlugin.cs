using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
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
        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            return script.CallFunction("onusingrune", player, target, tile, item).Then(result =>
            {
                return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            return script.CallFunction("onuserune", player, target, tile, item);
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}