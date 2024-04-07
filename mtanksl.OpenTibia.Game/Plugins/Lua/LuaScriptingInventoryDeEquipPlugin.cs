using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingInventoryDeEquipPlugin : InventoryDeEquipPlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingInventoryDeEquipPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingInventoryDeEquipPlugin(LuaScope script, LuaTable parameters)
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

        public override Promise OnDeEquip(Inventory inventory, Item item, byte slot)
        {
            if (fileName != null)
            {
                return script.CallFunction("ondeequip", inventory, item, slot);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["ondeequip"], inventory, item, slot);
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