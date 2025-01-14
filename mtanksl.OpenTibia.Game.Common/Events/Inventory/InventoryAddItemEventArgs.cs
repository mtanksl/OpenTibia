using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class InventoryAddItemEventArgs : GameEventArgs
    {
        public InventoryAddItemEventArgs(Inventory inventory, Item item, byte slot)
        {
            Inventory = inventory;
        
            Item = item;

            Slot = slot;
        }

        public Inventory Inventory { get; }

        public Item Item { get; }

        public byte Slot { get; }
    }
}