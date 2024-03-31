using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingAmmunitionPlugin : AmmunitionPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingAmmunitionPlugin(string fileName, Ammunition ammunition) : base(ammunition)
        {
            this.fileName = fileName;
        }

        public LuaScriptingAmmunitionPlugin(LuaScope script, LuaTable parameters, Ammunition ammunition) : base(ammunition)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/ammunitions/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/ammunitions/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnUseAmmunition(Player player, Creature target, Item weapon, Item ammunition)
        {
            if (fileName != null)
            {
                return script.CallFunction("onuseammunition", player, target, weapon, ammunition);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onuseammunition"], player, target, weapon, ammunition);
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