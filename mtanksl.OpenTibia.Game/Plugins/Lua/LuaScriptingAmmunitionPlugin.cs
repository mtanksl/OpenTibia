using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;

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
        }

        public override Promise OnUseAmmunition(Player player, Creature target, Item weapon, Item ammunition)
        {
            return script.CallFunction("onuseammunition", player, target, weapon, ammunition);
        }

        public override void Stop()
        {
            script.Dispose();
        }       
    }
}