using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class InventoryRemoveItemCommand : Command
    {
        public InventoryRemoveItemCommand(Inventory inventory, Item item)
        {
            Inventory = inventory;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            byte slot = Inventory.GetIndex(Item);

            Inventory.RemoveContent(slot);

            context.AddPacket(Inventory.Player.Client.Connection, new SlotRemoveOutgoingPacket( (Slot)slot) );

            context.AddEvent(new InventoryRemoveItemEventArgs(Inventory, Item,  slot) );

            base.OnCompleted(context);
        }
    }
}