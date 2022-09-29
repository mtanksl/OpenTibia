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
            byte slot = Inventory.GetIndex(FromItem);

            Inventory.ReplaceContent(slot, ToItem);

            context.AddPacket(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)slot, ToItem ) );

            base.Execute(context);
        }
    }
}