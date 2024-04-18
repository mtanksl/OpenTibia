using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingWeaponPlugin : WeaponPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingWeaponPlugin(string fileName, Weapon weapon) : base(weapon)
        {
            this.fileName = fileName;
        }

        public LuaScriptingWeaponPlugin(LuaScope script, LuaTable parameters, Weapon weapon) : base(weapon)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/weapons/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/weapons/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnUseWeapon(Player player, Creature target, Item weapon)
        {
            if (fileName != null)
            {
                return script.CallFunction("onuseweapon", player, target, weapon);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onuseweapon"], player, target, weapon);
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