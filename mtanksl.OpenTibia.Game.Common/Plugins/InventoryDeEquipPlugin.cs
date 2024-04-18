using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class InventoryDeEquipPlugin : Plugin
    {
        public abstract Promise OnDeEquip(Inventory inventory, Item item, byte slot);
    }
}