using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class InventoryRefreshItemCommand : Command
    {
        public InventoryRefreshItemCommand(Inventory inventory, Item item)
        {
            Inventory = inventory;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            byte slot = Inventory.GetIndex(Item);

            context.AddPacket(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket(slot, Item) );

            OnComplete(context);
        }
    }
}