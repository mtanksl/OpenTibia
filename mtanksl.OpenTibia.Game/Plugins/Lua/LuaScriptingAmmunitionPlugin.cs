using mtanksl.OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingAmmunitionPlugin : AmmunitionPlugin
    {
        private string fileName;

        public LuaScriptingAmmunitionPlugin(string fileName, Ammunition ammunition) : base(ammunition) 
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/ammunitions/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );

            Ammunition.Callback = (player, creature, weapon, ammunition) =>
            {
                return script.CallFunction("onuseammunition", player, creature, weapon, ammunition);
            };
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}