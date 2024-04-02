using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Plugins
{
    public abstract class InventoryEquipPlugin : Plugin
    {
        public abstract Promise OnEquip(Inventory inventory, Item item, byte slot);
    }
}