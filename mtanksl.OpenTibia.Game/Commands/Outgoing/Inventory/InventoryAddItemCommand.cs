using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class InventoryAddItemCommand : Command
    {
        public InventoryAddItemCommand(Inventory inventory, byte slot, Item item)
        {
            Inventory = inventory;

            Slot = slot;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Inventory.AddContent(Item, Slot);

                Context.AddPacket(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket(Slot, Item ) );

                resolve();
            } );
        }
    }
}