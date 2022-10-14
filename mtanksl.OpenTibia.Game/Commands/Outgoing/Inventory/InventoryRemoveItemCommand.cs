using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class InventoryRemoveItemCommand : CommandResult<byte>
    {
        public InventoryRemoveItemCommand(Inventory inventory, Item item)
        {
            Inventory = inventory;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public Item Item { get; set; }

        public override PromiseResult<byte> Execute(Context context)
        {
            return PromiseResult<byte>.Run(resolve =>
            {
                byte slot = Inventory.GetIndex(Item);

                Inventory.RemoveContent(slot);

                context.AddPacket(Inventory.Player.Client.Connection, new SlotRemoveOutgoingPacket(slot) );

                resolve(context, slot);
            } );
        }
    }
}