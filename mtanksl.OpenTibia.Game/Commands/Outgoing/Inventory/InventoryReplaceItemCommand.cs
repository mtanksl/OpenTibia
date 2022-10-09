using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class InventoryReplaceItemCommand : CommandResult<byte>
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

        public override PromiseResult<byte> Execute(Context context)
        {
            return PromiseResult<byte>.Run(resolve =>
            {
                byte slot = Inventory.GetIndex(FromItem);

                Inventory.ReplaceContent(slot, ToItem);

                context.AddPacket(Inventory.Player.Client.Connection, new SlotAddOutgoingPacket(slot, ToItem ) );

                resolve(context, slot);
            } );
        }
    }
}