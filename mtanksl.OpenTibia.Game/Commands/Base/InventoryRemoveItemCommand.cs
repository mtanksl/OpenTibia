using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class InventoryRemoveItemCommand : Command
    {
        public InventoryRemoveItemCommand(Inventory inventory, byte slot)
        {
            Inventory = inventory;

            Slot = slot;
        }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            Inventory.RemoveContent(Slot);

            //Notify

            context.Write(Inventory.Player.Client.Connection, new SlotRemoveOutgoingPacket( (Slot)Slot) );

            base.Execute(server, context);
        }
    }
}