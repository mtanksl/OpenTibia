using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
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

        public override Promise Execute()
        {          
            byte slot = Inventory.GetIndex(FromItem);

            Inventory.ReplaceContent(slot, ToItem);

            Context.AddPacket(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket(slot, ToItem ) );

            Context.AddEvent(new InventoryReplaceItemEventArgs(Inventory, FromItem, ToItem, slot) );

            return Promise.Completed;
        }
    }
}