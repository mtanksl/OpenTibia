using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class InventoryEquipPlugin : Plugin
    {
        public abstract Promise OnEquip(Inventory inventory, Item item, byte slot);
    }
}