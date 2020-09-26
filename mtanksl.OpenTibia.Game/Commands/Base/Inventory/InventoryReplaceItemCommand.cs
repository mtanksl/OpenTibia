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

        public override void Execute(Context context)
        {
            //Arrange

            byte slot = Inventory.GetIndex(FromItem);

            //Act

            Inventory.ReplaceContent(slot, ToItem);

            //Notify

            context.AddPacket(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)slot, ToItem ) );

            //Event

            if (context.Server.Events.InventoryRemoveItem != null)
            {
                context.Server.Events.InventoryRemoveItem(this, new InventoryRemoveItemEventArgs(Inventory, FromItem, slot) );
            }

            if (context.Server.Events.InventoryAddItem != null)
            {
                context.Server.Events.InventoryAddItem(this, new InventoryAddItemEventArgs(Inventory, ToItem, slot) );
            }

            base.Execute(context);
        }
    }
}