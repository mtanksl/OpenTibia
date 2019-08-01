using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class InventoryReplaceItemCommand : Command
    {
        public InventoryReplaceItemCommand(Inventory inventory, Item fromItem, Item toItem)
        {
            Inventory = inventory;

            FromItem = fromItem;

            ToItem = toItem;
        }

        public Inventory Inventory { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            byte slot = Inventory.GetIndex(FromItem);

            //Act

            Inventory.ReplaceContent(slot, ToItem);

            //Notify

            context.Write(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)slot, ToItem ) );

            //Event

            if (server.Events.InventoryRemoveItem != null)
            {
                server.Events.InventoryRemoveItem(this, new InventoryRemoveItemEventArgs(FromItem, Inventory, slot, server, context) );
            }

            if (server.Events.InventoryAddItem != null)
            {
                server.Events.InventoryAddItem(this, new InventoryAddItemEventArgs(ToItem, Inventory, slot, server, context) );
            }

            base.Execute(server, context);
        }
    }
}