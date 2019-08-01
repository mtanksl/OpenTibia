using OpenTibia.Common.Objects;
using OpenTibia.Game;

namespace OpenTibia.Common.Events
{
    public class InventoryAddItemEventArgs : GameEventArgs
    {
        public InventoryAddItemEventArgs(Item item, Inventory inventory, byte slot, Server server, Context context) : base(server, context)
        {
            Item = item;

            Inventory = inventory;

            Slot = slot;
        }

        public Item Item { get; set; }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }
    }
}