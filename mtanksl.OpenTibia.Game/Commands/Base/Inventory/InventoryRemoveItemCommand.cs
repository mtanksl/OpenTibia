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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            byte slot = Inventory.GetIndex(Item);

            //Act

            Inventory.RemoveContent(slot);

            //Notify

            context.Write(Inventory.Player.Client.Connection, new SlotRemoveOutgoingPacket( (Slot)slot) );

            //Event

            if (server.Events.InventoryRemoveItem != null)
            {
                server.Events.InventoryRemoveItem(this, new InventoryRemoveItemEventArgs(Item, Inventory, slot, server, context) );
            }

            base.Execute(server, context);
        }
    }
}