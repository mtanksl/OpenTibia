using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryAddItemScriptingHandler : EventHandler<InventoryAddItemEventArgs>
    {
        public override Promise Handle(InventoryAddItemEventArgs e)
        {
            InventoryEquipPlugin plugin = Context.Server.Plugins.GetInventoryEquipPlugin(e.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnEquip(e.Inventory, e.Item, e.Slot);
            }

            return Promise.Completed;
        }
    }
}