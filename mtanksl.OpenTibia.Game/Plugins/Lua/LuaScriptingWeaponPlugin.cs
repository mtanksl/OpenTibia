using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingWeaponPlugin : WeaponPlugin
    {
        private string fileName;

        public LuaScriptingWeaponPlugin(string fileName, Weapon weapon) : base(weapon) 
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/weapons/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
            return script.CallFunction("onuseweapon", player, target, weapon);
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}