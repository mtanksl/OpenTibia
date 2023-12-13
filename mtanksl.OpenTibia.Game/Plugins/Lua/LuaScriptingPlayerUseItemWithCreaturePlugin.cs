using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerUseItemWithCreaturePlugin : PlayerUseItemWithCreaturePlugin
    {
        private string fileName;

        public LuaScriptingPlayerUseItemWithCreaturePlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/actions/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> OnUseItemWithCreature(Player player, Item item, Creature toCreature)
        {
            return script.CallFunction("onuseitemwithcreature", player, item, toCreature).Then(result =>
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