using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerMoveCreaturePlugin : PlayerMoveCreaturePlugin
    {
        private string fileName;

        public LuaScriptingPlayerMoveCreaturePlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/actions/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> OnMoveCreature(Player player, Creature creature, Tile toTile)
        {
            return script.CallFunction("onmovecreature", player, creature, toTile).Then(result =>
            {
                return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}