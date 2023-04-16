using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class InventoryReplaceItemEventArgs : GameEventArgs
    {
        public InventoryReplaceItemEventArgs(Inventory inventory, Item fromItem, Item toItem, byte slot)
        {
            Inventory = inventory;

            FromItem = fromItem;

            ToItem = toItem;

            Slot = slot;
        }

        public Inventory Inventory { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }

        public byte Slot { get; set; }
    }
}