using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            Inventory.AddContent(Slot, Item);

            //Notify

            context.Write(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)Slot, Item ) );

            //Event

            if (server.Events.InventoryAddItem != null)
            {
                server.Events.InventoryAddItem(this, new InventoryAddItemEventArgs(Item, Inventory, Slot, server, context) );
            }

            base.Execute(server, context);
        }
    }
}