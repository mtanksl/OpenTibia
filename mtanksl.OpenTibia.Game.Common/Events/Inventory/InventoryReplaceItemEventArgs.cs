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

        public Inventory Inventory { get; }

        public Item FromItem { get; }

        public Item ToItem { get; }

        public byte Slot { get; }
    }
}