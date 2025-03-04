using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryRemoveItemScriptingHandler : EventHandler<InventoryRemoveItemEventArgs>
    {
        public override Promise Handle(InventoryRemoveItemEventArgs e)
        {
            InventoryDeEquipPlugin plugin = Context.Server.Plugins.GetInventoryDeEquipPlugin(e.Item);

            if (plugin != null)
            {
                return plugin.OnDeEquip(e.Inventory, e.Item, e.Slot);
            }            

            return Promise.Completed;
        }
    }
}