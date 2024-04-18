using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingInventoryEquipPlugin : InventoryEquipPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingInventoryEquipPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingInventoryEquipPlugin(LuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/movements/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/movements/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnEquip(Inventory inventory, Item item, byte slot)
        {
            if (fileName != null)
            {
                return script.CallFunction("onequip", inventory, item, slot);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onequip"], inventory, item, slot);
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